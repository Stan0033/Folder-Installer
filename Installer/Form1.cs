using System;
using System.Collections.Generic;

using System.Drawing;

using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Microsoft.Win32;
using System.Diagnostics;

using IWshRuntimeLibrary;
using File = System.IO.File;

using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading;

namespace Installer
{
    public partial class Form1 : Form
    {
        string header_path;
        string Installation_Name;
        string install_contents_folderName;
        string uninstall_bat;
        string Selected_Path; // selected + nameof Installation_Name
        string autorun;
        string shortcut;

        string IConPathSource;
        string IConPathInstalled;
        string Path_ExtractedFolder = string.Empty;
        public CancellationTokenSource cancellationTokenSource;
        bool Installed = false;
        string SoundFileName = string.Empty;
        bool hasSound = false;
        List<string> imageFiles;
        SoundPlayer backgroundMusicPlayer;
        List<string> listfiles;
        public Form1()
        {
            InitializeComponent();
            header_path = Path.Combine(Environment.CurrentDirectory, "header.jpg");
            install_contents_folderName = Path.Combine(Environment.CurrentDirectory, "content");
            autorun = Path.Combine(Environment.CurrentDirectory, "autorun.inf");
            Path_ExtractedFolder = Path.Combine(Environment.CurrentDirectory, "content");
            shortcut = string.Empty;
            listfiles = new List<string>();
            imageFiles = new List<string>();

            imageFiles = GetJpgFilesInFolder(Path.Combine(Environment.CurrentDirectory, "pics")); ;
            if (imageFiles.Count > 0)
            {
                button_viewImages.Visible = true;
            }


        }
        public bool IsThereIcon()
        {
            return File.Exists(Path.Combine(Environment.CurrentDirectory, "icon.ico"));
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            IConPathSource = Path.Combine(Environment.CurrentDirectory, "icon.ico");

            Installation_Name = ExtractApplicationNameFromAutorun();
            Text = Installation_Name == "Installer" ? Installation_Name : "Install " + Installation_Name;

            shortcut = ExtractAutorunComment(autorun);
            string pathShortcut = Path.Combine(Path_ExtractedFolder, shortcut);
            if (!File.Exists(pathShortcut)) { check_Shortcut.Enabled = false; }
            check_Shortcut.Enabled = (shortcut != string.Empty);

            //textBox1.Text = GetProgramFilesX86Path();
            LoadImage();
            if (!Directory.Exists(install_contents_folderName)) { MessageBox.Show($"{install_contents_folderName} folder missing"); button_install.Enabled = false; }
            else
            {
                if (IsFolderEmpty(install_contents_folderName))
                {
                    MessageBox.Show($"{install_contents_folderName} folder is empty");
                    button_install.Enabled = false;
                    check_Uninstall.Enabled = false;
                    check_Shortcut.Enabled = false;
                    check_openFolder.Enabled = false;
                }
            }
            comboBox2.Text = "Games";
            FillDiskLettersComboBox();



            Installed = RegistryKeyExists(Installation_Name);
            if (Installed)
            {
                button_install.Visible = false;
                button_uninstall.Visible = true;
                check_openFolder.Enabled = false;
                check_Shortcut.Enabled = false;
                check_Uninstall.Enabled = false;
            }
            else
            {
                button_install.Visible = true;
                button_uninstall.Visible = false;
            }
            SoundFileName = "music.wav";
            hasSound = IsValidSoundFile(Environment.CurrentDirectory, SoundFileName);
            if (hasSound)
            {
                button_stopMusic.Visible = true;
                string musicFileFullPath = Path.Combine(Environment.CurrentDirectory, SoundFileName);

                PlayBackgroundMusic(musicFileFullPath);
            }
        }
        static bool IsFolderEmpty(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath);
            string[] subDirectories = Directory.GetDirectories(folderPath);

            return (files.Length == 0 && subDirectories.Length == 0);
        }
        private void FillDiskLettersComboBox()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                comboBox1.Items.Add(drive.Name);
            }

            // Find the drive where the OS is installed
            string osDrive = Path.GetPathRoot(Environment.SystemDirectory);
            comboBox1.SelectedItem = osDrive;
        }
        static string ExtractApplicationNameFromAutorun()
        {
            string autorunFilePath = "autorun.inf"; // Replace with the actual path

            if (File.Exists(autorunFilePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(autorunFilePath);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("label=", StringComparison.OrdinalIgnoreCase))
                        {
                            string appName = line.Substring("label=".Length).Trim();
                            return string.IsNullOrEmpty(appName) ? "Installer" : appName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("autorun.inf file not found.");
            }

            return "Installer";
        }
        private void BrowseFolder_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedPath = folderBrowserDialog.SelectedPath.Trim();

                    if (Directory.Exists(selectedPath))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(selectedPath);
                        if ((directoryInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                        {
                            MessageBox.Show("Selected folder is read-only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            SplitPath(selectedPath);
                            Selected_Path = Path.Combine(selectedPath, Installation_Name);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Selected path does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void SplitPath(string inputPath)
        {

            string driveLetter = Path.GetPathRoot(inputPath);
            string remainingPath = inputPath.Substring(driveLetter.Length);

            comboBox1.Text = driveLetter;

            comboBox2.Text = remainingPath;

        }
        static string GetProgramFilesX86Path()
        {
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            return programFilesPath;
        }
        private void LoadImage()
        {
            // MessageBox.Show(header_path);

            if (File.Exists(header_path))
            {
                Image img = Image.FromFile(header_path);
                if (img.Width == 460 && img.Height == 215)
                {

                    previewer.BackgroundImage = img;
                    previewer.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else { MessageBox.Show("header.jpg does not have the required resolution (460x215).", "Resolution Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }


            }
            else
            {
                MessageBox.Show("header.jpg not found.", "Image Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button_install.Enabled = false;
            }

        }

        private void Install_Click(object sender, EventArgs e)
        {

             Selected_Path = Path.Combine(Path.Combine(comboBox1.Text, comboBox2.Text), Installation_Name);
            if (button_install.Text == "Install")
            {
                cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = cancellationTokenSource.Token;
                if (CheckFolder(Path.Combine(comboBox1.Text, comboBox2.Text.Trim())))
                {
                    CreateFolder(Installation_Name, Path.Combine(comboBox1.Text, comboBox2.Text.Trim()));

                    string target = Path.Combine(Path.Combine(comboBox1.Text, comboBox2.Text.Trim()), Installation_Name);
                    //MessageBox.Show(target + "\n" + install_contents);
                    //   File.WriteAllText(Path.Combine(target,"uninstall.bat"), uninstall_bat);

                    if (CheckForEnoughSpace(target, install_contents_folderName))
                    {

                        button_install.Text = "Stop";
                        //ExtractTarContents (target, install_contents);
                        listfiles = ListFiles(Path_ExtractedFolder, 20);
                        CopyICon();
                        CopyFilesAsync(listfiles, target, cancellationToken);


                    }
                    else
                    {
                        MessageBox.Show("Not enough space on the target location.");
                    }

                }
            }

            else if (button_install.Text == "Stop")

            {

                button_install.Enabled = false;
                CancelInstall();
                Directory.Delete(Selected_Path);
            }



        }
        private void CreateFolder(string folderName, string folderPath)
        {
            try
            {
                string newFolderPath = Path.Combine(folderPath, folderName);
                Directory.CreateDirectory(newFolderPath);

                // Make sure the folder is not read-only
                DirectoryInfo dirInfo = new DirectoryInfo(newFolderPath);
                dirInfo.Attributes &= ~FileAttributes.ReadOnly;

                //MessageBox.Show($"Folder '{folderName}' created at '{newFolderPath}'.", "Folder Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static bool CheckFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {

                return true; // Folder exists and is not read-only

            }
            else
            {
                Directory.CreateDirectory(folderPath);
                return true; // Folder doesn't exist or is read-only
            }
        }





        private void button3_Click(object sender, EventArgs e)
        {

            comboBox2.Text = string.Empty;
        }
        static string ConvertToValidFilename(string input)
        {
            // Define a regular expression pattern to match invalid characters
            string invalidCharsPattern = "[\\/:*?\"<>|]";

            // Replace colons with " - "
            string cleanedInput = input.Replace(":", " - ");

            // Replace other invalid characters with whitespaces
            cleanedInput = Regex.Replace(cleanedInput, invalidCharsPattern, " ");

            return cleanedInput;
        }





        static string ExtractAutorunComment(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (trimmedLine.StartsWith(";"))
                    {
                        // Remove the leading semicolon and any leading/trailing whitespace
                        return ConvertToValidFilename(trimmedLine.Substring(1).Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return string.Empty; // No comment found
        }
        static void CreateShortcut(string filePath)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string shortcutName = Path.GetFileNameWithoutExtension(filePath);
                string shortcutPath = Path.Combine(desktopPath, $"{shortcutName}.lnk");

                // Extract the first met line comment from autorun.inf
                string comment = ExtractAutorunComment("autorun.inf");

                // Create a shortcut
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = filePath;
                shortcut.Description = comment; // Set the comment as the description
                shortcut.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        static bool CheckForEnoughSpace(string targetPath, string myZip)
        {
            try
            {
                DriveInfo drive = new DriveInfo(Path.GetPathRoot(targetPath));
                long availableSpace = drive.AvailableFreeSpace;
                long requiredSpace = GetFolderSize(targetPath);

                return availableSpace >= requiredSpace;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        static long GetFolderSize(string folderPath)
        {
            long totalSize = 0;

            foreach (string filePath in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                totalSize += fileInfo.Length;
            }

            return totalSize;
        }

        static List<string> ListFiles(string folderPath, int maxNestingLevel)
        {
            List<string> results = new List<string>();
            ListFilesRecursive(folderPath, folderPath, results, maxNestingLevel, 0);
            return results;
        }

        static void ListFilesRecursive(string rootPath, string folderPath, List<string> results, int maxNestingLevel, int currentLevel)
        {
            if (currentLevel > maxNestingLevel)
                return;

            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                string relativePath = filePath.Substring(rootPath.Length).TrimStart('\\');
                string fullPath = Path.Combine(rootPath, relativePath); // Create full path
                results.Add(fullPath); // Include full path
            }

            foreach (string subFolderPath in Directory.GetDirectories(folderPath))
            {
                ListFilesRecursive(rootPath, subFolderPath, results, maxNestingLevel, currentLevel + 1);
            }
        }

        private void CancelInstall()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
        }
        public void CopyICon()
        {
            if (IsThereIcon())
            {
              IConPathSource = Path.Combine(Environment.CurrentDirectory, "icon.ico");
              IConPathInstalled = Path.Combine(Selected_Path, "icon.ico");
                if (!File.Exists(IConPathInstalled)){
                    File.Copy(IConPathSource, Path.Combine(Selected_Path, "icon.ico"));

                }
                

            }
        }
        public async Task CopyFilesAsync(List<string> files, string targetFolder, CancellationToken cancellationToken)
        {
            int totalFiles = files.Count;
            progressBar1.Maximum = totalFiles; // Set the maximum value of ProgressBar1
            int copiedFiles = 0;
            bool filesMissing = false;
          
                foreach (string filePath in files)
            {
                //  MessageBox.Show($"Copying: {filePath}");
                if (cancellationToken.IsCancellationRequested)
                {
                    MessageBox.Show("File copy operation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Source file missing: {filePath}");
                    filesMissing = true;
                    break;
                }

                string relativePath = Path.GetFullPath(filePath).Substring(Path.GetFullPath("content").Length + 1);
                string targetPath = Path.Combine(targetFolder, relativePath);

                //   MessageBox.Show($"Relative path: {relativePath}");
                //   MessageBox.Show($"Target path: {targetPath}");

                try
                {
                    string targetDir = Path.GetDirectoryName(targetPath);
                    Directory.CreateDirectory(targetDir);

                    await Task.Run(() => File.Copy(filePath, targetPath, true));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error copying {filePath} to {targetPath}: {ex.Message}");
                    filesMissing = true;
                    break;
                }

                copiedFiles++;
                UpdateProgressBar(copiedFiles); // Update ProgressBar1
                await Task.Delay(10); // Simulate some work
                Application.DoEvents(); // Update UI to show progress
            }

            if (filesMissing)
            {
                MessageBox.Show("One or more files are missing or failed to copy. Installation cannot continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Installation completed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                StopBackgroundMusic();
                button_stopMusic.Visible = false;
                button_install.Enabled = false;
                if (check_Uninstall.Checked)
                {
                    string target = Path.Combine(Path.Combine(comboBox1.Text, comboBox2.Text), Installation_Name);

                    CreateDeleteContentsAndFolderBatch(target, Installation_Name);

                    // MessageBox.Show(Installation_Name);
                    string uninstall_path = Path.Combine(target, "Uninstall.bat");
                    AddProgramToUninstall(Installation_Name, uninstall_path);
                }
                if (check_openFolder.Checked)
                {
                    string target = Path.Combine(Path.Combine(comboBox1.Text, comboBox2.Text), Installation_Name);
                    Process.Start("explorer.exe", target);
                }
                if (check_Shortcut.Checked)
                {
                    string shortcut_path = Path.Combine(Selected_Path, shortcut);
                    CreateShortcut(shortcut_path);
                }

            }
        }

        private void UpdateProgressBar(int value)
        {
            // Update ProgressBar1 with the provided value
            // Example: progressBar.Value = value;
            progressBar1.Value = value;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            listfiles = ListFiles(Path_ExtractedFolder, 20);
            MessageBox.Show(string.Join("\n", listfiles));
        }


        public void CreateDeleteContentsAndFolderBatch(string path, string name)
        {
            string batchScript = $@"
 @echo off
setlocal

rem Get the current script's directory
set ""ScriptDir=%~dp0""

rem Delete the contents of the folder recursively
for /r ""%ScriptDir%"" %%i in (*) do (
    if ""%%i"" neq ""%~f0"" (
        if exist ""%%i"" (
            if not exist ""%%i\"" (
                del /q ""%%i""
            )
        )
    )
)

rem Remove the uninstall entry from the registry
reg delete ""HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{name}"" /f

rem Return to the parent directory
cd ..

rem Delete the folder itself
rd /s /q ""%ScriptDir%""

endlocal
";

            string batchFilePath = Path.Combine(path, $"Uninstall.bat");
            File.WriteAllText(batchFilePath, batchScript);
        }



        public void AddProgramToUninstall(string displayName, string uninstallString)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall", true))
                {
                    if (key != null)
                    {
                        using (RegistryKey subKey = key.CreateSubKey(displayName))
                        {
                            if (subKey != null)
                            {
                                subKey.SetValue("DisplayName", displayName);
                                subKey.SetValue("UninstallString", "\"" + uninstallString + "\"  /UNINSTALL");
                                subKey.SetValue("NoModify", 1);
                                subKey.SetValue("NoRepair", 1);
                                if (IsThereIcon())
                                {
                                     
                                    subKey.SetValue("DisplayIcon", IConPathInstalled);
                                }


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create uninstall in registry");
            }
        }
        static bool RegistryKeyExists(string keyName)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
            {
                if (key != null)
                {
                    return key.GetSubKeyNames().Contains(keyName);
                }
                return false;
            }
        }
        static bool IsValidSoundFile(string appFolderPath, string soundFileName)
        {
            string soundFilePath = Path.Combine(appFolderPath, soundFileName);

            if (File.Exists(soundFilePath))
            {
                try
                {
                    using (SoundPlayer soundPlayer = new SoundPlayer(soundFilePath))
                    {
                        // Attempt to load the sound file
                        soundPlayer.Load();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading sound file: {ex.Message}");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Sound file not found.");
                return false;
            }
        }
        void PlayBackgroundMusic(string musicFileName)
        {

            if (File.Exists(musicFileName))
            {
                try
                {
                    backgroundMusicPlayer = new SoundPlayer(musicFileName);
                    backgroundMusicPlayer.PlayLooping();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error playing background music: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Background music file not found.");
            }
        }

        void StopBackgroundMusic()
        {
            if (backgroundMusicPlayer != null)
            {
                backgroundMusicPlayer.Stop();
                backgroundMusicPlayer.Dispose();
            }
            button_stopMusic.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopBackgroundMusic();

        }

        private void button_uninstall_Click(object sender, EventArgs e)
        {
            // find the registry key and run uninstall.bat 
            bool success = ExecuteUninstallEntry(Installation_Name);
            if (success) { MessageBox.Show("Uninstalled successfully."); button_uninstall.Enabled = false; StopBackgroundMusic(); button_stopMusic.Visible = false; }

        }

        static bool ExecuteUninstallEntry(string searchedName)
        {
            string uninstallKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey uninstallKey = Registry.CurrentUser.OpenSubKey(uninstallKeyPath, true))
            {
                if (uninstallKey != null)
                {
                    foreach (string subKeyName in uninstallKey.GetSubKeyNames())
                    {
                        using (RegistryKey subKey = uninstallKey.OpenSubKey(subKeyName, true))
                        {
                            string displayName = subKey.GetValue("DisplayName") as string;
                            if (displayName != null && displayName.Equals(searchedName, StringComparison.OrdinalIgnoreCase))
                            {
                                string uninstallString = subKey.GetValue("UninstallString") as string;
                                if (!string.IsNullOrEmpty(uninstallString))
                                {
                                    int startIndex = uninstallString.IndexOf("\"") + 1;
                                    int endIndex = uninstallString.LastIndexOf("\"");
                                    if (startIndex > -1 && endIndex > startIndex)
                                    {
                                        string command = uninstallString.Substring(startIndex, endIndex - startIndex);
                                        if (System.IO.File.Exists(command))
                                        {
                                            try
                                            {
                                                System.Diagnostics.Process.Start(command);
                                                return true;
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show($"Error executing UninstallString: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                        else
                                        {
                                            uninstallKey.DeleteSubKey(subKeyName);
                                            MessageBox.Show($"Uninstall entry for '{searchedName}' removed due to missing executable.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid UninstallString format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    uninstallKey.DeleteSubKey(subKeyName);
                                    MessageBox.Show($"Uninstall entry for '{searchedName}' removed due to missing UninstallString.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return true;
                                }
                                return false;
                            }
                        }
                    }

                    MessageBox.Show($"Uninstall entry with name '{searchedName}' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show("Uninstall registry key not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        static List<string> GetJpgFilesInFolder(string folderPath)
        {
            List<string> jpgFiles = new List<string>();

            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath, "*.jpg");

                foreach (string file in files)
                {
                    jpgFiles.Add(file);
                }
            }
            else
            {
                Console.WriteLine("Folder does not exist.");
            }

            return jpgFiles;
        }

        private void button_viewImages_Click(object sender, EventArgs e)
        {
            var v = new Form2(imageFiles);
            v.ShowDialog();

        }
    }
}









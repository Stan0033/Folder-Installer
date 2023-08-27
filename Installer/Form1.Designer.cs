namespace Installer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.previewer = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button_install = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.check_openFolder = new System.Windows.Forms.CheckBox();
            this.check_Shortcut = new System.Windows.Forms.CheckBox();
            this.check_Uninstall = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button_uninstall = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.previewer)).BeginInit();
            this.SuspendLayout();
            // 
            // previewer
            // 
            this.previewer.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.previewer.Location = new System.Drawing.Point(12, 12);
            this.previewer.Name = "previewer";
            this.previewer.Size = new System.Drawing.Size(460, 215);
            this.previewer.TabIndex = 0;
            this.previewer.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(432, 233);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_install
            // 
            this.button_install.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_install.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_install.Location = new System.Drawing.Point(336, 330);
            this.button_install.Name = "button_install";
            this.button_install.Size = new System.Drawing.Size(135, 55);
            this.button_install.TabIndex = 3;
            this.button_install.Text = "Install";
            this.button_install.UseVisualStyleBackColor = true;
            this.button_install.Click += new System.EventHandler(this.Install_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 267);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(459, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 233);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(41, 32);
            this.comboBox1.TabIndex = 5;
            // 
            // comboBox2
            // 
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Games"});
            this.comboBox2.Location = new System.Drawing.Point(59, 233);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(367, 32);
            this.comboBox2.TabIndex = 6;
            // 
            // check_openFolder
            // 
            this.check_openFolder.AutoSize = true;
            this.check_openFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.check_openFolder.Location = new System.Drawing.Point(12, 305);
            this.check_openFolder.Name = "check_openFolder";
            this.check_openFolder.Size = new System.Drawing.Size(256, 20);
            this.check_openFolder.TabIndex = 7;
            this.check_openFolder.Text = "Open folder when extraction completes";
            this.check_openFolder.UseVisualStyleBackColor = true;
            // 
            // check_Shortcut
            // 
            this.check_Shortcut.AutoSize = true;
            this.check_Shortcut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.check_Shortcut.Location = new System.Drawing.Point(12, 331);
            this.check_Shortcut.Name = "check_Shortcut";
            this.check_Shortcut.Size = new System.Drawing.Size(167, 20);
            this.check_Shortcut.TabIndex = 8;
            this.check_Shortcut.Text = "Create desktop shortcut";
            this.check_Shortcut.UseVisualStyleBackColor = true;
            // 
            // check_Uninstall
            // 
            this.check_Uninstall.AutoSize = true;
            this.check_Uninstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.check_Uninstall.Location = new System.Drawing.Point(12, 357);
            this.check_Uninstall.Name = "check_Uninstall";
            this.check_Uninstall.Size = new System.Drawing.Size(152, 20);
            this.check_Uninstall.TabIndex = 9;
            this.check_Uninstall.Text = "Add to installed apps";
            this.check_Uninstall.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(447, 296);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(25, 21);
            this.button2.TabIndex = 10;
            this.button2.Text = "||";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_uninstall
            // 
            this.button_uninstall.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_uninstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_uninstall.Location = new System.Drawing.Point(287, 330);
            this.button_uninstall.Name = "button_uninstall";
            this.button_uninstall.Size = new System.Drawing.Size(184, 55);
            this.button_uninstall.TabIndex = 11;
            this.button_uninstall.Text = "Uninstall";
            this.button_uninstall.UseVisualStyleBackColor = true;
            this.button_uninstall.Click += new System.EventHandler(this.button_uninstall_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 390);
            this.Controls.Add(this.button_uninstall);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.check_Uninstall);
            this.Controls.Add(this.check_Shortcut);
            this.Controls.Add(this.check_openFolder);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button_install);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.previewer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.previewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox previewer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_install;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.CheckBox check_openFolder;
        private System.Windows.Forms.CheckBox check_Shortcut;
        private System.Windows.Forms.CheckBox check_Uninstall;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button_uninstall;
    }
}


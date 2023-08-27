using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Installer
{
    public partial class Form2 : Form
    {
        int currentPicNumber = 0;
        List<Image> images;
        public Form2()
        {
            InitializeComponent();
            images = new List<Image>();
        }
        public Form2(List<string> imageFiles)
        {
            InitializeComponent();
            images = new List<Image>();
            foreach (string file in imageFiles)
            {
                if (File.Exists(file))
                {
                    Image m = Image.FromFile(file);
                    images.Add(m);
                }
                
            }
            pictureBox1.Image = images[currentPicNumber];
            if (images.Count > 1)
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }
        public void processImages()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentPicNumber++;
            if (currentPicNumber == images.Count) { currentPicNumber = 0; } //for increase
            pictureBox1.Image = images[currentPicNumber];
             
        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentPicNumber--;
            if (currentPicNumber < 0) { currentPicNumber = images.Count-1; } //for decrese
            pictureBox1.Image = images[currentPicNumber];
        }
    }
}

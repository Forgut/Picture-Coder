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

namespace PictureCoding
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string imageLocation = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| bnp files(*.bmp)|*.bmp| All Files(*.*)|*.*";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    pictureBox1.ImageLocation = imageLocation;
                }

            }
            catch
            {
                MessageBox.Show("Error", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string fillTo8bits(string input)
        {
            int length = input.Length;
            for (int i = 0; i < 8 - length; i++)
            {
                input = '0' + input;
            }
            return input;
        }
        private string fillToMod3(string input)
        {
            int length = input.Length;
            for (int i = 0; i < 3 - length % 3; i++)
                input += '0';
            return input;
        }

        private char createChar(string input)
        {
            int output = 0;
            for (int i = 7; i >= 0; i--)
            {
                try
                {
                    if (input[i] == '1')
                        output += (int)Math.Pow(2, 7-i);
                }
                catch (Exception e)
                {

                }
            }
            return (char)output;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap bmap = new Bitmap(pictureBox1.ImageLocation, true);
            int size = bmap.Width > bmap.Height ? bmap.Height : bmap.Width;
            string message = textBox1.Text;
            int pixelsCount = 0;
            string fullbinary = "";
            foreach (var c in message)
            {
                string binary = Convert.ToString(c, 2);
                fullbinary += fillTo8bits(binary);
            }
            fullbinary = fillToMod3(fullbinary);
            for (int i = 0; i < fullbinary.Length; i += 3)
            {
                var x = pixelsCount / size;
                var y = pixelsCount % size;
                var px = bmap.GetPixel(pixelsCount / size, pixelsCount % size);
                var R = px.R;
                var G = px.G;
                var B = px.B;
                if (fullbinary[i] == '0' && px.R % 2 == 1)
                    R--;
                if (fullbinary[i] == '1' && px.R % 2 == 0)
                    R++;
                if (fullbinary[i + 1] == '0' && px.G % 2 == 1)
                    G--;
                if (fullbinary[i + 1] == '1' && px.G % 2 == 0)
                    G++;
                if (fullbinary[i + 2] == '0' && px.B % 2 == 1)
                    B--;
                if (fullbinary[i + 2] == '1' && px.B % 2 == 0)
                    B++;
                bmap.SetPixel(pixelsCount / size, pixelsCount % size, Color.FromArgb(255, R, G, B));

                pixelsCount++;
            }
            bmap.Save(pictureBox1.ImageLocation.Substring(0, pictureBox1.ImageLocation.Length - 4) + "_coded.png", System.Drawing.Imaging.ImageFormat.Png);
            
            pictureBox2.ImageLocation = pictureBox1.ImageLocation.Substring(0, pictureBox1.ImageLocation.Length - 4) + "_coded.png";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bmap = new Bitmap(pictureBox2.ImageLocation, true);
            int size = bmap.Width > bmap.Height ? bmap.Height : bmap.Width;
            int pixelsCount = 0;
            string fullbinary = "";
            for (int i = 0; i < bmap.Width * bmap.Height; i++)
            {
                var x = pixelsCount / size;
                var y = pixelsCount % size;
                var px = bmap.GetPixel(pixelsCount / size, pixelsCount % size);
                if (Convert.ToInt32(px.R) % 2 == 0)
                    fullbinary += '0';
                else
                    fullbinary += '1';
                if (Convert.ToInt32(px.G) % 2 == 0)
                    fullbinary += '0';
                else
                    fullbinary += '1';
                if (Convert.ToInt32(px.B) % 2 == 0)
                    fullbinary += '0';
                else
                    fullbinary += '1';
                pixelsCount++;
            }
            string output = "";
            for (int i = 0; i < fullbinary.Length; i += 8)
            {
                string temp = "";
                for (int j = i; j < i + 8; j++)
                {
                    try
                    {
                        temp += fullbinary[j];
                    }
                    catch
                    {

                    }
                }
                output += createChar(temp);
            }
            textBox2.Text = output;
        }



    }
}
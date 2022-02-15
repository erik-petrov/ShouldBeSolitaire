using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ShouldBeSolitaire
{
	public partial class StartForm : Form
	{
        RadioButton selected;
        Image chosenImage;
        public StartForm()
		{
			InitializeComponent();
            selected = radioButton2;
		}
        private void button1_Click(object sender, System.EventArgs e)
        {
            if(selected == null || chosenImage == null)
            {
                MessageBox.Show("Please select a diffculty or an image.");
                return;
            }
            if(selected.Text == "Custom")
            {
                int x = (int)numericUpDown1.Value;
                int y = (int)numericUpDown2.Value;
                Form1 game = new Form1(x, y, chosenImage);
                game.Show();
                this.Hide();
            }
            else
            {
                string[] strArr = selected.Text.Split(':');
                string[] coords = strArr[1].Split('x');
                int coord = Int32.Parse(coords[0].ToString());
                Console.WriteLine(coord);
                Form1 game = new Form1(coord, coord, chosenImage);
                game.Show();
                this.Hide();
            }
        }
        private void setSelectedRadio(object sender, System.EventArgs e)
        {
            RadioButton send = sender as RadioButton;
            selected = send;
        }
        private void button2_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog1.Filter = "Image files (*.png;*.gif;*.jpg)|*.png;*.gif;*.jpg";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                chosenImage = new Bitmap(openFileDialog1.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = chosenImage;
            }
        }
    }
}

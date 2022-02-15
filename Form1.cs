using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShouldBeSolitaire
{
	public partial class Form1 : Form
	{
        Image chosenImage;
		PictureBox[] pbArr;
		int[] winArr;
		int draggedFrom;
		bool amogus;
		readonly TableLayoutPanel tlp = new TableLayoutPanel();
		readonly TableLayoutPanel tlp2 = new TableLayoutPanel();
		int w, h;
		public Form1(int x, int y, Image img)
		{
			InitializeComponent();
			this.AllowDrop = true;
			this.DragDrop += Puzzle_DragDropP;
			pbArr = new PictureBox[x * y];
			winArr = new int[x * y];
			//just make this into a puzzle
			//chosenImage = ResizeImage(img, 900, 900);
			chosenImage = img;
			Image[] imageArr = SliceImage(chosenImage, x, y);
			tlp = InitializeTLP(tlp, x, y);
			int l = 0;
			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y; j++)
				{
					PictureBox pb = new PictureBox
					{
						//Image = imageArr[l],
						Name = i+";"+j,
						BorderStyle = BorderStyle.FixedSingle,
						Dock = DockStyle.Fill,
						Tag = l,
						SizeMode = PictureBoxSizeMode.StretchImage
					};
					pb.DragDrop += Puzzle_DragDropP;
					pb.DragEnter += DragEnterP;
					pb.MouseDown += MouseDownP;
					pb.AllowDrop = true;
					pbArr[l] = pb;
					winArr[l] = l;
					l++;
					tlp.Controls.Add(pb, i, j);
				}
			}
			w = (100 / x);
			tlp.Dock = DockStyle.Fill;
			h = (100 / y);
			tlp.Size = new Size(tlp.ColumnCount * w * 2, tlp.RowCount * h * 2);
			groupBox1.Controls.Add(tlp);
			tlp2 = InitializeTLP(tlp2, x, y);
			Random random = new Random();
			imageArr = imageArr.OrderBy(h => random.Next()).ToArray();
            for (int i = 0; i < imageArr.Length; i++)
            {
				PictureBox pb = new PictureBox
				{
					Image = imageArr[i],
					Name = imageArr[i].Tag.ToString(),
					BorderStyle = BorderStyle.FixedSingle,
					Dock = DockStyle.Fill,
					SizeMode = PictureBoxSizeMode.StretchImage
				};
				pb.MouseDown += MouseDownP;
				pb.DragEnter += DragEnterP;
				tlp2.Controls.Add(pb);
            }
			groupBox2.Controls.Add(tlp2);
			PictureBox picture = new PictureBox
			{
				Image = chosenImage,
				Dock = DockStyle.Fill,
				SizeMode = PictureBoxSizeMode.StretchImage
			};
			groupBox3.Controls.Add(picture);
			//this.Controls.Add(tlp);
		}
        private TableLayoutPanel InitializeTLP(TableLayoutPanel tlp, int x, int y)
        {
			tlp.ColumnCount = x;
			tlp.RowCount = y;
			tlp.ColumnStyles.Clear();
			tlp.RowStyles.Clear();
			for (int i = 0; i < x; i++)
			{
				tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent));
				tlp.ColumnStyles[i].Width = 100 / x;
			}
			for (int i = 0; i < y; i++)
			{
				tlp.RowStyles.Add(new RowStyle(SizeType.Percent));
				tlp.RowStyles[i].Height = 100 / y;
			}
			w = (100 / x);
			tlp.Dock = DockStyle.Fill;
			h = (100 / y);
			tlp.Size = new Size(tlp.ColumnCount * w * 2, tlp.RowCount * h * 2);
			return tlp;
        }
		private Image[] SliceImage(Image img, int x, int y)
        {
			int l = 0;
			Graphics g = Graphics.FromImage(img);
			var imgarray = new Image[x*y];
			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y; j++)
				{
					/*imgarray[l] = new Bitmap(_x/x, _y/y);
					var graphics = Graphics.FromImage(imgarray[l]);
					graphics.DrawImage(img, new Rectangle(0, 0, 900/x, 900/y), new Rectangle(900 / x, 900 / y, 900 / x, 900 / y), GraphicsUnit.Pixel);
					graphics.Dispose();*/
					Rectangle r = new Rectangle(i * (img.Width / x),
										j * (img.Height / y),
										img.Width / x,
										img.Height / y);
					//g.DrawRectangle(pen, r);
					imgarray[l] = cropImage(img, r, i+";"+j);
					l++;
				}
			}
			g.Dispose();
			return imgarray;
		}
		private static Image cropImage(Image img, Rectangle cropArea, string id)
		{
			Bitmap bmpImage = new Bitmap(img);
			Bitmap bmpCrop = bmpImage.Clone(cropArea, PixelFormat.DontCare);
			bmpCrop.Tag = id;
			return bmpCrop;
		}
		void DragEnterP(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Bitmap))
				e.Effect = DragDropEffects.Move;
		}
		void Puzzle_DragDropP(object sender, DragEventArgs e)
		{
			PictureBox pb = sender as PictureBox;
			var bmp = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
			if (pb.Image != null)
			{
				/*string[] coords = draggedFrom.Split(';');
				int x = Int32.Parse(coords[0]);
				int y = Int32.Parse(coords[1]);*/
				//pbArr[(x * y)-1].Image = pb.Image;
				pbArr[draggedFrom].Image = pb.Image;
				pb.Image = bmp;
			}
			else
			{
				pb.Image = bmp;
				if(amogus) pbArr[draggedFrom].Image = null;
			}
			if (bmp.Tag.ToString() == pb.Name) MakeUnavailable((int)pb.Tag, pb);
		}
		//TODO: make game finish
		private void MouseDownP(object sender, MouseEventArgs e)
		{
			PictureBox pb = sender as PictureBox;
			amogus = int.TryParse(Convert.ToString(pb.Tag), out _);//if from puzzle to puzzle
			if (pb.Tag != null && amogus) draggedFrom = (int)pb.Tag;
			var img = pb.Image;
			if (img == null) return;
			if (amogus && CheckIfRight((int)pb.Tag))
			{
                Console.WriteLine((int)pb.Tag);
				return;
			}
			DoDragDrop(img, DragDropEffects.Move);
			if (!amogus)
			{
				pb.Image = null;
			}
		}
		private void MakeUnavailable(int id, PictureBox pb)
		{
			winArr[id] = 2281337;
			pb.AllowDrop = false;
			pb.MouseDown -= MouseDownP;
			pb.DragEnter -= DragEnterP;
			pb.DragDrop -= Puzzle_DragDropP;
			pb.Image = DarkenImage((Bitmap)pb.Image);
			Win();
		}
		private Bitmap DarkenImage(Bitmap bmp)
        {
			Rectangle r = new Rectangle(0, 0, bmp.Width, bmp.Height);
			using (Graphics g = Graphics.FromImage(bmp))
				using (Brush cloud_brush = new SolidBrush(Color.FromArgb(128, Color.Black)))
					g.FillRectangle(cloud_brush, r);
			return bmp;
		}
		private bool CheckIfRight(int tag)
        {
			if (tag == 2281337) return false;
			return !winArr.Contains(tag);
        }
		private void Win()
        {
			bool won = true;
			for (int i = 0; i < winArr.Length; i++)
			{
				if (winArr[i] != 2281337)
				{
					won = false;
					break;
				}
			}
			if (won)
			{
				DialogResult res = MessageBox.Show("You've won!\n Wanna choose another image?", "Winner", MessageBoxButtons.OKCancel);
				if(res == DialogResult.OK)
                {
					StartForm strt = new StartForm();
					strt.Show();
					this.Dispose();
					this.Close();
                }
                else
					Application.Exit();
			}
		}
	}
}

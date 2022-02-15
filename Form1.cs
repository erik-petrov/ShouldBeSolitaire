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
		readonly TableLayoutPanel tlp = new TableLayoutPanel();
		readonly TableLayoutPanel tlp2 = new TableLayoutPanel();
		int w, h;
		public Form1(int x, int y, Image img)
		{
			InitializeComponent();
			//just make this into a puzzle
			//chosenImage = ResizeImage(img, 900, 900);
			chosenImage = img;
			Image[] imageArr = SliceImage(chosenImage, x, y);
			tlp = InitializeTLP(tlp, x, y);
			groupBox1.AllowDrop = true;
			tlp.AllowDrop = true;
			int l = 0;
			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y; j++)
				{
					PictureBox pb = new PictureBox
					{
						//Image = imageArr[l],
						Name = x+";"+y,
						BorderStyle = BorderStyle.FixedSingle,
						Dock = DockStyle.Fill,
						SizeMode = PictureBoxSizeMode.StretchImage
					};
					pb.AllowDrop = true;
					pb.DragDrop += DragDropP;
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
				pb.AllowDrop = true;
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
        /*private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }*/
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
			Brush redBrush = new SolidBrush(Color.Red);
			Pen pen = new Pen(redBrush, 3);
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
			Bitmap bmpCrop = bmpImage.Clone(cropArea, System.Drawing.Imaging.PixelFormat.DontCare);
			bmpCrop.Tag = id;
			return (Image)(bmpCrop);
		}
		void DragEnterP(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Bitmap))
				e.Effect = DragDropEffects.Move;
		}
		void DragDropP(object sender, DragEventArgs e)
		{
			PictureBox pb = sender as PictureBox;
			var bmp = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
			pb.Image = bmp;
		}
		private void MouseDownP(object sender, MouseEventArgs e)
		{
			PictureBox pb = sender as PictureBox;
			var img = pb.Image;
			if (img == null) return;
			if (DoDragDrop(img, DragDropEffects.Move) == DragDropEffects.Move)
			{
				pb.Image = null;
			}
		}
	}
}

using System.Drawing;
using System.Windows.Forms;

namespace ShouldBeSolitaire
{
	public partial class StartForm : Form
	{
        private GroupBox groupBox1;
        private RadioButton medium, easy, hard, custom;
        private TextBox dim1, dim2;
        public StartForm()
		{
			//InitializeComponent();
			this.Size = new Size(800, 400);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.easy = new System.Windows.Forms.RadioButton();
            this.medium = new System.Windows.Forms.RadioButton();
            this.hard = new System.Windows.Forms.RadioButton();
            this.custom = new System.Windows.Forms.RadioButton();

            this.groupBox1.Controls.Add(this.medium);
            this.groupBox1.Controls.Add(this.easy);
            this.groupBox1.Controls.Add(this.hard);
            this.groupBox1.Controls.Add(this.custom);
            this.groupBox1.Size = new System.Drawing.Size(220, 130);
            this.groupBox1.Location = new System.Drawing.Point(320, 100);
            this.groupBox1.Text = "Difficulties";

            this.easy.Location = new System.Drawing.Point(31, 20);
            this.easy.Size = new System.Drawing.Size(67, 17);
            this.easy.Text = "Easy 5x5";

            this.medium.Location = new System.Drawing.Point(31, 43);
            this.medium.Size = new System.Drawing.Size(67, 17);
            this.medium.Text = "Medium 10x10";

            this.hard.Location = new System.Drawing.Point(31, 66);
            this.hard.Size = new System.Drawing.Size(67, 17);
            this.hard.Text = "Hard 20x20";

            this.custom.Location = new System.Drawing.Point(31, 89);
            this.custom.Size = new System.Drawing.Size(67, 17);
            this.custom.Text = "Custom";

            this.Controls.Add(this.groupBox1);

            Label lbl = new Label
			{
				Text = "Welcome to my 'Puzzle' game!\nChoose a difficulty and select your image!",
				AutoSize = true,
			};
            Label lbl1 = new Label
            {
                Text = "x",
                Location = Location = new Point(405, 250),
                Width = 10,
            };
            Label lbl2 = new Label
            {
                Text = "Custom dimensions(no more than 40x40): ",
                Location = Location = new Point(320, 230),
                AutoSize = true,
            };
            dim1 = new TextBox
            {
                Name = "Dim1",
                Width = 20,
                Height = 10,
                Location = new Point(380, 250)
            };
            dim2 = new TextBox
            {
                Name = "Dim2",
                Width = 20,
                Height = 10,
                Location = new Point(420, 250)
            };
            Button btn = new Button
            {
                Text = "Play",
                Location = new Point(375, 280),
            };
            lbl.Location = new Point((this.Width / 2) - lbl.Width);
			this.Controls.Add(lbl);
			this.Controls.Add(lbl1);
			this.Controls.Add(lbl2);
			this.Controls.Add(dim1);
			this.Controls.Add(dim2);
			this.Controls.Add(btn);
		}
	}
}

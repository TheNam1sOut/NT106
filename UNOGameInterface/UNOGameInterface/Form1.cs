using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UNOGameInterface.Properties;
using System.Drawing.Imaging;


namespace UNOGameInterface
{
    public partial class Form1: Form
    {
        private Panel emojiPanel;
        private Timer blinkTimer;
        private bool isBlinking = false;
        private Bitmap originalImageCard;
        private Bitmap originalImageIcon;
        private TableLayoutPanel emojiTable;
        private int columns = 3;
        private int rows = 3;
        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
            InitializeEmojiPanel();
            originalImageCard = new Bitmap("F:\\winform\\UNOGameInterface\\UNOGameInterface\\Resources\\pngtree-uno-card-png-image_9101654.png");
        

            blinkTimer = new Timer();
            blinkTimer.Interval = 100; // Thời gian nháy
            blinkTimer.Tick += BlinkTimer_Tick;
            picDrawPile.Cursor = Cursors.Hand;
            picDrawPile.MouseEnter += picDrawPile_MouseEnter;
            picDrawPile.MouseLeave += picDrawPile_MouseLeave;
            picDrawPile.MouseDown += PictureBox_MouseDown;
            picDrawPile.MouseUp += PictureBox_MouseUp;
            Emoji.Cursor = Cursors.Hand;
            Emoji.Click += pictureBox1_Click;
            Emoji.MouseEnter += pictureBox1_MouseEnter;
            Emoji.MouseLeave += pictureBox1_MouseLeave;
            Setting.MouseDown += Setting_MouseDown;
            Setting.MouseUp += Setting_MouseUp;
        }
        private void Setting_MouseDown(object sender, MouseEventArgs e)
        {
            Bitmap settingImage = new Bitmap(Setting.Image);
            Setting.Image = AdjustBrightness(settingImage, 0.8f); 
        }

        private void Setting_MouseUp(object sender, MouseEventArgs e)
        {
            Setting.Image = Properties.Resources.settings_glyph_black_icon_png_292947_removebg_preview;
        }
        private void InitializeCustomComponents()
        {
            Emoji.Parent = this;
            picDrawPile.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            picDrawPile.BackColor = Color.Transparent;
            picDrawPile.SizeMode = PictureBoxSizeMode.StretchImage;
            Emoji.Image = Properties.Resources._19822c18e912ad0ffb2ad2faed8a61af__1__removebg_preview1;
            Emoji.BackColor = Color.Transparent;
            Emoji.SizeMode = PictureBoxSizeMode.StretchImage;
            Emoji.Parent = this;
            Emoji.Cursor = Cursors.Hand;
            Emoji.Click += pictureBox1_Click;
            this.DoubleBuffered = true;
            Player1.Image = Properties.Resources.avatar_removebg_preview;
            Player1.BackColor = Color.Transparent;
            Player1.SizeMode = PictureBoxSizeMode.StretchImage;
            Player2.Image = Properties.Resources.avatar_removebg_preview;
            Player2.BackColor = Color.Transparent;
            Player2.SizeMode = PictureBoxSizeMode.StretchImage;
            Setting.Image = Properties.Resources.settings_glyph_black_icon_png_292947_removebg_preview;
            Setting.BackColor = Color.Transparent;
            Setting.SizeMode = PictureBoxSizeMode.StretchImage;
            Setting.Cursor = Cursors.Hand;
        }

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            if (isBlinking)
            {
                picDrawPile.Image = originalImageCard;
                isBlinking = false;
                blinkTimer.Stop();
            }
            else
            {
                picDrawPile.Image = AdjustBrightness(originalImageCard, 1.5f); 
                isBlinking = true;
            }
        }
        private Bitmap AdjustBrightness(Bitmap image, float brightness)
        {
            Bitmap adjustedImage = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(adjustedImage))
            {
                float[][] matrix = {
                new float[] {brightness, 0, 0, 0, 0},
                new float[] {0, brightness, 0, 0, 0},
                new float[] {0, 0, brightness, 0, 0},
                new float[] {0, 0, 0, 1f, 0},
                new float[] {0, 0, 0, 0, 1f}
            };

                ColorMatrix colorMatrix = new ColorMatrix(matrix);
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);
                    g.DrawImage(
                        image,
                        new Rectangle(0, 0, image.Width, image.Height),
                        0, 0, image.Width, image.Height,
                        GraphicsUnit.Pixel,
                        attributes
                    );
                }
            }
            return adjustedImage;
        }
        private void ApplyClickEffect()
        {
            picDrawPile.Image = AdjustBrightness(originalImageCard, 0.8f); // Giảm độ sáng
            blinkTimer.Start();
        }
        private void ApplyHoverEffect()
        {
            picDrawPile.Image = AdjustBrightness(originalImageCard, 1.2f);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void InitializeEmojiPanel()
        {
            emojiPanel = new Panel();
            emojiPanel.Size = new Size(100, 100);
            emojiPanel.Location = new Point(Emoji.Left, Emoji.Top - emojiPanel.Height - 10);
            emojiPanel.BackColor = Color.Transparent;
            emojiPanel.BorderStyle = BorderStyle.FixedSingle;
            emojiPanel.Paint += EmojiPanel_Paint;
            emojiTable = new TableLayoutPanel();
            emojiTable.ColumnCount = columns;
            emojiTable.RowCount = rows;
            emojiTable.Dock = DockStyle.Fill;
            emojiTable.BackColor = Color.Transparent;
            for (int i = 0; i < columns; i++)
                emojiTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
            for (int i = 0; i < rows; i++)
                emojiTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));
            string[] emojis = new string[] { "😀", "😂", "😍", "😎", "😢", "😡", "👍", "👎", "🙏" };
            foreach (var emoji in emojis)
            {
                Label lblEmoji = new Label();
                lblEmoji.Text = emoji;
                lblEmoji.Dock = DockStyle.Fill;
                lblEmoji.TextAlign = ContentAlignment.MiddleCenter;
                lblEmoji.Font = new Font("Segoe UI Emoji", 24);
                lblEmoji.ForeColor = Color.Yellow;
                lblEmoji.Cursor = Cursors.Hand;
                lblEmoji.Click += Emoji_Click;

                lblEmoji.MouseEnter += (s, e) =>
                {
                    Label lbl = s as Label;
                    if (lbl != null)
                        lbl.ForeColor = Color.Red;
                };
                lblEmoji.MouseLeave += (s, e) =>
                {
                    Label lbl = s as Label;
                    if (lbl != null)
                        lbl.ForeColor = Color.Yellow; // Màu mặc định
                };

                // Thêm emoji vào bảng chứ không trực tiếp vào panel
                emojiTable.Controls.Add(lblEmoji);
            }

            emojiTable.Resize += EmojiTable_Resize;
            emojiPanel.Controls.Add(emojiTable);

            emojiPanel.Visible = false;  // Ẩn panel khi khởi tạo
            this.Controls.Add(emojiPanel);
            emojiPanel.BringToFront();
        }

        private void EmojiTable_Resize(object sender, EventArgs e)
        {
            // Tính kích thước của mỗi ô trong bảng
            float cellWidth = (float)emojiTable.ClientSize.Width / columns;
            float cellHeight = (float)emojiTable.ClientSize.Height / rows;
            // Chọn kích thước font phù hợp: lấy giá trị nhỏ hơn của cellWidth và cellHeight, nhân với một hệ số điều chỉnh (ví dụ 0.5f)
            float newFontSize = Math.Min(cellWidth, cellHeight) * 0.5f;

            // Cập nhật font của tất cả các Label trong bảng
            foreach (Control ctrl in emojiTable.Controls)
            {
                if (ctrl is Label lbl)
                {
                    lbl.Font = new Font("Segoe UI Emoji", newFontSize);
                }
            }
        }
        private void EmojiPanel_Paint(object sender, PaintEventArgs e)
        {
            // Vẽ khung viền hình vuông (màu đen, độ dày 1)
            ControlPaint.DrawBorder(e.Graphics, emojiPanel.ClientRectangle, Color.Black, 1, ButtonBorderStyle.Solid,
                                                                      Color.Black, 1, ButtonBorderStyle.Solid,
                                                                      Color.Black, 1, ButtonBorderStyle.Solid,
                                                                      Color.Black, 1, ButtonBorderStyle.Solid);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            emojiPanel.Visible = !emojiPanel.Visible;
        }
        private void PictureBox_MouseEnter(object sender, EventArgs e) => ApplyHoverEffect();
        private void PictureBox_MouseLeave(object sender, EventArgs e) => picDrawPile.Image = originalImageCard;
        private void PictureBox_MouseDown(object sender, MouseEventArgs e) => ApplyClickEffect();
        private void PictureBox_MouseUp(object sender, MouseEventArgs e) => picDrawPile.Image = originalImageCard;
        private void picDrawPile_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Hoặc FixedDialog
            this.MaximizeBox = false;
        }

        private void picDrawPile_MouseEnter(object sender, EventArgs e)
        {

        }

        private void picDrawPile_MouseLeave(object sender, EventArgs e)
        {

        }


        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {

        }
        private void Emoji_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null)
            {
                MessageBox.Show("Bạn đã chọn emoji: " + lbl.Text);
            }
        }
    }
}

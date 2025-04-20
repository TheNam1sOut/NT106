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
using System.Drawing.Imaging;
using static System.Runtime.InteropServices.JavaScript.JSType;

using Timer = System.Windows.Forms.Timer;

namespace UNO
{
    public partial class Arena : Form
    {
        private Panel emojiPanel;
        private Timer blinkTimer;
        private bool isBlinking = false;
        private Bitmap originalImageCard;
        private Bitmap originalImageIcon;
        private TableLayoutPanel emojiTable;
        private int columns = 3;
        private int rows = 3;
        private List<string> deck;      // bộ bài còn lại để rút
        private Random rand = new Random();

        /* Một map lưu tất cả hình ảnh lá bài và key để truy xuất các phần tử đó
         * Quy ước về tên lá bài:
         * R, G, B, Y: Màu lá bài, lần lượt là đỏ, xanh lá cây, xanh biển, vàng
         * 0 - 9: Cho các lá số (VD: R9, G8, B7, Y6)
         * C: Lá skip (VD: RC, GC, BC, YC)
         * D: Lá đảo lượt (VD: RD, GD, BD, YD)
         * P: Lá +2 (VD: RP, GP, BP, YP)
         * DD: Lá chọn màu
         * DP: Lá +4
         */
        private Dictionary<string, Image> imageCards = new Dictionary<string, Image>();

        //Lưu tên toàn bộ bài người chơi đang giữ
        private List<string> playerHand = new List<string>();

        //Lưu vị trí đầu tiên mà mình sẽ thực hiện in một lần 6 lá bài, khởi tạo là 0
        private int firstIndex = 0;

        //Lưu toàn bộ lá bài vào trong Dictionary/map với key là tên file (đã theo quy ước)
        private void LoadCards()
        {
            // đường dẫn chứa file thực thi là G:\PC\Coding\VS_repo\UNOv2\UNO\bin\Debug\net8.0-windows
            // đường dẫn cần xử lí là G:\PC\Coding\VS_repo\UNOv2\UNO\Resources\Cards
            // => cần xử lí cardsDirectory như trên để quay lại thư mục UNO
            string cardsDirectory = "..\\..\\..\\Resources\\Cards";
            string[] cardsName = System.IO.Directory.GetFiles(cardsDirectory);

            foreach (string cardName in cardsName)
            {
                string fileCardName = System.IO.Path.GetFileNameWithoutExtension(cardName);
                imageCards[fileCardName] = Image.FromFile(cardName);
            }
            deck = imageCards.Keys.ToList();
            ShuffleDeck();
        }

        private void ShuffleDeck()
        {
            for (int i = deck.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                var tmp = deck[j];
                deck[j] = deck[i];
                deck[i] = tmp;
            }
        }

        //Update lá bài nằm ở giữa
        private void DisplayCard(string cardName)
        {

        }

        //Update danh sách lá bài, với việc thay đổi từng lá bài nằm ở hàm này
        //Tham số thứ hai chỉ ra picturebox nào sẽ bị thay đổi
        private void DisplayCard(string cardName, PictureBox pictureBox)
        {
            try
            {
                pictureBox.Image = imageCards[cardName];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message);
            }
        }

        //Như tên hàm, xuất ra sáu lá bài đầu tiên, chủ yếu dùng khi mà người dùng mới vào trận
        private void DisplayFirstSixCards()
        {
            //test trước về load card, sẽ xóa sau
            playerHand.Add("R0");
            playerHand.Add("G0");
            playerHand.Add("B0");
            playerHand.Add("DP");
            playerHand.Add("DD");
            playerHand.Add("RD");

            playerHand.Add("R1");
            playerHand.Add("G1");
            playerHand.Add("B1");
            playerHand.Add("DD");
            playerHand.Add("DP");
            playerHand.Add("RP");

            playerHand.Add("R0");
            playerHand.Add("G0");


            for (int i = 0; i < 6; i++)
            {
                //nếu danh sách bài người chơi có ít hơn 6 lá thì ngắt khi chỉ số vượt quá giới hạn
                if (i >= playerHand.Count) return;

                //Tìm pictureBox sẽ hiển thị lá bài ở vị trí tương ứng
                string pictureBoxName = "Card" + (i + 1).ToString();
                PictureBox pictureBox = this.Controls[pictureBoxName] as PictureBox;

                //Hiển thị lá bài tại pictureBox đã tìm được
                DisplayCard(playerHand[i], pictureBox);
            }
        }

        // Ba hàm dưới đây sử dụng biến firstIndex để hiển thị 6 lá bài từ danh sách lá bài của người dùng

        //Xử lí khi người dùng bấm nút next
        private void NextButton_Click(object sender, EventArgs e)
        {
            if (firstIndex + 6 >= playerHand.Count) return;
            firstIndex += 6;
            UpdateSixCards();
        }

        //Xử lí khi người dùng bấm nút back
        private void PreviousButton_Click(object sender, EventArgs e)
        {
            if (firstIndex - 6 < 0) return;
            firstIndex -= 6;
            UpdateSixCards();
        }

        //Hàm xử lí hiển thị lại các lá bài khi nhấn vào hai nút back/next 
        private void UpdateSixCards()
        {
            //reset ảnh toàn bộ 6 picturebox
            Card1.Image = Image.FromFile("..\\..\\..\\Resources\\pngtree-uno-card-png-image_9101654.png");
            Card2.Image = Image.FromFile("..\\..\\..\\Resources\\pngtree-uno-card-png-image_9101654.png");
            Card3.Image = Image.FromFile("..\\..\\..\\Resources\\pngtree-uno-card-png-image_9101654.png");
            Card4.Image = Image.FromFile("..\\..\\..\\Resources\\pngtree-uno-card-png-image_9101654.png");
            Card5.Image = Image.FromFile("..\\..\\..\\Resources\\pngtree-uno-card-png-image_9101654.png");
            Card6.Image = Image.FromFile("..\\..\\..\\Resources\\pngtree-uno-card-png-image_9101654.png");

            for (int i = firstIndex; i < firstIndex + 6; i++)
            {
                //nếu danh sách bài người chơi có ít hơn 6 lá thì ngắt khi chỉ số vượt quá giới hạn
                if (i >= playerHand.Count) return;

                //Tìm pictureBox sẽ hiển thị lá bài ở vị trí tương ứng
                string pictureBoxName = "Card" + ((i % 6) + 1).ToString();
                PictureBox pictureBox = this.Controls[pictureBoxName] as PictureBox;

                //Hiển thị lá bài tại pictureBox đã tìm được
                DisplayCard(playerHand[i], pictureBox);
            }
        }


        //Hàm kiểm tra tính hợp lệ của lá bài khi nhấn vào, nếu hợp lệ thì khi nhấn lần nữa sẽ chơi lá đó
        private void pictureBox_Click(object sender, EventArgs e) { }

        // Xử lí khi nhấn nút Draw
        private void drawBtn_Click(object sender, EventArgs e) { }

        // Hàm xử lí yêu cầu rút thêm lá khi người dùng nhấn nút Draw
        //nếu xử lí được +2 +4 tự động thì không cần tham số count
        private void DrawCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (deck.Count == 0)
                {
                    MessageBox.Show("Hết bài để rút!", "UNO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }

                // Lấy lá đầu tiên, remove khỏi deck, add vào tay người chơi
                string card = deck[0];
                deck.RemoveAt(0);
                playerHand.Add(card);
            }

            // Cập nhật lại 6 lá đang hiển thị
            UpdateSixCards();
        }



        //Hàm sắp xếp lại danh sách bài người chơi
        private void sortBtn_Click(object sender, EventArgs e) { }
        public Arena()
        {
            InitializeComponent();
            InitializeCustomComponents();
            InitializeEmojiPanel();
            originalImageCard = new Bitmap(Properties.Resources.pngtree_uno_card_png_image_9101654);


            blinkTimer = new Timer();
            blinkTimer.Interval = 100;
            blinkTimer.Tick += BlinkTimer_Tick;
            CardPrevious.Cursor = Cursors.Hand;
            CardPrevious.MouseEnter += drawCard_MouseEnter;
            CardPrevious.MouseLeave += drawCard_MouseLeave;
            CardPrevious.MouseDown += PictureBox_MouseDown;
            CardPrevious.MouseUp += PictureBox_MouseUp;
            imojiButon.Cursor = Cursors.Hand;
            imojiButon.Click += pictureBox1_Click;
            imojiButon.MouseEnter += pictureBox1_MouseEnter;
            imojiButon.MouseLeave += pictureBox1_MouseLeave;
            setting.MouseDown += setting_MouseDown;
            setting.MouseUp += setting_MouseUp;

            //định nghĩa các hàm khi vào trận
            LoadCards();
            DisplayFirstSixCards();
        }
        private void setting_MouseDown(object sender, MouseEventArgs e)
        {
            Bitmap settingImage = new Bitmap(setting.Image);
            setting.Image = AdjustBrightness(settingImage, 0.8f);
        }

        private void setting_MouseUp(object sender, MouseEventArgs e)
        {
            setting.Image = Properties.Resources.light_blue_settings_gear_22453__1_;
        }
        private void InitializeCustomComponents()
        {
            imojiButon.Parent = this;
            CardPrevious.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            CardPrevious.BackColor = Color.Transparent;
            CardPrevious.SizeMode = PictureBoxSizeMode.StretchImage;
            imojiButon.Image = Properties.Resources._19822c18e912ad0ffb2ad2faed8a61af__1__removebg_preview1;
            imojiButon.BackColor = Color.Transparent;
            imojiButon.SizeMode = PictureBoxSizeMode.StretchImage;
            imojiButon.Parent = this;
            imojiButon.Cursor = Cursors.Hand;
            imojiButon.Click += pictureBox1_Click;
            this.DoubleBuffered = true;
            AvatarPlayer.Image = Properties.Resources.avatar_removebg_preview;
            AvatarPlayer.BackColor = Color.Transparent;
            AvatarPlayer.SizeMode = PictureBoxSizeMode.StretchImage;
            Enemy.Image = Properties.Resources.avatar_removebg_preview;
            Enemy.BackColor = Color.Transparent;
            Enemy.SizeMode = PictureBoxSizeMode.StretchImage;
            setting.Image = Properties.Resources.light_blue_settings_gear_22453__1_;
            setting.BackColor = Color.Transparent;
            setting.SizeMode = PictureBoxSizeMode.StretchImage;
            setting.Cursor = Cursors.Hand;
        }

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            if (isBlinking)
            {
                CardPrevious.Image = originalImageCard;
                isBlinking = false;
                blinkTimer.Stop();
            }
            else
            {
                CardPrevious.Image = AdjustBrightness(originalImageCard, 1.5f);
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
            CardPrevious.Image = AdjustBrightness(originalImageCard, 0.8f); // Giảm độ sáng
            blinkTimer.Start();
        }
        private void ApplyHoverEffect()
        {
            CardPrevious.Image = AdjustBrightness(originalImageCard, 1.2f);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void InitializeEmojiPanel()
        {
            emojiPanel = new Panel();
            emojiPanel.Size = new Size(100, 100);
            emojiPanel.Location = new Point(imojiButon.Left, imojiButon.Top - emojiPanel.Height - 10);
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
        private void PictureBox_MouseLeave(object sender, EventArgs e) => CardPrevious.Image = originalImageCard;
        private void PictureBox_MouseDown(object sender, MouseEventArgs e) => ApplyClickEffect();
        private void PictureBox_MouseUp(object sender, MouseEventArgs e) => CardPrevious.Image = originalImageCard;
        private System.Windows.Forms.Timer aTimer;
        private int counter = 10; // Giá trị khởi tạo
        private void drawCard_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Hoặc FixedDialog
            this.MaximizeBox = false;
        }

        private void drawCard_MouseEnter(object sender, EventArgs e)
        {

        }

        private void drawCard_MouseLeave(object sender, EventArgs e)
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

        private void Arena_Load(object sender, EventArgs e)
        {
            // Xử lý khi form được load
        }

        private void AvatarPlayer_Click(object sender, EventArgs e)
        {
            // Xử lý khi click vào AvatarPlayer
        }

        private void Enemy_Click(object sender, EventArgs e)
        {
            // Xử lý khi click vào Enemy
        }

        private void setting_Click(object sender, EventArgs e)
        {
            // Xử lý khi click vào setting
        }

        private void imojiButon_Click(object sender, EventArgs e)
        {
            // Xử lý khi click vào imojiButon
        }

        private void ClockIcon_Click(object sender, EventArgs e)
        {
            // Xử lý khi click vào ClockIcon
        }

        private void clock1_Click(object sender, EventArgs e)
        {
            // Xử lý khi click vào clock1
        }

        private void setting_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
        private void aTimer_Tick(object sender, EventArgs e)
        {
            counter--;

            if (counter == 0)
            {
                aTimer.Stop();
                aTimer.Dispose();
            }

            TimeMe.Text = counter.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Dừng và hủy timer cũ nếu đang chạy
            DrawCards(1);
            if (aTimer != null)
            {
                aTimer.Stop();
                aTimer.Dispose();
            }

            // Reset giá trị đếm
            counter = 10;
            TimeMe.Text = counter.ToString();

            // Tạo timer mới
            aTimer = new System.Windows.Forms.Timer();
            aTimer.Tick += aTimer_Tick;
            aTimer.Interval = 1000;
            aTimer.Start();
        }

        private void Card5_Click(object sender, EventArgs e)
        {

        }

        private void Card6_Click(object sender, EventArgs e)
        {

        }

        private void Card2_Click(object sender, EventArgs e)
        {

        }
        // hàm sort theo màu và số 
        private void SortButton_Click(object sender, EventArgs e)
        {
            playerHand.Sort();
            UpdateSixCards();
        }
    }
}

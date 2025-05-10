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
using System.Net.Sockets;
using System.IO;

namespace UNO
{
    public partial class Arena : Form
    {
        private NetworkStream stream;
        private string CardTop;
        public TcpClient TcpClient;
        private Panel emojiPanel;
        private Timer blinkTimer;
        private bool isBlinking = false;
        private Bitmap originalImageCard;
        private Bitmap originalImageIcon;
        private TableLayoutPanel emojiTable;
        private int columns = 3;
        private int rows = 3;
        private List<string> deck;      // bộ bài còn lại để rút
        private List<string> discardDeck = new List<string>(); // bo bai bo
        private Random rand = new Random();
        // trạng thái lá bài giữa và lượt chơi
        private string currentMiddleCard;
        private char currentColor;       // 'R','G','B','Y' hoặc 'W' (wild)
        private string currentValue;     // "0"-"9", "C","D","P", "DD","DP"
        private bool isPlayerTurn = true; // true: đến lượt client, false: đối thủ (sau này dùng mạng)
        private int myPlayerId;


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
        private void InitializeMiddleCard()
        {
            DisplayMiddleCard(); // gọi ngay sau LoadCards(), trước DisplayFirstSixCards()
        }
        private void DisplayMiddleCard()
        {
            if (deck.Count == 0)
            {
                MessageBox.Show("Không còn bài để đặt giữa!");
                return;
            }

            // Rút lá đầu tiên trong deck (sau khi đã Shuffle)
            currentMiddleCard = deck[0];
            deck.RemoveAt(0);

            // Lấy màu và giá trị
            currentColor = currentMiddleCard[0];         // R/G/B/Y/DP/DD ...
            currentValue = currentMiddleCard.Substring(1);

            // Hiển thị ảnh
            this.Invoke((Action)(() =>
            {
                MiddlePictureBox.Image = imageCards[currentMiddleCard];
                MiddlePictureBox.Tag = currentMiddleCard;    // lưu lại key để truy xuất nếu cần
            }));
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

        private void ShuffleDiscardDeck()
        {
            for (int i = discardDeck.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                var tmp = discardDeck[j];
                discardDeck[j] = discardDeck[i];
                discardDeck[i] = tmp;
            }
        }

        //Update lá bài nằm ở giữa
        //Update danh sách lá bài, với việc thay đổi từng lá bài nằm ở hàm này
        //Tham số thứ hai chỉ ra picturebox nào sẽ bị thay đổi


        //Như tên hàm, xuất ra sáu lá bài đầu tiên, chủ yếu dùng khi mà người dùng mới vào trận
        private void DisplayFirstSixCards()
        {
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
            this.Invoke((Action)(() =>
            {
                Card1.Image = Image.FromFile("..\\..\\..\\Resources\\pngtree-uno-card-png-image_9101654.png");
                Card2.Image = Image.FromFile("..\\..\\..\\Resources\\pngtree-uno-card-png-image_9101654.png");
                Card3.Image = Image.FromFile("..\\..\\..\\Resources\\pngtree-uno-card-png-image_9101654.png");
                Card4.Image = Image.FromFile("..\\..\\..\\Resources\\pngtree-uno-card-png-image_9101654.png");
                Card5.Image = Image.FromFile("..\\..\\..\\Resources\\pngtree-uno-card-png-image_9101654.png");
                Card6.Image = Image.FromFile("..\\..\\..\\Resources\\pngtree-uno-card-png-image_9101654.png");
            }));

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
        private void drawBtn_Click(object sender, EventArgs e) { }

        // Hàm xử lí yêu cầu rút thêm lá khi người dùng nhấn nút Draw
        private async Task DrawCards(int count)
        {

            for (int i = 0; i < count; i++)
            {
                if (deck.Count > 0)
                {
                    string card = deck[0];
                    playerHand.Add(card);
                    deck.RemoveAt(0);

                    //each time the user draws, sends the update to the server
                    string message = "Drawn\n";
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                }
                else
                {
                    //tron lai chong bai bo va bo vao chong bai chinh
                    ShuffleDiscardDeck();
                    foreach (string tmpCard in discardDeck)
                    {
                        int index = deck.Count;
                        deck[index] = tmpCard;
                    }
                    discardDeck.Clear();

                    //thao tac nhu tren
                    string card = deck[0];
                    playerHand.Add(card);
                    deck.RemoveAt(0);

                    //gui chong bai da thay doi den server
                    string message = "New Deck: ";
                    string drawPile = string.Join(',', deck);
                    foreach (string tmpCard in deck)
                    {
                        message += tmpCard + drawPile;
                    }
                    message += '\n';
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                }
            }

            // nhan voi server la da rut het so luong can thiet
            string messageDone = "Done\n";
            byte[] buffer1 = Encoding.UTF8.GetBytes(messageDone);
            await stream.WriteAsync(buffer1, 0, buffer1.Length);
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
            imojiButon.Cursor = Cursors.Hand;
            imojiButon.Click += pictureBox1_Click;
            imojiButon.MouseEnter += pictureBox1_MouseEnter;
            imojiButon.MouseLeave += pictureBox1_MouseLeave;
            setting.MouseDown += setting_MouseDown;
            setting.MouseUp += setting_MouseUp;

            //định nghĩa các hàm khi vào trận
            LoadCards();
            this.Shown += Arena_Shown;
        }

        public Arena(string playerName, TcpClient playerSocket, string roomName)
        {
            InitializeComponent();
            InitializeCustomComponents();
            InitializeEmojiPanel();
            originalImageCard = new Bitmap(Properties.Resources.pngtree_uno_card_png_image_9101654);
            imojiButon.Cursor = Cursors.Hand;
            imojiButon.Click += pictureBox1_Click;
            imojiButon.MouseEnter += pictureBox1_MouseEnter;
            imojiButon.MouseLeave += pictureBox1_MouseLeave;
            setting.MouseDown += setting_MouseDown;
            setting.MouseUp += setting_MouseUp;
            Room.Text += " " + roomName;
            this.TcpClient=playerSocket;
            //định nghĩa các hàm khi vào trận
            LoadCards();
            this.Shown += Arena_Shown;
        }

        private void DisplayCard(string cardName, PictureBox pictureBox)
        {
            this.Invoke((Action)(() =>
            {
                pictureBox.Image = imageCards[cardName];
                pictureBox.Tag = cardName;      // rất quan trọng để lấy lại key khi click
            }));
            
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
            MiddlePictureBox.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            MiddlePictureBox.BackColor = Color.Transparent;
            MiddlePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
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
            foreach (var i in Enumerable.Range(1, 6))
            {
                var pb = this.Controls[$"Card{i}"] as PictureBox;
                pb.Click += PlayerCard_Click;
            }

            setting.Cursor = Cursors.Hand;
        }


        private string pendingCard = null;  // lá đã click lần 1

        private void PlayerCard_Click(object sender, EventArgs e)
        {
            var pb = sender as PictureBox;
            if (pb == null || pb.Tag == null) return;

            string cardName = pb.Tag.ToString();

            // Nếu chưa tới lượt thì cảnh báo
            if (!isPlayerTurn)
            {
                MessageBox.Show("Chưa đến lượt bạn!", "UNO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lần click đầu: kiểm tra hợp lệ
            if (pendingCard == null)
            {
                if (!IsValidMove(cardName))
                {
                    MessageBox.Show("Bạn không thể đánh lá này!", "UNO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                pendingCard = cardName;
                pb.BorderStyle = BorderStyle.Fixed3D;  // highlight
                return;
            }

            // Lần click hai: phải cùng lá với pending, thực sự đánh
            if (cardName != pendingCard)
            {
                // bấm lá khác thì xóa highlight cũ và set lá mới
                ClearPendingHighlight();
                pendingCard = cardName;
                pb.BorderStyle = BorderStyle.Fixed3D;
                return;
            }

            // Xử lý đánh
            pb.BorderStyle = BorderStyle.None;
            ClearPendingHighlight();  // xoá highlight
            PlayCard(cardName, pb);
            pendingCard = null;
        }
        private void PlayCard(string cardName, PictureBox pb)
        {
            string message;

            // 1. Xoá khỏi tay
            playerHand.Remove(cardName);
            // 2. Update UI: xoá pictureBox hoặc đổi sang lá úp
            pb.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            pb.Tag = null;

            // 3. Nếu là đổi màu (DD hoặc DP), hỏi chọn màu mới
            if (cardName == "DD" || cardName == "DP")
            {
                char chosen = PromptForColor(); // 'R','G','B','Y'
                currentColor = chosen;
                currentValue = cardName;
                // Gửi màu đã chọn kèm lá bài
                message = $"PlayCard: {cardName}|{chosen}\n";
            }
            else
            {
                // cập nhật trạng thái lá giữa
                currentMiddleCard = cardName;
                currentColor = cardName[0];
                currentValue = cardName.Substring(1);
                message = $"PlayCard: {cardName}\n";
            }
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
            MiddlePictureBox.Image = imageCards[cardName];
            MiddlePictureBox.Tag = cardName;

            // 4. Chuyển lượt
            isPlayerTurn = false;
            // TODO: gửi trạng thái mạng cho đối thủ hoặc gọi hàm xử lý lượt đối thủ
        }
        private char PromptForColor()
        {
            // Cách đơn giản: MessageBox 4 nút hoặc ColorDialog
            using (Form f = new Form())
            {
                f.Text = "Chọn màu";
                var panel = new FlowLayoutPanel { Dock = DockStyle.Fill };
                foreach (var kv in new Dictionary<char, string>
                 { {'R',"Đỏ"}, {'G',"Xanh lá"}, {'B',"Xanh dương"}, {'Y',"Vàng"} })
                {
                    var btn = new Button { Text = kv.Value, Tag = kv.Key, AutoSize = true };
                    btn.Click += (s, e) => { f.Tag = ((Button)s).Tag; f.Close(); };
                    panel.Controls.Add(btn);
                }
                f.Controls.Add(panel);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                return f.Tag != null ? (char)f.Tag : 'R';
            }
        }
        private bool IsValidMove(string cardName)
        {
            // Wild (DD hoặc DP) luôn hợp lệ
            if (cardName == "DD" || cardName == "DP") return true;

            // Lấy màu & giá trị
            char color = cardName[0];
            string value = cardName.Substring(1);

            // Theo quy tắc UNO: hợp lệ khi màu trùng hoặc số/symbol trùng
            if (color == currentColor) return true;
            if (value == currentValue) return true;

            return false;
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
            MiddlePictureBox.Image = AdjustBrightness(originalImageCard, 0.8f); // Giảm độ sáng
            blinkTimer.Start();
        }
        private void ApplyHoverEffect()
        {
            MiddlePictureBox.Image = AdjustBrightness(originalImageCard, 1.2f);
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
        private void PictureBox_MouseLeave(object sender, EventArgs e) => MiddlePictureBox.Image = originalImageCard;
        private void PictureBox_MouseDown(object sender, MouseEventArgs e) => ApplyClickEffect();
        private void PictureBox_MouseUp(object sender, MouseEventArgs e) => MiddlePictureBox.Image = originalImageCard;
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

        public  async void Arena_Shown(object sender, EventArgs e)
        {
            try
            {
                stream = TcpClient.GetStream();
                await ReceiveMessagesfromsv();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to server: " + ex.Message);
            }

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
        private void ClearPendingHighlight()
        {
            // Lặp qua các PictureBox Card1…Card6
            for (int i = 1; i <= 6; i++)
            {
                var pb = this.Controls[$"Card{i}"] as PictureBox;
                if (pb != null)
                {
                    pb.BorderStyle = BorderStyle.None;
                }
            }
        }

        private void Card1_Click(object sender, EventArgs e)
        {

        }
        private async Task ReceiveMessagesfromsv()
        {
            try
            {
                using var reader = new StreamReader(stream, Encoding.UTF8);
                while (true)
                {
                    if (TcpClient == null || !TcpClient.Connected)
                    {
                        // marshal MessageBox onto UI thread
                        if (this.InvokeRequired)
                        {
                            this.Invoke((Action)(() =>
                                MessageBox.Show("Not connected to server. Please reconnect.")
                            ));
                        }
                        else
                        {
                            MessageBox.Show("Not connected to server. Please reconnect.");
                        }
                        return;
                    }

                    string msg = await reader.ReadLineAsync(); // Chuyển đổi dữ liệu nhận được thành chuỗi
                    if (msg.StartsWith("isPlay: "))
                    {

                        var IsPlayControls = new[] { isPlay1, isPlay2 };
                        // lưu mảng toàn bộ các controls trong form arena
                        var labelControls = new[] { TimeMe, TimeEnemy };

                        var buttonControls = new[] { DrawButton, PreviousButton, NextButton, SortButton };
                        var pictureBoxControls = new[]
                        {
                            MiddlePictureBox,
                            AvatarPlayer,
                            Enemy,
                            setting,
                            imojiButon,
                            ClockIcon,
                            clock1,
                            Card1,
                            Card2,
                            Card3,
                            Card4,
                            Card5,
                            Card6,
                        };
                        //foreach (var control in IsPlayControls)
                        //{
                        //    this.Invoke((Action)(() =>
                        //    {
                        //        control.BackColor = Color.White;
                        //    }));
                        //}
                        string[] IdCards = msg.Split(':')[1].Trim().Split(','); // Lấy danh sách ID từ thông báo

                        //for (int i = 0; i < IdCards.Length; i++)
                        //{
                        //    //MessageBox.Show(IdCards[i]);
                        //    if (IdCards[i] == "1")
                        //    {
                        //        this.Invoke((Action)(() =>
                        //        {
                        //            IsPlayControls[i].BackColor = Color.Yellow;
                        //        }));         
                        //    }
                        //}
                        if (IdCards.All(id => id.Trim() == "1"))
                        {
                            
                            // Fix for CS0119: 'Action' is a type, which is not valid in the given context
                            // The issue is caused by an incorrect cast syntax. The correct syntax is to cast to `Action` without parentheses.

                            this.Invoke((Action)(() =>
                            {
                                foreach (var control in IsPlayControls)
                                {
                                    control.Visible = false;
                                }
                                // Your code here
                                // Example: Update UI elements
                                foreach (var control in pictureBoxControls)
                                {
                                    control.Visible = true;
                                }
                                foreach (var control in buttonControls)
                                {
                                    control.Visible = true;
                                }
                                foreach (var control in labelControls)
                                {
                                    control.Visible = true;
                                }
                            }));
                        } 
                        else
                        {
                            this.Invoke((Action)(() =>
                            {
                                for (int i = 0; i < IsPlayControls.Length; i++)
                                {
                                    IsPlayControls[i].BackColor = IdCards[i].Trim() == "1"
                                                                  ? Color.Yellow
                                                                  : Color.White;
                                }
                            }));
                        }
                    }
                    else if (msg.StartsWith("CardTop: "))
                    {
                        string[] parts = msg.Substring("CardTop: ".Length).Split('|');
                        string cardName = parts[0].Trim();
                        char color = (cardName == "DD" || cardName == "DP") ? parts[1][0] : cardName[0];
                        string value = (cardName == "DD" || cardName == "DP") ? cardName : cardName.Substring(1);

                        this.Invoke((Action)(() =>
                        {
                            MiddlePictureBox.Image = imageCards[cardName];
                            MiddlePictureBox.Tag = cardName;
                            currentMiddleCard = cardName;
                            currentColor = color;
                            currentValue = value;
                        }));
                    }
                    //handling draw card request
                    else if (msg.StartsWith("InitialHand: "))
                    {
                        //get the draw amount
                        string[] cards = msg.Substring("InitialHand: ".Length).Split(',');
                        foreach(string card in cards)
                        {
                            playerHand.Add(card);
                            
                        }
                        DisplayFirstSixCards();
                    }
                    else if (msg.StartsWith("Turn: "))
                    {
                        int currentTurn = int.Parse(msg.Substring("Turn: ".Length).Trim());
                        // Giả sử client biết ID của mình trong phòng (lưu khi tham gia phòng)
                        this.Invoke((Action)(() =>
                        {
                            // So sánh ID lượt với ID của client
                            isPlayerTurn = (myPlayerId == currentTurn);

                            // Ví dụ: Đổi màu UI để hiển th lượt
                            if (isPlayerTurn)
                            {
                                this.BackColor = Color.LightGreen; // Highlight khi đến lượt
                            }
                            else
                            {
                                this.BackColor = SystemColors.Control; // Reset màu
                            }
                        }));
                    }
                    else if (msg.StartsWith("Room: "))
                    {
                        string roomId = msg.Substring("Room: ".Length).Trim();
                        // Lưu ID phòng (nếu cần)
                    }
                    else if (msg.StartsWith("YourId: "))
                    {
                        int myId = int.Parse(msg.Substring("YourId: ".Length).Trim());
                        myPlayerId = myId;
                        Console.WriteLine($"My Player ID: {myPlayerId}");
                        // Lưu myId vào biến để sử dụng khi xử lý lượt
                    }
                    //update draw pile and discard pile
                    //else if (msg.StartsWith("Dataqueue: "))
                    //{
                    //    string[] parts = msg.Substring("Dataqueue: ".Length).Split('|');
                    //    if (parts.Length == 2)
                    //    {
                    //        string[] dataQueue = parts[0].Split(',');
                    //        string[] dataQueue1 = parts[1].Split(',');

                    //        deck.Clear();
                    //        foreach (string card in dataQueue)
                    //        {
                    //            deck.Add(card.Trim());
                    //        }
                    //        discardDeck.Clear();
                    //        foreach (string card in dataQueue1)
                    //        {
                    //            discardDeck.Add(card.Trim());
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                // marshal error dialog
                this.Invoke((Action)(() =>
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ));
            }
        }
        private async void ReadyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ReadyBtn.Visible = false;
                byte[] buffer = Encoding.UTF8.GetBytes($"Ready: {Room.Text.Trim()}");
                await stream.WriteAsync(buffer, 0, buffer.Length); // Gửi thông báo "Ready" đến server
              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Room_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Timer = System.Windows.Forms.Timer;

namespace UNO
{
    public partial class Arena : Form
    {
        private bool isExiting = false;
        private string CardTop;
        public string playerName;
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
        private bool isPlayerTurn; // true: đến lượt client, false: đối thủ (sau này dùng mạng)
        private int myPlayerId;
        private int pendingDraw = 0;
        private bool unoCalled = false;
        private int targetId = -1;
        private System.Windows.Forms.Timer uiTimer; // Timer để cập nhật UI
        private int remainingTimeSeconds; // Thời gian còn lại tính bằng giây
        private int currentTurnPlayerId; // ID của người chơi hiện tại (từ server)
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
        private readonly object playerHandLock = new object();
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
        private void UiTimer_Tick(object sender, EventArgs e)
        {
            if (remainingTimeSeconds > 0)
            {
                remainingTimeSeconds--;
                lblTimer.Text = $"{remainingTimeSeconds}s";

                if (remainingTimeSeconds <= 3)
                {
                    lblTimer.ForeColor = Color.Red;
                }
                else
                {
                    lblTimer.ForeColor = SystemColors.ControlText;
                }
            }
            else
            {
                uiTimer.Stop();
                lblTimer.Text = "Hết giờ!";
                lblTimer.ForeColor = Color.Red;
            }
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
                for (int i = 1; i <= 6; i++)
                {
                    var pb = this.Controls[$"Card{i}"] as PictureBox;
                    pb.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
                    pb.Tag = null;
                }
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

        //helper function 
        private void RefillDeckFromDiscard()
        {
            // Giữ lại lá trên cùng
            var top = discardDeck.Last();
            discardDeck.RemoveAt(discardDeck.Count - 1);

            ShuffleDiscardDeck();
            deck.AddRange(discardDeck);
            discardDeck.Clear();
            discardDeck.Add(top);
            ShuffleDeck();
        }
        // Hàm xử lí yêu cầu rút thêm lá khi người dùng nhấn nút Draw
        private async Task DrawCards(int count)
        {
            await NetworkManager.Instance.SendAsync("DrawCard");
        }



        //Hàm sắp xếp lại danh sách bài người chơi
        private void sortBtn_Click(object sender, EventArgs e) { }

        public Arena(string playerName, string roomName)
        {
            InitializeComponent();
            InitializeCustomComponents();
            InitializeEmojiPanel();
            originalImageCard = new Bitmap(Properties.Resources.pngtree_uno_card_png_image_9101654);
            imojiButon.Cursor = Cursors.Hand;
            imojiButon.Click += pictureBox1_Click;
            imojiButon.MouseEnter += pictureBox1_MouseEnter;
            imojiButon.MouseLeave += pictureBox1_MouseLeave;
            //setting.MouseDown += setting_MouseDown;
            //setting.MouseUp += setting_MouseUp;
            Room.Text += " " + roomName;
            btnUno.Visible = false;
            btnCatch.Visible = false;
            sendBtn.Visible = false;
            chatBox.Visible = false;
            chatInput.Visible = false;
            NameMe.Visible = false;
            Name1.Visible = false;
            Name2.Visible = false;
            Name3.Visible = false;
            Number1.Visible = false;
            Number2.Visible = false;
            Number3.Visible = false;
            NumberMe.Visible = false;
            Me.Visible = false;
            Player1.Visible = false;
            Player2.Visible = false;
            Player3.Visible = false;    
            CurentColor.Visible = false;



            this.playerName = playerName;
            uiTimer = new System.Windows.Forms.Timer();
            uiTimer.Interval = 1000; // Cập nhật mỗi 1 giây (1000 miligiây)
            uiTimer.Tick += UiTimer_Tick; // Gán sự kiện Tick cho phương thức xử lý
            //định nghĩa các hàm khi vào trận
            LoadCards();

            // Add form closing event handler
            this.FormClosing += Arena_FormClosing;

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
        //private void setting_MouseDown(object sender, MouseEventArgs e)
        //{
        //    Bitmap settingImage = new Bitmap(setting.Image);
        //    setting.Image = AdjustBrightness(settingImage, 0.8f);
        //}

        //private void setting_MouseUp(object sender, MouseEventArgs e)
        //{
        //    setting.Image = Properties.Resources.light_blue_settings_gear_22453__1_;
        //}
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
            //AvatarPlayer.Image = Properties.Resources.avatar_removebg_preview;
            //AvatarPlayer.BackColor = Color.Transparent;
            //AvatarPlayer.SizeMode = PictureBoxSizeMode.StretchImage;
            //Enemy.Image = Properties.Resources.avatar_removebg_preview;
            //Enemy.BackColor = Color.Transparent;
            //Enemy.SizeMode = PictureBoxSizeMode.StretchImage;
            //setting.Image = Properties.Resources.light_blue_settings_gear_22453__1_;
            //setting.BackColor = Color.Transparent;
            //setting.SizeMode = PictureBoxSizeMode.StretchImage;
            foreach (var i in Enumerable.Range(1, 6))
            {
                var pb = this.Controls[$"Card{i}"] as PictureBox;
                pb.Click += PlayerCard_Click;
            }

            //setting.Cursor = Cursors.Hand;
        }


        private string pendingCard = null;  // lá đã click lần 1

        private void PlayerCard_Click(object sender, EventArgs e)
        {
            ClearPendingHighlight();
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
                ClearPendingHighlight();
                pendingCard = cardName;
                pb.BorderStyle = BorderStyle.Fixed3D;  // highlight
                pb.BackColor = Color.Yellow; // Thêm màu nền để dễ nhận biết
                if (playerHand.Count == 2)
                {
                    btnUno.Visible = true;
                }

                return;
            }

            // Lần click hai: phải cùng lá với pending, thực sự đánh
            if (cardName != pendingCard)
            {
                if (!IsValidMove(cardName))
                {
                    MessageBox.Show("Lá này không hợp lệ, vui lòng chọn lá khác hoặc nhấn Draw.", "UNO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // bấm lá khác thì xóa highlight cũ và set lá mới
                ClearPendingHighlight();
                pendingCard = cardName;
                pb.BorderStyle = BorderStyle.Fixed3D;
                pb.BackColor = Color.Yellow;
                return;
            }

            // Xử lý đánh
            pb.BorderStyle = BorderStyle.None;
            ClearPendingHighlight();  // xoá highlight
            pb.BackColor = Color.Transparent;
            PlayCard(cardName, pb);
            pendingCard = null;
        }
        private async void PlayCard(string cardName, PictureBox pb)
        {
            char newColor = currentColor;
            string message;

            // 1. Xoá khỏi tay

            // 2. Update UI: xoá pictureBox hoặc đổi sang lá úp
            pb.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            pb.Tag = null;
            // Sau khi gọi PlayCard và gửi thông điệp xong:

            // 3. Nếu là đổi màu (DD hoặc DP), hỏi chọn màu mới
            if (cardName == "DD" || cardName == "DP")
            {
                newColor = PromptForColor();
                message = $"PlayCard: {cardName}|{newColor}";
            }
            else if (cardName.EndsWith("P"))
            {
                message = $"PlayCard: {cardName}|{currentColor}";
            }
            else
            {
                message = $"PlayCard: {cardName}";
            }
            Console.WriteLine($"[DEBUG] Pre-Remove hand: {string.Join(",", playerHand)}");
            lock (playerHandLock)
            {
                playerHand.Remove(cardName);
                playerHand.Sort();
            }
            Console.WriteLine($"[DEBUG] Post-Remove hand: {string.Join(",", playerHand)}");
            this.Invoke((Action)(() => UpdateSixCards()));
            discardDeck.Add(cardName);

            // Gửi message
            await NetworkManager.Instance.SendAsync(message);
            // Cập nhật trạng thái
            currentMiddleCard = cardName;
            currentColor = newColor;
            currentValue = cardName.EndsWith("P") ? "P" : cardName.Substring(1);

            // Update UI
            MiddlePictureBox.Image = imageCards[cardName];
            MiddlePictureBox.Tag = cardName;
            // TODO: gửi trạng thái mạng cho đối thủ hoặc gọi hàm xử lý lượt đối thủ
            btnUno.Visible = false;
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
                    var btn = new System.Windows.Forms.Button { Text = kv.Value, Tag = kv.Key, AutoSize = true };
                    btn.Click += (s, e) => { f.Tag = ((System.Windows.Forms.Button)s).Tag; f.Close(); };
                    panel.Controls.Add(btn);
                }
                f.Controls.Add(panel);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                return f.Tag != null ? (char)f.Tag : 'R';
            }
        }
        private bool IsValidMove(string card)
        {
            char topColor = currentColor;
            string topValue = currentValue;
            char cardColor = card[0];
            string cardValue = card.Substring(1);

            // Đang phạt
            if (pendingDraw > 0)
            {
                if (topValue == "DP") // +4
                {
                    return card == "DP";
                }
                else if (topValue == "P") // +2
                {
                    return card.EndsWith("P") || card == "DP";
                }
            }

            // Nếu lá trên cùng là Wild (DD)
            if (topValue == "DD")
            {
                if (card == "DD" || card == "DP") return true;
                return cardColor == topColor;
            }

            // Luật bình thường
            if (card == "DD" || card == "DP") return true;
            return (cardColor == topColor || cardValue == topValue);
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
            if (emojiPanel.Visible)
            {
                emojiPanel.BringToFront();
            }
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
                // Add emoji to chat input
                chatInput.Text += lbl.Text;
                chatInput.Focus();
                chatInput.SelectionStart = chatInput.Text.Length;

                // Hide emoji panel after selection
                emojiPanel.Visible = false;
            }
        }

        public void Arena_Shown(object sender, EventArgs e)
        {
            // Subscribe to messages
            NetworkManager.Instance.MessageReceived += HandleServerMessage;
            try
            {
                // No longer need to get stream from TcpClient here
                // NetworkManager.Instance.Connect(playerName, roomName); // Assuming NetworkManager handles connection
                // For now, we'll just assume connection is established or will be handled by NetworkManager
                // The actual connection logic should be in NetworkManager.Instance.Connect
                // For this example, we'll just try to send a ready message to test
                // await NetworkManager.Instance.SendAsync($"Ready: {Room.Text.Trim()}\n"); // This line is removed as per new_code
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

            lblTimer.Text = counter.ToString();
        }
        private TaskCompletionSource<bool> turnTcs;
        private async void button1_Click(object sender, EventArgs e)
        {
            if (!isPlayerTurn)
            {
                MessageBox.Show("Chưa đến lượt bạn!", "UNO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (HasPlayableCard())
            {
                MessageBox.Show("Bạn có lá bài hợp lệ, không thể rút thêm!", "UNO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Nếu đang bị phạt
            if (pendingDraw > 0)
            {
                MessageBox.Show($"Bạn đang bị phạt {pendingDraw} lá!", "UNO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Trường hợp bình thường (không phạt): rút 1 lá mỗi click
            await DrawCards(1);
            UpdateSixCards();
            DrawButton.Enabled = isPlayerTurn;
        }


        // Helper kiểm tra lá bình thường (không tính pendingDraw)
        private bool IsValidNormalMove(string card)
        {
            char topColor = currentColor;
            string topValue = currentValue;
            char cardColor = card[0];
            string cardValue = card.Substring(1);

            if (card == "DD" || card == "DP") return true;
            return (cardColor == topColor || cardValue == topValue);
        }
        // Hàm tìm PictureBox chứa lá bài cụ thể
        private PictureBox FindPictureBoxForCard(string cardName)
        {
            foreach (Control control in this.Controls)
            {
                if (control is PictureBox pb && pb.Tag?.ToString() == cardName)
                    return pb;
            }
            return null;
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
                    pb.BackColor = Color.Transparent; // Reset màu nền
                }
            }
        }

        private void Card1_Click(object sender, EventArgs e)
        {

        }
        private void HandleServerMessage(string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() => HandleServerMessage(msg)));
                return;
            }
            try
            {
                if (msg.StartsWith("isPlay: "))
                {
                    var IsPlayControls = new[] { isPlay1, isPlay2, isPlay3, isPlay4 };
                    var labelControls = new[] { lblTimer, NameMe, Name1, Name2, Name3, NumberMe, Number1, Number2, Number3, CurentColor  };
                    var labels = new[] { Player1, Me, Player2,Player3 };
                    var buttonControls = new[] { DrawButton, PreviousButton, NextButton, SortButton, sendBtn };
                    var pictureBoxControls = new[]
                    {
                        MiddlePictureBox,
                        //pictureBox1,
                        //pictureBox2,
                        //AvatarPlayer,
                        //Enemy,
                        //setting,
                        imojiButon,
                        ClockIcon,
                        //clock1,
                        Card1,
                        Card2,
                        Card3,
                        Card4,
                        Card5,
                        Card6,
                    };
                    string[] IdCards = msg.Split(':')[1].Trim().Split(',');
                    // Show which players are ready (yellow) and which are not (white)
                    for (int i = 0; i < IsPlayControls.Length; i++)
                    {
                        IsPlayControls[i].BackColor = IdCards[i].Trim() == "1"
                            ? Color.Yellow
                            : Color.White;
                    }
                    // If all are ready, show the main game UI
                    if (IdCards.All(id => id.Trim() == "1"))
                    {
                        // this.Invoke((Action)(() =>
                        // {
                            foreach (var control in IsPlayControls)
                            {
                                control.Visible = false;
                            }
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
                            foreach (var control in labels)
                            {
                                control.Visible = true;
                            }
                            chatBox.Visible = true;
                            chatInput.Visible = true;
                        //}));
                    }
                    else
                    {
                        // Not all ready: show waiting state
                        foreach (var control in IsPlayControls)
                            control.Visible = true;
                        foreach (var control in pictureBoxControls)
                            control.Visible = false;
                        foreach (var control in buttonControls)
                            control.Visible = false;
                        foreach (var control in labelControls)
                            control.Visible = false;
                        foreach (var control in labels)
                            control.Visible = false;
                        chatBox.Visible = false;
                        chatInput.Visible = false;
                        // Optionally, show a waiting message
                        
                        backBtn.Visible = false;
                    }
                }
                else if (msg.StartsWith("CardTop: "))
                {
                    var p = msg.Substring(9).Split('|');
                    currentMiddleCard = p[0].Trim();
                    currentColor = p.Length > 1 ? p[1][0] : currentMiddleCard[0];
                    if (p.Length > 1 && p[1][0] != 'W')
                    {
                        currentColor = p[1][0];
                        CurentColor.BackColor = ColorFromChar(currentColor);
                    }
                    currentValue = (currentMiddleCard == "DD" || currentMiddleCard == "DP")
                                          ? currentMiddleCard
                                          : currentMiddleCard.Substring(1);
                    MiddlePictureBox.Image = imageCards[currentMiddleCard];
                }
                else if (msg.StartsWith("InitialHand: "))
                {
                    string[] cards = msg.Substring("InitialHand: ".Length).Split(',');
                    playerHand.Clear();
                    foreach (string card in cards)
                    {
                        playerHand.Add(card.Trim());
                    }
                    //this.Invoke((Action)(() => DisplayFirstSixCards()));
                    DisplayFirstSixCards();
                }
                else if (msg.StartsWith("PendingDraw: "))
                {
                    pendingDraw = int.Parse(msg.Substring("PendingDraw: ".Length).Trim());
                    DrawButton.Enabled = isPlayerTurn;
                }
                else if (msg.StartsWith("Turn: "))
                {
                    int id = int.Parse(msg.Substring(6));
                    UpdatePlayerPanelColor(id);
                    isPlayerTurn = (id == myPlayerId);
                    DrawButton.Enabled = isPlayerTurn && pendingDraw == 0;
                    if (isPlayerTurn)
                        unoCalled = false;
                }
                else if (msg.StartsWith("Room: "))
                {
                    string roomId = msg.Substring("Room: ".Length).Trim();
                }
                else if (msg.StartsWith("YourId: "))
                {
                    int myId = int.Parse(msg.Substring("YourId: ".Length).Trim());
                    myPlayerId = myId;
                    Console.WriteLine($"My Player ID: {myPlayerId}");
                }
                else if (msg.StartsWith("DrawCard: "))
                {
                    string card = msg.Substring("DrawCard: ".Length).Trim();
                    playerHand.Add(card);
                    // this.Invoke((Action)(() =>
                    // {
                        UpdateSixCards();
                        int count = playerHand.Count;
                        var mePanel = this.Controls["Me"] as Panel;
                        var lbl = mePanel?.Controls["NumberMe"] as Label;
                        if (lbl != null)
                            lbl.Text = $"Số lá: {count}";
                        if (IsValidMove(card))
                        {
                            var pb = FindPictureBoxForCard(card);
                            if (pb != null)
                            {
                                pb.BorderStyle = BorderStyle.Fixed3D;
                                pb.BackColor = Color.Yellow;
                            }
                        }
                    //}));
                }
                else if (msg.StartsWith("CatchWindow: "))
                {
                    targetId = int.Parse(msg.Substring("CatchWindow: ".Length).Trim());
                    Console.WriteLine("[DEBUG] Nhận CatchWindow cho targetId: " + targetId + ", myId: " + myPlayerId);
                    bool showCatch = (myPlayerId != targetId);
                    // this.Invoke((Action)(() =>
                    // {
                        btnCatch.Visible = showCatch;
                    //}));
                    Console.WriteLine("[DEBUG] Catch nút hiện lên: " + (showCatch ? "TRUE" : "FALSE"));
                }
                else if (msg.StartsWith("PlayerWin: "))
                {
                    var labelControls = new[] { lblTimer, NameMe, Name1, Name2, Name3, NumberMe, Number1, Number2, Number3, CurentColor };
                    var labels = new[] { Player1, Me, Player2, Player3 };
                    var buttonControls = new[] { DrawButton, PreviousButton, NextButton, SortButton, sendBtn };
                    var pictureBoxControls = new[]
                    {
                        MiddlePictureBox,
                        //AvatarPlayer,
                        //Enemy,
                        //setting,
                        imojiButon,
                        ClockIcon,
                        //clock1,
                        Card1,
                        Card2,
                        Card3,
                        Card4,
                        Card5,
                        Card6,
                    };
                    string winner = msg.Substring("PlayerWin: ".Length).Trim();
                    // this.Invoke((Action)(() =>
                    // {
                        foreach (var control in pictureBoxControls)
                        {
                            control.Visible = false;
                        }
                        foreach (var control in buttonControls)
                        {
                            control.Visible = false;
                        }
                        foreach (var control in labelControls)
                        {
                            control.Visible = false;
                        }
                        foreach (var control in labels)
                        {
                            control.Visible = false;
                        }
                        chatBox.Visible = false;
                        chatInput.Visible = false;
                        resultLabel.Text = $"{winner} đã chiến thắng!";
                        resultLabel.Visible = true;
                        backBtn.Visible = true;
                    //}));
                }
                else if (msg.StartsWith("AutoDrawCount: "))
                {
                    int count = int.Parse(msg.Substring("AutoDrawCount: ".Length).Trim());
                    // this.Invoke((Action)(() =>
                    // {
                        MessageBox.Show($"Bạn sẽ bị rút {count} lá do hiệu ứng +2/+4!", "Thông báo",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}));
                }
                else if (msg.StartsWith("Chat: "))
                {
                    string[] parts = msg.Substring(6).Trim().Split('|', 2);
                    string playerName = parts[0];
                    string msgContent = parts[1];
                    // this.Invoke((Action)(() =>
                    // {
                        chatBox.AppendText($"{playerName}: {msgContent}\n");
                    //}));
                }
                else if (msg.StartsWith("PlayerDisconnected: "))
                {
                    if (isExiting) return; // Prevent multiple triggers
                    isExiting = true;
                    NetworkManager.Instance.MessageReceived -= HandleServerMessage; // Unsubscribe!
                    string disconnectedPlayer = msg.Substring("PlayerDisconnected: ".Length).Trim();
                    // this.Invoke((Action)(() =>
                    // {
                        MessageBox.Show($"{disconnectedPlayer} has disconnected from the game.\nThe match has been ended.",
                                       "Player Disconnected",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Information);
                        this.Hide();
                        Menu Form1 = new Menu(playerName);
                        Form1.Show();
                    //}));
                }
                else if (msg.StartsWith("HideCatchWindow"))
                {
                    // this.Invoke((Action)(() =>
                    // {
                        btnCatch.Visible = false;
                    //}));
                    Console.WriteLine("[DEBUG] Received HideCatchWindow, hiding Catch button on this client");
                }
                else if (msg.StartsWith("PlayerNames: "))
                {
                    var parts = msg.Substring("PlayerNames: ".Length).Split('|');
                    if (parts.Length == 4)
                    {
                        NameMe.Invoke((Action)(() => NameMe.Text = parts[myPlayerId - 1]));
                        for (int i = 1; i <= 3; i++)
                        {
                            int idx = (myPlayerId - 1 + i) % 4;
                            var panel = this.Controls[$"Player{i}"] as Panel;
                            var lbl = panel?.Controls[$"Name{i}"] as Label;
                            lbl?.Invoke((Action)(() => lbl.Text = parts[idx]));
                        }
                    }
                }
                else if (msg.StartsWith("Remaining: "))
                {
                    var token = msg.Substring("Remaining: ".Length).Trim();
                    var parts = token.Split(':');
                    if (parts.Length == 2
                        && int.TryParse(parts[0], out int pid)
                        && int.TryParse(parts[1], out int count))
                    {
                        // this.Invoke((Action)(() =>
                        // {
                            if (pid == myPlayerId)
                            {
                                var mePanel = this.Controls["Me"] as Panel;
                                if (mePanel != null)
                                {
                                    var numberMeLabel = mePanel.Controls["NumberMe"] as Label;
                                    if (numberMeLabel != null)
                                    {
                                        numberMeLabel.Text = $"Số lá: {count}";
                                        numberMeLabel.Visible = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("[DEBUG] Label 'NumberMe' not found inside 'Me' panel.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("[DEBUG] Panel 'Me' not found on the form.");
                                }
                            }
                            else
                            {
                                int offset = (pid - myPlayerId + 4) % 4;
                                var panel = this.Controls[$"Player{offset}"] as Panel;
                                if (panel != null)
                                {
                                    var numberLabel = panel.Controls[$"Number{offset}"] as Label;
                                    if (numberLabel != null)
                                    {
                                        numberLabel.Text = $"Số lá: {count}";
                                        numberLabel.Visible = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"[DEBUG] Label 'Number{offset}' not found inside 'Player{offset}' panel.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"[DEBUG] Panel 'Player{offset}' not found on the form.");
                                }
                            }
                        //}));
                    }
                }
                else if (msg.StartsWith("TurnTimerStart:"))
                {
                    string[] parts = msg.Substring("TurnTimerStart:".Length).Trim().Split('|');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int playerTurnId) && int.TryParse(parts[1], out int timeLimit))
                    {
                        currentTurnPlayerId = playerTurnId;
                        remainingTimeSeconds = timeLimit;
                        uiTimer.Stop();
                        lblTimer.Text = $"{remainingTimeSeconds}s";
                        lblTimer.ForeColor = SystemColors.ControlText;
                        if (playerTurnId == myPlayerId)
                        {
                            uiTimer.Start();
                        }
                    }
                }
                else if (msg.StartsWith("Timeout:"))
                {
                    string timeoutMessage = msg.Substring("Timeout:".Length).Trim();
                    MessageBox.Show(timeoutMessage, "Hết thời gian!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    uiTimer.Stop();
                    lblTimer.Text = "Hết giờ!";
                    lblTimer.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                //this.Invoke((Action)(() =>
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //));
            }
        }
        private void UpdatePlayerPanelColor(int currentTurn)
        {
            // Đặt tất cả panel về màu mặc định
            Me.BackColor = Color.White;
            Player1.BackColor = Color.White;
            Player2.BackColor = Color.White;
            Player3.BackColor = Color.White;

            // Tính toán ánh xạ từ playerId đến panel dựa trên myPlayerId
            // Tạo mảng ánh xạ ID của người chơi theo thứ tự tương đối
            int[] playerOrder = new int[4];
            for (int i = 0; i < 4; i++)
            {
                // Tính ID của người chơi tại vị trí thứ i (bắt đầu từ myPlayerId)
                playerOrder[i] = (myPlayerId + i - 1) % 4 + 1; // Tuần hoàn từ 1 đến 4
            }

            // Xác định panel tương ứng với currentTurn
            if (currentTurn == playerOrder[0]) // NameMe
            {
                Me.BackColor = Color.Yellow;
            }
            else if (currentTurn == playerOrder[1]) // Name1
            {
                Player1.BackColor = Color.Yellow;
            }
            else if (currentTurn == playerOrder[2]) // Name2
            {
                Player2.BackColor = Color.Yellow;
            }
            else if (currentTurn == playerOrder[3]) // Name3
            {
                Player3.BackColor = Color.Yellow;
            }
        }
        private async void ReadyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ReadyBtn.Visible = false;
                await NetworkManager.Instance.SendAsync($"Ready: {Room.Text.Trim()}\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //helper check bài có thể đánh để ban draw 
        private bool HasPlayableCard()
        {
            return playerHand.Any(card => IsValidMove(card));
        }
        private void Room_TextChanged(object sender, EventArgs e)
        {

        }

        private void isPlay2_Click(object sender, EventArgs e)
        {

        }

        private async void btnUno_Click(object sender, EventArgs e)
        {
            await NetworkManager.Instance.SendAsync($"UnoCall: {myPlayerId}");
            unoCalled = true;
            btnUno.Visible = false;
            Console.WriteLine($"[DEBUG] Sent UnoCall for player {myPlayerId}");
        }

        private async void btnCatch_Click(object sender, EventArgs e)
        {
            await NetworkManager.Instance.SendAsync($"CatchUno: {targetId}");
            btnCatch.Visible = false;
            Console.WriteLine($"[DEBUG] Sent CatchUno for target {targetId}");
        }

        private void chatInput_TextChanged(object sender, EventArgs e)
        {

        }

        private async void sendBtn_Click(object sender, EventArgs e)
        {
            string chatMsg = chatInput.Text.Trim();
            if (!string.IsNullOrEmpty(chatMsg))
            {
                await NetworkManager.Instance.SendAsync($"Chat: {chatMsg}");
            }
            chatInput.Clear();
        }
        private void Arena_Load(object sender, EventArgs e)
        {

        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Menu Form1 = new Menu(playerName);
            Form1.Show();
        }
        private Color ColorFromChar(char c)
        {
            switch (c)
            {
                case 'R': return Color.Red;
                case 'G': return Color.Green;
                case 'B': return Color.Blue;
                case 'Y': return Color.Yellow;
                default: return SystemColors.Control;  
            }
        }

        private async void Arena_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Console.WriteLine("[DEBUG] Arena form is closing, handling disconnection...");
                NetworkManager.Instance.MessageReceived -= HandleServerMessage; // Unsubscribe from messages

                // Optionally notify the server
                await NetworkManager.Instance.SendAsync($"Disconnect: {playerName}");

                // Optionally close the connection if you want to fully disconnect the client
                // NetworkManager.Instance.Disconnect();

                Console.WriteLine("[DEBUG] Client disconnection handled successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error during form closing: {ex.Message}");
            }
        }

        private void Name3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void NumberMe_Click(object sender, EventArgs e)
        {

        }

        private void Number1_Click(object sender, EventArgs e)
        {

        }

        private void Name2_Click(object sender, EventArgs e)
        {

        }

        private void Number2_Click(object sender, EventArgs e)
        {

        }

        private void TimeEnemy_Click(object sender, EventArgs e)
        {

        }

        private void NameMe_Click(object sender, EventArgs e)
        {

        }
    }
}
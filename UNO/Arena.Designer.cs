namespace UNO
{
    partial class Arena
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            MiddlePictureBox = new PictureBox();
            setting = new PictureBox();
            imojiButon = new PictureBox();
            ClockIcon = new PictureBox();
            clock1 = new PictureBox();
            DrawButton = new Button();
            Card1 = new PictureBox();
            Card2 = new PictureBox();
            Card3 = new PictureBox();
            Card4 = new PictureBox();
            Card5 = new PictureBox();
            PreviousButton = new Button();
            NextButton = new Button();
            SortButton = new Button();
            Card6 = new PictureBox();
            ReadyBtn = new Button();
            isPlay1 = new PictureBox();
            isPlay2 = new PictureBox();
            Room = new TextBox();
            isPlay3 = new PictureBox();
            isPlay4 = new PictureBox();
            sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            sqlCommandBuilder1 = new Microsoft.Data.SqlClient.SqlCommandBuilder();
            sqlCommand2 = new Microsoft.Data.SqlClient.SqlCommand();
            sqlCommand3 = new Microsoft.Data.SqlClient.SqlCommand();
            sqlCommand4 = new Microsoft.Data.SqlClient.SqlCommand();
            sqlCommandBuilder2 = new Microsoft.Data.SqlClient.SqlCommandBuilder();
            btnUno = new Button();
            btnCatch = new Button();
            chatBox = new RichTextBox();
            chatInput = new TextBox();
            sendBtn = new Button();
            resultLabel = new Label();
            scoreLabel = new Label();
            backBtn = new Button();
            Name1 = new Label();
            Name2 = new Label();
            Name3 = new Label();
            NameMe = new Label();
            Number3 = new Label();
            Number2 = new Label();
            Number1 = new Label();
<<<<<<< HEAD
            Me = new Panel();
            NumberMe = new Label();
            Player1 = new Panel();
            Player2 = new Panel();
            Player3 = new Panel();
=======
            lblTimer = new Label();
            TimeEnemy = new Label();
>>>>>>> timer chua xu phat
            ((System.ComponentModel.ISupportInitialize)MiddlePictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)setting).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imojiButon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ClockIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)clock1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Card1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Card2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Card3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Card4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Card5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Card6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)isPlay1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)isPlay2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)isPlay3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)isPlay4).BeginInit();
            Me.SuspendLayout();
            Player1.SuspendLayout();
            Player2.SuspendLayout();
            Player3.SuspendLayout();
            SuspendLayout();
            // 
            // MiddlePictureBox
            // 
            MiddlePictureBox.BackColor = Color.Transparent;
            MiddlePictureBox.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            MiddlePictureBox.Location = new Point(348, 132);
            MiddlePictureBox.Margin = new Padding(3, 2, 3, 2);
            MiddlePictureBox.Name = "MiddlePictureBox";
            MiddlePictureBox.Size = new Size(77, 88);
            MiddlePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            MiddlePictureBox.TabIndex = 0;
            MiddlePictureBox.TabStop = false;
            MiddlePictureBox.Visible = false;
            MiddlePictureBox.Click += drawCard_Click;
            // 
<<<<<<< HEAD
=======
            // AvatarPlayer
            // 
            AvatarPlayer.BackgroundImage = Properties.Resources.uno_card_red_poster_fahn507dk0y40lko;
            AvatarPlayer.Cursor = Cursors.Hand;
            AvatarPlayer.Image = Properties.Resources.avatar_removebg_preview;
            AvatarPlayer.Location = new Point(62, 314);
            AvatarPlayer.Margin = new Padding(3, 2, 3, 2);
            AvatarPlayer.Name = "AvatarPlayer";
            AvatarPlayer.Size = new Size(70, 60);
            AvatarPlayer.SizeMode = PictureBoxSizeMode.StretchImage;
            AvatarPlayer.TabIndex = 1;
            AvatarPlayer.TabStop = false;
            AvatarPlayer.Visible = false;
            AvatarPlayer.Click += AvatarPlayer_Click;
            // 
            // Enemy
            // 
            Enemy.BackColor = Color.Transparent;
            Enemy.Cursor = Cursors.Hand;
            Enemy.Image = Properties.Resources.avatar_removebg_preview;
            Enemy.Location = new Point(430, 14);
            Enemy.Margin = new Padding(3, 2, 3, 2);
            Enemy.Name = "Enemy";
            Enemy.Size = new Size(70, 60);
            Enemy.SizeMode = PictureBoxSizeMode.StretchImage;
            Enemy.TabIndex = 2;
            Enemy.TabStop = false;
            Enemy.Visible = false;
            Enemy.Click += Enemy_Click;
            // 
>>>>>>> timer chua xu phat
            // setting
            // 
            setting.BackColor = Color.Transparent;
            setting.Image = Properties.Resources.light_blue_settings_gear_22453__1_;
            setting.Location = new Point(8, 9);
            setting.Margin = new Padding(3, 2, 3, 2);
            setting.Name = "setting";
            setting.Size = new Size(41, 35);
            setting.SizeMode = PictureBoxSizeMode.Zoom;
            setting.TabIndex = 3;
            setting.TabStop = false;
            setting.Visible = false;
            setting.Click += setting_Click_1;
            // 
            // imojiButon
            // 
            imojiButon.BackColor = Color.Transparent;
            imojiButon.Image = Properties.Resources._19822c18e912ad0ffb2ad2faed8a61af__1__removebg_preview1;
            imojiButon.Location = new Point(8, 330);
            imojiButon.Margin = new Padding(3, 2, 3, 2);
            imojiButon.Name = "imojiButon";
            imojiButon.Size = new Size(49, 38);
            imojiButon.SizeMode = PictureBoxSizeMode.StretchImage;
            imojiButon.TabIndex = 4;
            imojiButon.TabStop = false;
            imojiButon.Visible = false;
            imojiButon.Click += pictureBox1_Click;
            // 
            // ClockIcon
            // 
            ClockIcon.BackColor = Color.Transparent;
            ClockIcon.Image = Properties.Resources.clock_removebg_preview;
            ClockIcon.Location = new Point(638, 330);
            ClockIcon.Margin = new Padding(3, 2, 3, 2);
            ClockIcon.Name = "ClockIcon";
            ClockIcon.Size = new Size(58, 44);
            ClockIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            ClockIcon.TabIndex = 5;
            ClockIcon.TabStop = false;
            ClockIcon.Visible = false;
            // 
            // clock1
            // 
            clock1.BackColor = Color.Transparent;
            clock1.Image = Properties.Resources.clock_removebg_preview;
            clock1.Location = new Point(54, 2);
            clock1.Margin = new Padding(3, 2, 3, 2);
            clock1.Name = "clock1";
            clock1.Size = new Size(58, 44);
            clock1.SizeMode = PictureBoxSizeMode.StretchImage;
            clock1.TabIndex = 6;
            clock1.TabStop = false;
            clock1.Visible = false;
            // 
            // DrawButton
            // 
            DrawButton.Cursor = Cursors.Hand;
            DrawButton.Location = new Point(688, 172);
            DrawButton.Margin = new Padding(3, 2, 3, 2);
            DrawButton.Name = "DrawButton";
            DrawButton.Size = new Size(66, 27);
            DrawButton.TabIndex = 8;
            DrawButton.Text = "Draw";
            DrawButton.UseVisualStyleBackColor = true;
            DrawButton.Visible = false;
            DrawButton.Click += button1_Click;
            // 
            // Card1
            // 
            Card1.BackColor = Color.Transparent;
            Card1.Cursor = Cursors.Hand;
            Card1.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            Card1.Location = new Point(137, 280);
            Card1.Margin = new Padding(3, 2, 3, 2);
            Card1.Name = "Card1";
            Card1.Size = new Size(77, 88);
            Card1.SizeMode = PictureBoxSizeMode.Zoom;
            Card1.TabIndex = 9;
            Card1.TabStop = false;
            Card1.Visible = false;
            Card1.Click += Card1_Click;
            // 
            // Card2
            // 
            Card2.BackColor = Color.Transparent;
            Card2.Cursor = Cursors.Hand;
            Card2.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            Card2.Location = new Point(218, 280);
            Card2.Margin = new Padding(3, 2, 3, 2);
            Card2.Name = "Card2";
            Card2.Size = new Size(77, 88);
            Card2.SizeMode = PictureBoxSizeMode.Zoom;
            Card2.TabIndex = 10;
            Card2.TabStop = false;
            Card2.Visible = false;
            Card2.Click += Card2_Click;
            // 
            // Card3
            // 
            Card3.BackColor = Color.Transparent;
            Card3.Cursor = Cursors.Hand;
            Card3.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            Card3.Location = new Point(298, 280);
            Card3.Margin = new Padding(3, 2, 3, 2);
            Card3.Name = "Card3";
            Card3.Size = new Size(77, 88);
            Card3.SizeMode = PictureBoxSizeMode.Zoom;
            Card3.TabIndex = 10;
            Card3.TabStop = false;
            Card3.Visible = false;
            // 
            // Card4
            // 
            Card4.BackColor = Color.Transparent;
            Card4.Cursor = Cursors.Hand;
            Card4.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            Card4.Location = new Point(380, 280);
            Card4.Margin = new Padding(3, 2, 3, 2);
            Card4.Name = "Card4";
            Card4.Size = new Size(77, 88);
            Card4.SizeMode = PictureBoxSizeMode.Zoom;
            Card4.TabIndex = 11;
            Card4.TabStop = false;
            Card4.Visible = false;
            // 
            // Card5
            // 
            Card5.BackColor = Color.Transparent;
            Card5.Cursor = Cursors.Hand;
            Card5.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            Card5.Location = new Point(462, 280);
            Card5.Margin = new Padding(3, 2, 3, 2);
            Card5.Name = "Card5";
            Card5.Size = new Size(77, 88);
            Card5.SizeMode = PictureBoxSizeMode.Zoom;
            Card5.TabIndex = 12;
            Card5.TabStop = false;
            Card5.Visible = false;
            Card5.Click += Card5_Click;
            // 
            // PreviousButton
            // 
            PreviousButton.Cursor = Cursors.Hand;
            PreviousButton.Location = new Point(88, 275);
            PreviousButton.Margin = new Padding(3, 2, 3, 2);
            PreviousButton.Name = "PreviousButton";
            PreviousButton.Size = new Size(32, 22);
            PreviousButton.TabIndex = 13;
            PreviousButton.Text = "<";
            PreviousButton.UseVisualStyleBackColor = true;
            PreviousButton.Visible = false;
            PreviousButton.Click += PreviousButton_Click;
            // 
            // NextButton
            // 
            NextButton.Cursor = Cursors.Hand;
            NextButton.Location = new Point(638, 275);
            NextButton.Margin = new Padding(3, 2, 3, 2);
            NextButton.Name = "NextButton";
            NextButton.Size = new Size(32, 22);
            NextButton.TabIndex = 14;
            NextButton.Text = ">";
            NextButton.UseVisualStyleBackColor = true;
            NextButton.Visible = false;
            NextButton.Click += NextButton_Click;
            // 
            // SortButton
            // 
            SortButton.Cursor = Cursors.Hand;
            SortButton.Location = new Point(8, 275);
            SortButton.Margin = new Padding(3, 2, 3, 2);
            SortButton.Name = "SortButton";
            SortButton.Size = new Size(66, 27);
            SortButton.TabIndex = 20;
            SortButton.Text = "Sort";
            SortButton.UseVisualStyleBackColor = true;
            SortButton.Visible = false;
            SortButton.Click += SortButton_Click;
            // 
            // Card6
            // 
            Card6.BackColor = Color.Transparent;
            Card6.Cursor = Cursors.Hand;
            Card6.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            Card6.Location = new Point(544, 280);
            Card6.Margin = new Padding(3, 2, 3, 2);
            Card6.Name = "Card6";
            Card6.Size = new Size(77, 88);
            Card6.SizeMode = PictureBoxSizeMode.Zoom;
            Card6.TabIndex = 21;
            Card6.TabStop = false;
            Card6.Visible = false;
            Card6.Click += Card6_Click;
            // 
            // ReadyBtn
            // 
            ReadyBtn.Cursor = Cursors.Hand;
            ReadyBtn.Location = new Point(688, 275);
            ReadyBtn.Margin = new Padding(3, 2, 3, 2);
            ReadyBtn.Name = "ReadyBtn";
            ReadyBtn.Size = new Size(66, 27);
            ReadyBtn.TabIndex = 23;
            ReadyBtn.Text = "Ready";
            ReadyBtn.UseVisualStyleBackColor = true;
            ReadyBtn.Click += ReadyBtn_Click;
            // 
            // isPlay1
            // 
            isPlay1.BackColor = SystemColors.Control;
            isPlay1.Cursor = Cursors.Hand;
            isPlay1.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            isPlay1.Location = new Point(62, 132);
            isPlay1.Margin = new Padding(3, 2, 3, 2);
            isPlay1.Name = "isPlay1";
            isPlay1.Size = new Size(95, 125);
            isPlay1.SizeMode = PictureBoxSizeMode.Zoom;
            isPlay1.TabIndex = 24;
            isPlay1.TabStop = false;
            // 
            // isPlay2
            // 
            isPlay2.BackColor = SystemColors.Control;
            isPlay2.Cursor = Cursors.Hand;
            isPlay2.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            isPlay2.Location = new Point(244, 132);
            isPlay2.Margin = new Padding(3, 2, 3, 2);
            isPlay2.Name = "isPlay2";
            isPlay2.Size = new Size(98, 125);
            isPlay2.SizeMode = PictureBoxSizeMode.Zoom;
            isPlay2.TabIndex = 25;
            isPlay2.TabStop = false;
            isPlay2.Click += isPlay2_Click;
            // 
            // Room
            // 
            Room.Location = new Point(8, 51);
            Room.Name = "Room";
            Room.Size = new Size(100, 23);
            Room.TabIndex = 26;
            Room.Text = "Room";
            Room.TextChanged += Room_TextChanged;
            // 
            // isPlay3
            // 
            isPlay3.BackColor = SystemColors.Control;
            isPlay3.Cursor = Cursors.Hand;
            isPlay3.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            isPlay3.Location = new Point(441, 132);
            isPlay3.Margin = new Padding(3, 2, 3, 2);
            isPlay3.Name = "isPlay3";
            isPlay3.Size = new Size(98, 125);
            isPlay3.SizeMode = PictureBoxSizeMode.Zoom;
            isPlay3.TabIndex = 27;
            isPlay3.TabStop = false;
            // 
            // isPlay4
            // 
            isPlay4.BackColor = SystemColors.Control;
            isPlay4.Cursor = Cursors.Hand;
            isPlay4.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            isPlay4.Location = new Point(610, 132);
            isPlay4.Margin = new Padding(3, 2, 3, 2);
            isPlay4.Name = "isPlay4";
            isPlay4.Size = new Size(98, 125);
            isPlay4.SizeMode = PictureBoxSizeMode.Zoom;
            isPlay4.TabIndex = 28;
            isPlay4.TabStop = false;
            // 
            // sqlCommand1
            // 
            sqlCommand1.CommandTimeout = 30;
            sqlCommand1.EnableOptimizedParameterBinding = false;
            // 
            // sqlCommand2
            // 
            sqlCommand2.CommandTimeout = 30;
            sqlCommand2.EnableOptimizedParameterBinding = false;
            // 
            // sqlCommand3
            // 
            sqlCommand3.CommandTimeout = 30;
            sqlCommand3.EnableOptimizedParameterBinding = false;
            // 
            // sqlCommand4
            // 
            sqlCommand4.CommandTimeout = 30;
            sqlCommand4.EnableOptimizedParameterBinding = false;
            // 
            // btnUno
            // 
            btnUno.Location = new Point(544, 172);
            btnUno.Margin = new Padding(3, 2, 3, 2);
            btnUno.Name = "btnUno";
            btnUno.Size = new Size(60, 27);
            btnUno.TabIndex = 29;
            btnUno.Text = "UNO!";
            btnUno.UseVisualStyleBackColor = true;
            btnUno.Click += btnUno_Click;
            // 
            // btnCatch
            // 
            btnCatch.Location = new Point(544, 203);
            btnCatch.Margin = new Padding(3, 2, 3, 2);
            btnCatch.Name = "btnCatch";
            btnCatch.Size = new Size(60, 26);
            btnCatch.TabIndex = 30;
            btnCatch.Text = "Catch";
            btnCatch.UseVisualStyleBackColor = true;
            btnCatch.Click += btnCatch_Click;
            // 
            // chatBox
            // 
            chatBox.Location = new Point(8, 76);
            chatBox.Margin = new Padding(3, 2, 3, 2);
            chatBox.Name = "chatBox";
            chatBox.Size = new Size(162, 144);
            chatBox.TabIndex = 31;
            chatBox.Text = "";
            // 
            // chatInput
            // 
            chatInput.Location = new Point(8, 224);
            chatInput.Margin = new Padding(3, 2, 3, 2);
            chatInput.Name = "chatInput";
            chatInput.Size = new Size(162, 23);
            chatInput.TabIndex = 32;
            chatInput.TextChanged += chatInput_TextChanged;
            // 
            // sendBtn
            // 
            sendBtn.Location = new Point(8, 249);
            sendBtn.Margin = new Padding(3, 2, 3, 2);
            sendBtn.Name = "sendBtn";
            sendBtn.Size = new Size(82, 22);
            sendBtn.TabIndex = 33;
            sendBtn.Text = "Send";
            sendBtn.UseVisualStyleBackColor = true;
            sendBtn.Click += sendBtn_Click;
            // 
            // resultLabel
            // 
            resultLabel.AutoSize = true;
            resultLabel.BackColor = Color.Transparent;
            resultLabel.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            resultLabel.ForeColor = SystemColors.Window;
            resultLabel.Location = new Point(298, 12);
            resultLabel.Name = "resultLabel";
            resultLabel.Size = new Size(131, 37);
            resultLabel.TabIndex = 34;
            resultLabel.Text = "KẾT QUẢ";
            resultLabel.Visible = false;
            // 
            // scoreLabel
            // 
            scoreLabel.AutoSize = true;
            scoreLabel.BackColor = Color.Transparent;
            scoreLabel.Font = new Font("Segoe UI", 13.2000008F, FontStyle.Italic, GraphicsUnit.Point, 0);
            scoreLabel.ForeColor = SystemColors.Window;
            scoreLabel.Location = new Point(244, 76);
            scoreLabel.Name = "scoreLabel";
            scoreLabel.Size = new Size(226, 25);
            scoreLabel.TabIndex = 35;
            scoreLabel.Text = "Điểm chiến thắng hiện tại: ";
            scoreLabel.Visible = false;
            // 
            // backBtn
            // 
            backBtn.BackColor = Color.LimeGreen;
            backBtn.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            backBtn.ForeColor = SystemColors.ButtonHighlight;
            backBtn.Location = new Point(313, 197);
            backBtn.Margin = new Padding(3, 2, 3, 2);
            backBtn.Name = "backBtn";
            backBtn.Size = new Size(158, 60);
            backBtn.TabIndex = 36;
            backBtn.Text = "BACK";
            backBtn.UseVisualStyleBackColor = false;
            backBtn.Visible = false;
            backBtn.Click += backBtn_Click;
            // 
<<<<<<< HEAD
=======
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = Properties.Resources.avatar_removebg_preview;
            pictureBox1.Location = new Point(175, 107);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(70, 60);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 37;
            pictureBox1.TabStop = false;
            pictureBox1.Visible = false;
            pictureBox1.Click += pictureBox1_Click_1;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Image = Properties.Resources.avatar_removebg_preview;
            pictureBox2.Location = new Point(668, 107);
            pictureBox2.Margin = new Padding(3, 2, 3, 2);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(70, 60);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 38;
            pictureBox2.TabStop = false;
            pictureBox2.Visible = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
>>>>>>> timer chua xu phat
            // Name1
            // 
            Name1.BackColor = Color.Transparent;
<<<<<<< HEAD
            Name1.ForeColor = Color.Black;
            Name1.Location = new Point(0, 0);
=======
            Name1.ForeColor = Color.White;
            Name1.Location = new Point(660, 90);
>>>>>>> timer chua xu phat
            Name1.Name = "Name1";
            Name1.Size = new Size(81, 15);
            Name1.TabIndex = 40;
            Name1.Text = "zzzzSatthuzzzz";
            Name1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Name2
            // 
            Name2.BackColor = Color.Transparent;
<<<<<<< HEAD
            Name2.ForeColor = Color.Black;
            Name2.Location = new Point(1, 0);
=======
            Name2.ForeColor = SystemColors.ButtonHighlight;
            Name2.Location = new Point(419, 0);
>>>>>>> timer chua xu phat
            Name2.Name = "Name2";
            Name2.Size = new Size(81, 15);
            Name2.TabIndex = 41;
            Name2.Text = "zzzzSatthuzzzz";
            Name2.TextAlign = ContentAlignment.MiddleCenter;
            Name2.Click += Name2_Click;
            // 
            // Name3
            // 
            Name3.BackColor = Color.Transparent;
<<<<<<< HEAD
            Name3.ForeColor = Color.Black;
            Name3.Location = new Point(1, 0);
=======
            Name3.ForeColor = Color.White;
            Name3.Location = new Point(169, 90);
>>>>>>> timer chua xu phat
            Name3.Name = "Name3";
            Name3.Size = new Size(81, 15);
            Name3.TabIndex = 42;
            Name3.Text = "zzzzSatthuzz";
            Name3.TextAlign = ContentAlignment.MiddleCenter;
            Name3.Click += Name3_Click;
            // 
            // NameMe
            // 
            NameMe.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            NameMe.BackColor = Color.Transparent;
<<<<<<< HEAD
            NameMe.ForeColor = Color.Black;
            NameMe.Location = new Point(3, 0);
=======
            NameMe.ForeColor = Color.White;
            NameMe.Location = new Point(54, 299);
>>>>>>> timer chua xu phat
            NameMe.Name = "NameMe";
            NameMe.Size = new Size(80, 15);
            NameMe.TabIndex = 39;
            NameMe.Text = "zzzsatthuzzzzz";
            NameMe.TextAlign = ContentAlignment.MiddleCenter;
            // 
<<<<<<< HEAD
            // Number3
            // 
            Number3.AutoSize = true;
            Number3.BackColor = Color.Cyan;
            Number3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Number3.ForeColor = Color.Red;
            Number3.Location = new Point(23, 24);
            Number3.Name = "Number3";
            Number3.Size = new Size(68, 20);
=======
            // NumberMe
            // 
            NumberMe.AutoSize = true;
            NumberMe.BackColor = Color.Transparent;
            NumberMe.ForeColor = Color.White;
            NumberMe.Location = new Point(137, 262);
            NumberMe.Name = "NumberMe";
            NumberMe.Size = new Size(80, 15);
            NumberMe.TabIndex = 43;
            NumberMe.Text = "zzzsatthuzzzzz";
            NumberMe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Number3
            // 
            Number3.AutoSize = true;
            Number3.BackColor = Color.Transparent;
            Number3.ForeColor = Color.White;
            Number3.Location = new Point(250, 115);
            Number3.Name = "Number3";
            Number3.Size = new Size(80, 15);
>>>>>>> timer chua xu phat
            Number3.TabIndex = 44;
            Number3.Text = "Số lá: 15";
            Number3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Number2
            // 
            Number2.AutoSize = true;
<<<<<<< HEAD
            Number2.BackColor = Color.Cyan;
            Number2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Number2.ForeColor = Color.Red;
            Number2.Location = new Point(22, 24);
            Number2.Name = "Number2";
            Number2.Size = new Size(68, 20);
=======
            Number2.BackColor = Color.Transparent;
            Number2.ForeColor = Color.White;
            Number2.Location = new Point(419, 76);
            Number2.Name = "Number2";
            Number2.Size = new Size(80, 15);
>>>>>>> timer chua xu phat
            Number2.TabIndex = 45;
            Number2.Text = "Số lá: 14";
            Number2.TextAlign = ContentAlignment.MiddleCenter;
            Number2.Click += Number2_Click;
            // 
            // Number1
            // 
            Number1.AutoSize = true;
<<<<<<< HEAD
            Number1.BackColor = Color.Cyan;
            Number1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Number1.ForeColor = Color.Red;
            Number1.Location = new Point(20, 24);
            Number1.Name = "Number1";
            Number1.Size = new Size(68, 20);
=======
            Number1.BackColor = Color.Transparent;
            Number1.ForeColor = Color.White;
            Number1.Location = new Point(578, 115);
            Number1.Name = "Number1";
            Number1.Size = new Size(80, 15);
>>>>>>> timer chua xu phat
            Number1.TabIndex = 46;
            Number1.Text = "Số lá: 13";
            Number1.TextAlign = ContentAlignment.MiddleCenter;
            Number1.Click += Number1_Click;
            // 
            // Me
            // 
            Me.Controls.Add(NumberMe);
            Me.Controls.Add(NameMe);
            Me.Location = new Point(212, 12);
            Me.Name = "Me";
            Me.Size = new Size(111, 47);
            Me.TabIndex = 47;
            // 
            // NumberMe
            // 
            NumberMe.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            NumberMe.AutoSize = true;
            NumberMe.BackColor = Color.Cyan;
            NumberMe.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NumberMe.ForeColor = Color.Red;
            NumberMe.Location = new Point(21, 24);
            NumberMe.Name = "NumberMe";
            NumberMe.Size = new Size(68, 20);
            NumberMe.TabIndex = 43;
            NumberMe.Text = "Số lá: 12";
            NumberMe.TextAlign = ContentAlignment.MiddleCenter;
            NumberMe.Click += NumberMe_Click;
            // 
            // Player1
            // 
            Player1.Controls.Add(Name1);
            Player1.Controls.Add(Number1);
            Player1.Location = new Point(341, 12);
            Player1.Name = "Player1";
            Player1.Size = new Size(111, 47);
            Player1.TabIndex = 48;
            // 
            // Player2
            // 
            Player2.Controls.Add(Name2);
            Player2.Controls.Add(Number2);
            Player2.Location = new Point(469, 12);
            Player2.Name = "Player2";
            Player2.Size = new Size(111, 47);
            Player2.TabIndex = 49;
            // 
            // Player3
            // 
            Player3.Controls.Add(Name3);
            Player3.Controls.Add(Number3);
            Player3.Location = new Point(599, 12);
            Player3.Name = "Player3";
            Player3.Size = new Size(111, 47);
            Player3.TabIndex = 50;
            // 
            // lblTimer
            // 
            lblTimer.BackColor = Color.Transparent;
            lblTimer.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTimer.Location = new Point(702, 330);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(52, 44);
            lblTimer.TabIndex = 7;
            lblTimer.Text = "10";
            lblTimer.TextAlign = ContentAlignment.MiddleCenter;
            lblTimer.Visible = false;
            lblTimer.Click += label1_Click;
            // 
            // TimeEnemy
            // 
            TimeEnemy.BackColor = Color.Transparent;
            TimeEnemy.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TimeEnemy.Location = new Point(117, 0);
            TimeEnemy.Name = "TimeEnemy";
            TimeEnemy.Size = new Size(52, 44);
            TimeEnemy.TabIndex = 22;
            TimeEnemy.Text = "10";
            TimeEnemy.TextAlign = ContentAlignment.MiddleCenter;
            TimeEnemy.Visible = false;
            TimeEnemy.Click += TimeEnemy_Click;
            // 
            // Arena
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.uno_card_red_poster_fahn507dk0y40lko;
            BackgroundImageLayout = ImageLayout.Stretch;
<<<<<<< HEAD
            ClientSize = new Size(875, 511);
            Controls.Add(Player3);
            Controls.Add(Player2);
            Controls.Add(Player1);
            Controls.Add(Me);
=======
            ClientSize = new Size(766, 383);
            Controls.Add(Number1);
            Controls.Add(Number2);
            Controls.Add(Number3);
            Controls.Add(NumberMe);
            Controls.Add(Name3);
            Controls.Add(Name2);
            Controls.Add(Name1);
            Controls.Add(NameMe);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
>>>>>>> timer chua xu phat
            Controls.Add(backBtn);
            Controls.Add(scoreLabel);
            Controls.Add(resultLabel);
            Controls.Add(sendBtn);
            Controls.Add(chatInput);
            Controls.Add(chatBox);
            Controls.Add(btnCatch);
            Controls.Add(btnUno);
            Controls.Add(isPlay4);
            Controls.Add(isPlay3);
            Controls.Add(Room);
            Controls.Add(isPlay2);
            Controls.Add(isPlay1);
            Controls.Add(ReadyBtn);
            Controls.Add(TimeEnemy);
            Controls.Add(Card6);
            Controls.Add(SortButton);
            Controls.Add(NextButton);
            Controls.Add(PreviousButton);
            Controls.Add(Card5);
            Controls.Add(Card4);
            Controls.Add(Card3);
            Controls.Add(Card2);
            Controls.Add(Card1);
            Controls.Add(DrawButton);
            Controls.Add(lblTimer);
            Controls.Add(clock1);
            Controls.Add(ClockIcon);
            Controls.Add(imojiButon);
            Controls.Add(setting);
            Controls.Add(MiddlePictureBox);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "Arena";
            Text = "Arena";
            Load += Arena_Load;
            ((System.ComponentModel.ISupportInitialize)MiddlePictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)setting).EndInit();
            ((System.ComponentModel.ISupportInitialize)imojiButon).EndInit();
            ((System.ComponentModel.ISupportInitialize)ClockIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)clock1).EndInit();
            ((System.ComponentModel.ISupportInitialize)Card1).EndInit();
            ((System.ComponentModel.ISupportInitialize)Card2).EndInit();
            ((System.ComponentModel.ISupportInitialize)Card3).EndInit();
            ((System.ComponentModel.ISupportInitialize)Card4).EndInit();
            ((System.ComponentModel.ISupportInitialize)Card5).EndInit();
            ((System.ComponentModel.ISupportInitialize)Card6).EndInit();
            ((System.ComponentModel.ISupportInitialize)isPlay1).EndInit();
            ((System.ComponentModel.ISupportInitialize)isPlay2).EndInit();
            ((System.ComponentModel.ISupportInitialize)isPlay3).EndInit();
            ((System.ComponentModel.ISupportInitialize)isPlay4).EndInit();
            Me.ResumeLayout(false);
            Me.PerformLayout();
            Player1.ResumeLayout(false);
            Player1.PerformLayout();
            Player2.ResumeLayout(false);
            Player2.PerformLayout();
            Player3.ResumeLayout(false);
            Player3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox MiddlePictureBox;
        private PictureBox setting;
        private PictureBox imojiButon;
        private PictureBox ClockIcon;
        private PictureBox clock1;
        private Button DrawButton;
        private PictureBox Card1;
        private PictureBox Card2;
        private PictureBox Card3;
        private PictureBox Card4;
        private PictureBox Card5;
        private Button PreviousButton;
        private Button NextButton;
        private Button SortButton;
        private PictureBox Card6;
        private Button ReadyBtn;
        private PictureBox isPlay1;
        private PictureBox isPlay2;
        private TextBox Room;
        private PictureBox isPlay3;
        private PictureBox isPlay4;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
        private Microsoft.Data.SqlClient.SqlCommandBuilder sqlCommandBuilder1;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand2;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand3;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand4;
        private Microsoft.Data.SqlClient.SqlCommandBuilder sqlCommandBuilder2;
        private Button btnUno;
        private Button btnCatch;
        private RichTextBox chatBox;
        private TextBox chatInput;
        private Button sendBtn;
        private Label resultLabel;
        private Label scoreLabel;
        private Button backBtn;
        private Label Name1;
        private Label Name2;
        private Label Name3;
        private Label NameMe;
        private Label Number3;
        private Label Number2;
        private Label Number1;
<<<<<<< HEAD
        private Panel Me;
        private Label NumberMe;
        private Panel Player1;
        private Panel Player2;
        private Panel Player3;
=======
        private Label lblTimer;
        private Label TimeEnemy;
>>>>>>> timer chua xu phat
    }
}
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
            imojiButon = new PictureBox();
            ClockIcon = new PictureBox();
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
            backBtn = new Button();
            Name1 = new Label();
            Name2 = new Label();
            Name3 = new Label();
            NameMe = new Label();
            Number3 = new Label();
            Number2 = new Label();
            Number1 = new Label();
            lblTimer = new Label();
            Me = new Panel();
            NumberMe = new Label();
            Player1 = new Panel();
            Player2 = new Panel();
            Player3 = new Panel();
            CurentColor = new Label();
            ((System.ComponentModel.ISupportInitialize)MiddlePictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imojiButon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ClockIcon).BeginInit();
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
            MiddlePictureBox.Location = new Point(398, 176);
            MiddlePictureBox.Name = "MiddlePictureBox";
            MiddlePictureBox.Size = new Size(88, 117);
            MiddlePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            MiddlePictureBox.TabIndex = 0;
            MiddlePictureBox.TabStop = false;
            MiddlePictureBox.Visible = false;
            MiddlePictureBox.Click += drawCard_Click;
            // 
            // imojiButon
            // 
            imojiButon.BackColor = Color.Transparent;
            imojiButon.Image = Properties.Resources._19822c18e912ad0ffb2ad2faed8a61af__1__removebg_preview1;
            imojiButon.Location = new Point(109, 332);
            imojiButon.Name = "imojiButon";
            imojiButon.Size = new Size(29, 29);
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
            ClockIcon.Location = new Point(729, 440);
            ClockIcon.Name = "ClockIcon";
            ClockIcon.Size = new Size(66, 59);
            ClockIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            ClockIcon.TabIndex = 5;
            ClockIcon.TabStop = false;
            ClockIcon.Visible = false;
            // 
            // DrawButton
            // 
            DrawButton.Cursor = Cursors.Hand;
            DrawButton.Location = new Point(786, 229);
            DrawButton.Name = "DrawButton";
            DrawButton.Size = new Size(75, 36);
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
            Card1.Location = new Point(157, 373);
            Card1.Name = "Card1";
            Card1.Size = new Size(88, 117);
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
            Card2.Location = new Point(249, 373);
            Card2.Name = "Card2";
            Card2.Size = new Size(88, 117);
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
            Card3.Location = new Point(341, 373);
            Card3.Name = "Card3";
            Card3.Size = new Size(88, 117);
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
            Card4.Location = new Point(434, 373);
            Card4.Name = "Card4";
            Card4.Size = new Size(88, 117);
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
            Card5.Location = new Point(528, 373);
            Card5.Name = "Card5";
            Card5.Size = new Size(88, 117);
            Card5.SizeMode = PictureBoxSizeMode.Zoom;
            Card5.TabIndex = 12;
            Card5.TabStop = false;
            Card5.Visible = false;
            Card5.Click += Card5_Click;
            // 
            // PreviousButton
            // 
            PreviousButton.Cursor = Cursors.Hand;
            PreviousButton.Location = new Point(101, 367);
            PreviousButton.Name = "PreviousButton";
            PreviousButton.Size = new Size(37, 29);
            PreviousButton.TabIndex = 13;
            PreviousButton.Text = "<";
            PreviousButton.UseVisualStyleBackColor = true;
            PreviousButton.Visible = false;
            PreviousButton.Click += PreviousButton_Click;
            // 
            // NextButton
            // 
            NextButton.Cursor = Cursors.Hand;
            NextButton.Location = new Point(729, 367);
            NextButton.Name = "NextButton";
            NextButton.Size = new Size(37, 29);
            NextButton.TabIndex = 14;
            NextButton.Text = ">";
            NextButton.UseVisualStyleBackColor = true;
            NextButton.Visible = false;
            NextButton.Click += NextButton_Click;
            // 
            // SortButton
            // 
            SortButton.Cursor = Cursors.Hand;
            SortButton.Location = new Point(9, 367);
            SortButton.Name = "SortButton";
            SortButton.Size = new Size(75, 36);
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
            Card6.Location = new Point(622, 373);
            Card6.Name = "Card6";
            Card6.Size = new Size(88, 117);
            Card6.SizeMode = PictureBoxSizeMode.Zoom;
            Card6.TabIndex = 21;
            Card6.TabStop = false;
            Card6.Visible = false;
            Card6.Click += Card6_Click;
            // 
            // ReadyBtn
            // 
            ReadyBtn.Cursor = Cursors.Hand;
            ReadyBtn.Location = new Point(786, 367);
            ReadyBtn.Name = "ReadyBtn";
            ReadyBtn.Size = new Size(75, 36);
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
            isPlay1.Location = new Point(71, 176);
            isPlay1.Name = "isPlay1";
            isPlay1.Size = new Size(109, 167);
            isPlay1.SizeMode = PictureBoxSizeMode.Zoom;
            isPlay1.TabIndex = 24;
            isPlay1.TabStop = false;
            // 
            // isPlay2
            // 
            isPlay2.BackColor = SystemColors.Control;
            isPlay2.Cursor = Cursors.Hand;
            isPlay2.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            isPlay2.Location = new Point(279, 176);
            isPlay2.Name = "isPlay2";
            isPlay2.Size = new Size(112, 167);
            isPlay2.SizeMode = PictureBoxSizeMode.Zoom;
            isPlay2.TabIndex = 25;
            isPlay2.TabStop = false;
            isPlay2.Click += isPlay2_Click;
            // 
            // Room
            // 
            Room.Location = new Point(9, 68);
            Room.Margin = new Padding(3, 4, 3, 4);
            Room.Name = "Room";
            Room.ReadOnly = true;
            Room.Size = new Size(114, 27);
            Room.TabIndex = 26;
            Room.Text = "Room";
            Room.TextChanged += Room_TextChanged;
            // 
            // isPlay3
            // 
            isPlay3.BackColor = SystemColors.Control;
            isPlay3.Cursor = Cursors.Hand;
            isPlay3.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            isPlay3.Location = new Point(504, 176);
            isPlay3.Name = "isPlay3";
            isPlay3.Size = new Size(112, 167);
            isPlay3.SizeMode = PictureBoxSizeMode.Zoom;
            isPlay3.TabIndex = 27;
            isPlay3.TabStop = false;
            // 
            // isPlay4
            // 
            isPlay4.BackColor = SystemColors.Control;
            isPlay4.Cursor = Cursors.Hand;
            isPlay4.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            isPlay4.Location = new Point(697, 176);
            isPlay4.Name = "isPlay4";
            isPlay4.Size = new Size(112, 167);
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
            btnUno.Location = new Point(622, 229);
            btnUno.Name = "btnUno";
            btnUno.Size = new Size(69, 36);
            btnUno.TabIndex = 29;
            btnUno.Text = "UNO!";
            btnUno.UseVisualStyleBackColor = true;
            btnUno.Click += btnUno_Click;
            // 
            // btnCatch
            // 
            btnCatch.Location = new Point(622, 271);
            btnCatch.Name = "btnCatch";
            btnCatch.Size = new Size(69, 35);
            btnCatch.TabIndex = 30;
            btnCatch.Text = "Catch";
            btnCatch.UseVisualStyleBackColor = true;
            btnCatch.Click += btnCatch_Click;
            // 
            // chatBox
            // 
            chatBox.Location = new Point(9, 101);
            chatBox.Name = "chatBox";
            chatBox.Size = new Size(185, 191);
            chatBox.TabIndex = 31;
            chatBox.Text = "";
            // 
            // chatInput
            // 
            chatInput.Location = new Point(9, 299);
            chatInput.Name = "chatInput";
            chatInput.Size = new Size(185, 27);
            chatInput.TabIndex = 32;
            chatInput.TextChanged += chatInput_TextChanged;
            // 
            // sendBtn
            // 
            sendBtn.Location = new Point(9, 332);
            sendBtn.Name = "sendBtn";
            sendBtn.Size = new Size(94, 29);
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
            resultLabel.Location = new Point(9, 315);
            resultLabel.Name = "resultLabel";
            resultLabel.Size = new Size(164, 46);
            resultLabel.TabIndex = 34;
            resultLabel.Text = "KẾT QUẢ";
            resultLabel.Visible = false;
            // 
            // backBtn
            // 
            backBtn.BackColor = Color.LimeGreen;
            backBtn.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            backBtn.ForeColor = SystemColors.ButtonHighlight;
            backBtn.Location = new Point(13, 419);
            backBtn.Name = "backBtn";
            backBtn.Size = new Size(181, 80);
            backBtn.TabIndex = 36;
            backBtn.Text = "BACK";
            backBtn.UseVisualStyleBackColor = false;
            backBtn.Visible = false;
            backBtn.Click += backBtn_Click;
            // 
            // Name1
            // 
            Name1.BackColor = Color.Transparent;
            Name1.ForeColor = Color.Black;
            Name1.Location = new Point(8, 0);
            Name1.Name = "Name1";
            Name1.Size = new Size(93, 20);
            Name1.TabIndex = 40;
            Name1.Text = "zzzzSatthuzzzz";
            Name1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Name2
            // 
            Name2.BackColor = Color.Transparent;
            Name2.ForeColor = Color.Black;
            Name2.Location = new Point(8, 0);
            Name2.Name = "Name2";
            Name2.Size = new Size(93, 20);
            Name2.TabIndex = 41;
            Name2.Text = "zzzzSatthuzzzz";
            Name2.TextAlign = ContentAlignment.MiddleCenter;
            Name2.Click += Name2_Click;
            // 
            // Name3
            // 
            Name3.BackColor = Color.Transparent;
            Name3.ForeColor = Color.Black;
            Name3.Location = new Point(8, 0);
            Name3.Name = "Name3";
            Name3.Size = new Size(93, 20);
            Name3.TabIndex = 42;
            Name3.Text = "zzzzSatthuzz";
            Name3.TextAlign = ContentAlignment.MiddleCenter;
            Name3.Click += Name3_Click;
            // 
            // NameMe
            // 
            NameMe.BackColor = Color.Transparent;
            NameMe.ForeColor = Color.Black;
            NameMe.Location = new Point(8, 0);
            NameMe.Name = "NameMe";
            NameMe.Size = new Size(88, 20);
            NameMe.TabIndex = 39;
            NameMe.Text = "zzzsatthuzzzzz";
            NameMe.TextAlign = ContentAlignment.MiddleCenter;
            NameMe.Click += NameMe_Click;
            // 
            // Number3
            // 
            Number3.AutoSize = true;
            Number3.BackColor = Color.Cyan;
            Number3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Number3.ForeColor = Color.Red;
            Number3.Location = new Point(23, 24);
            Number3.Name = "Number3";
            Number3.Size = new Size(68, 20);
            Number3.TabIndex = 44;
            Number3.Text = "Số lá: 15";
            Number3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Number2
            // 
            Number2.AutoSize = true;
            Number2.BackColor = Color.Cyan;
            Number2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Number2.ForeColor = Color.Red;
            Number2.Location = new Point(23, 24);
            Number2.Name = "Number2";
            Number2.Size = new Size(68, 20);
            Number2.TabIndex = 45;
            Number2.Text = "Số lá: 14";
            Number2.TextAlign = ContentAlignment.MiddleCenter;
            Number2.Click += Number2_Click;
            // 
            // Number1
            // 
            Number1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            Number1.AutoSize = true;
            Number1.BackColor = Color.Cyan;
            Number1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Number1.ForeColor = Color.Red;
            Number1.Location = new Point(23, 24);
            Number1.Name = "Number1";
            Number1.Size = new Size(68, 20);
            Number1.TabIndex = 46;
            Number1.Text = "Số lá: 13";
            Number1.TextAlign = ContentAlignment.MiddleCenter;
            Number1.Click += Number1_Click;
            // 
            // lblTimer
            // 
            lblTimer.BackColor = Color.Transparent;
            lblTimer.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTimer.Location = new Point(802, 440);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(59, 59);
            lblTimer.TabIndex = 7;
            lblTimer.Text = "10";
            lblTimer.TextAlign = ContentAlignment.MiddleCenter;
            lblTimer.Visible = false;
            lblTimer.Click += label1_Click;
            // 
            // Me
            // 
            Me.Controls.Add(NameMe);
            Me.Controls.Add(NumberMe);
            Me.Location = new Point(242, 16);
            Me.Margin = new Padding(3, 4, 3, 4);
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
            NumberMe.Location = new Point(23, 24);
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
            Player1.Location = new Point(390, 16);
            Player1.Margin = new Padding(3, 4, 3, 4);
            Player1.Name = "Player1";
            Player1.Size = new Size(111, 47);
            Player1.TabIndex = 48;
            // 
            // Player2
            // 
            Player2.Controls.Add(Name2);
            Player2.Controls.Add(Number2);
            Player2.Location = new Point(536, 16);
            Player2.Margin = new Padding(3, 4, 3, 4);
            Player2.Name = "Player2";
            Player2.Size = new Size(111, 47);
            Player2.TabIndex = 49;
            // 
            // Player3
            // 
            Player3.Controls.Add(Name3);
            Player3.Controls.Add(Number3);
            Player3.Location = new Point(685, 16);
            Player3.Margin = new Padding(3, 4, 3, 4);
            Player3.Name = "Player3";
            Player3.Size = new Size(111, 47);
            Player3.TabIndex = 50;
            // 
            // CurentColor
            // 
            CurentColor.AutoSize = true;
            CurentColor.BackColor = Color.White;
            CurentColor.ForeColor = Color.Black;
            CurentColor.Location = new Point(398, 144);
            CurentColor.Name = "CurentColor";
            CurentColor.Size = new Size(91, 20);
            CurentColor.TabIndex = 51;
            CurentColor.Text = "Màu hiện tại";
            CurentColor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Arena
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.uno_card_red_poster_fahn507dk0y40lko;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(875, 511);
            Controls.Add(CurentColor);
            Controls.Add(Player3);
            Controls.Add(Player2);
            Controls.Add(Player1);
            Controls.Add(Me);
            Controls.Add(backBtn);
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
            Controls.Add(ClockIcon);
            Controls.Add(imojiButon);
            Controls.Add(MiddlePictureBox);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "Arena";
            Text = "Arena";
            Load += Arena_Load;
            ((System.ComponentModel.ISupportInitialize)MiddlePictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)imojiButon).EndInit();
            ((System.ComponentModel.ISupportInitialize)ClockIcon).EndInit();
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
        private PictureBox imojiButon;
        private PictureBox ClockIcon;
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
        private Button backBtn;
        private Label Name1;
        private Label Name2;
        private Label Name3;
        private Label NameMe;
        private Label Number3;
        private Label Number2;
        private Label Number1;
        private Label lblTimer;
        private Panel Me;
        private Label NumberMe;
        private Panel Player1;
        private Panel Player2;
        private Panel Player3;
        private Label CurentColor;
    }
}
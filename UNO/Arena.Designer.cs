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
            AvatarPlayer = new PictureBox();
            Enemy = new PictureBox();
            setting = new PictureBox();
            imojiButon = new PictureBox();
            ClockIcon = new PictureBox();
            clock1 = new PictureBox();
            TimeMe = new Label();
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
            TimeEnemy = new Label();
            ReadyBtn = new Button();
            isPlay1 = new PictureBox();
            isPlay2 = new PictureBox();
            Room = new TextBox();
            isPlay3 = new PictureBox();
            isPlay4 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)MiddlePictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AvatarPlayer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Enemy).BeginInit();
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
            Enemy.Location = new Point(638, 2);
            Enemy.Margin = new Padding(3, 2, 3, 2);
            Enemy.Name = "Enemy";
            Enemy.Size = new Size(70, 60);
            Enemy.SizeMode = PictureBoxSizeMode.StretchImage;
            Enemy.TabIndex = 2;
            Enemy.TabStop = false;
            Enemy.Visible = false;
            Enemy.Click += Enemy_Click;
            // 
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
            // TimeMe
            // 
            TimeMe.BackColor = Color.Transparent;
            TimeMe.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TimeMe.Location = new Point(702, 330);
            TimeMe.Name = "TimeMe";
            TimeMe.Size = new Size(52, 44);
            TimeMe.TabIndex = 7;
            TimeMe.Text = "10";
            TimeMe.TextAlign = ContentAlignment.MiddleCenter;
            TimeMe.Visible = false;
            TimeMe.Click += label1_Click;
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
            PreviousButton.Location = new Point(93, 280);
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
            NextButton.Location = new Point(627, 280);
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
            // Arena
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.uno_card_red_poster_fahn507dk0y40lko;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(766, 383);
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
            Controls.Add(TimeMe);
            Controls.Add(clock1);
            Controls.Add(ClockIcon);
            Controls.Add(imojiButon);
            Controls.Add(setting);
            Controls.Add(Enemy);
            Controls.Add(AvatarPlayer);
            Controls.Add(MiddlePictureBox);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "Arena";
            Text = "Arena";
            ((System.ComponentModel.ISupportInitialize)MiddlePictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AvatarPlayer).EndInit();
            ((System.ComponentModel.ISupportInitialize)Enemy).EndInit();
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
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox MiddlePictureBox;
        private PictureBox AvatarPlayer;
        private PictureBox Enemy;
        private PictureBox setting;
        private PictureBox imojiButon;
        private PictureBox ClockIcon;
        private PictureBox clock1;
        private Label TimeMe;
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
        private Label TimeEnemy;
        private Button ReadyBtn;
        private PictureBox isPlay1;
        private PictureBox isPlay2;
        private TextBox Room;
        private PictureBox isPlay3;
        private PictureBox isPlay4;
    }
}
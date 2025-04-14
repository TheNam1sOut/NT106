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
            drawCard = new PictureBox();
            AvatarPlayer = new PictureBox();
            Enemy = new PictureBox();
            setting = new PictureBox();
            imojiButon = new PictureBox();
            ClockIcon = new PictureBox();
            clock1 = new PictureBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)drawCard).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AvatarPlayer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Enemy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)setting).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imojiButon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ClockIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)clock1).BeginInit();
            SuspendLayout();
            // 
            // drawCard
            // 
            drawCard.BackColor = Color.Transparent;
            drawCard.Image = Properties.Resources.pngtree_uno_card_png_image_9101654;
            drawCard.Location = new Point(327, 137);
            drawCard.Margin = new Padding(3, 2, 3, 2);
            drawCard.Name = "drawCard";
            drawCard.Size = new Size(105, 91);
            drawCard.SizeMode = PictureBoxSizeMode.StretchImage;
            drawCard.TabIndex = 0;
            drawCard.TabStop = false;
            drawCard.Click += drawCard_Click;
            // 
            // AvatarPlayer
            // 
            AvatarPlayer.BackgroundImage = Properties.Resources.uno_card_red_poster_fahn507dk0y40lko;
            AvatarPlayer.Image = Properties.Resources.avatar_removebg_preview;
            AvatarPlayer.Location = new Point(62, 314);
            AvatarPlayer.Margin = new Padding(3, 2, 3, 2);
            AvatarPlayer.Name = "AvatarPlayer";
            AvatarPlayer.Size = new Size(70, 60);
            AvatarPlayer.SizeMode = PictureBoxSizeMode.StretchImage;
            AvatarPlayer.TabIndex = 1;
            AvatarPlayer.TabStop = false;
            AvatarPlayer.Click += AvatarPlayer_Click;
            // 
            // Enemy
            // 
            Enemy.BackColor = Color.Transparent;
            Enemy.Image = Properties.Resources.avatar_removebg_preview;
            Enemy.Location = new Point(638, 2);
            Enemy.Margin = new Padding(3, 2, 3, 2);
            Enemy.Name = "Enemy";
            Enemy.Size = new Size(70, 60);
            Enemy.SizeMode = PictureBoxSizeMode.StretchImage;
            Enemy.TabIndex = 2;
            Enemy.TabStop = false;
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
            // 
            // clock1
            // 
            clock1.BackColor = Color.Transparent;
            clock1.Image = Properties.Resources.clock_removebg_preview;
            clock1.Location = new Point(67, 2);
            clock1.Margin = new Padding(3, 2, 3, 2);
            clock1.Name = "clock1";
            clock1.Size = new Size(58, 44);
            clock1.SizeMode = PictureBoxSizeMode.StretchImage;
            clock1.TabIndex = 6;
            clock1.TabStop = false;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(702, 330);
            label1.Name = "label1";
            label1.Size = new Size(52, 44);
            label1.TabIndex = 7;
            label1.Text = "10";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Click += label1_Click;
            // 
            // Arena
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.uno_card_red_poster_fahn507dk0y40lko;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(766, 383);
            Controls.Add(label1);
            Controls.Add(clock1);
            Controls.Add(ClockIcon);
            Controls.Add(imojiButon);
            Controls.Add(setting);
            Controls.Add(Enemy);
            Controls.Add(AvatarPlayer);
            Controls.Add(drawCard);
            Margin = new Padding(3, 2, 3, 2);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "Arena";
            Text = "Arena";
            Load += Arena_Load;
            ((System.ComponentModel.ISupportInitialize)drawCard).EndInit();
            ((System.ComponentModel.ISupportInitialize)AvatarPlayer).EndInit();
            ((System.ComponentModel.ISupportInitialize)Enemy).EndInit();
            ((System.ComponentModel.ISupportInitialize)setting).EndInit();
            ((System.ComponentModel.ISupportInitialize)imojiButon).EndInit();
            ((System.ComponentModel.ISupportInitialize)ClockIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)clock1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox drawCard;
        private PictureBox AvatarPlayer;
        private PictureBox Enemy;
        private PictureBox setting;
        private PictureBox imojiButon;
        private PictureBox ClockIcon;
        private PictureBox clock1;
        private Label label1;
    }
}
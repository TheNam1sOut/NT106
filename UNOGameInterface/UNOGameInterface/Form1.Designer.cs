namespace UNOGameInterface
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.picDrawPile = new System.Windows.Forms.PictureBox();
            this.Emoji = new System.Windows.Forms.PictureBox();
            this.Player2 = new System.Windows.Forms.PictureBox();
            this.Player1 = new System.Windows.Forms.PictureBox();
            this.Setting = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picDrawPile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Emoji)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Player2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Player1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Setting)).BeginInit();
            this.SuspendLayout();
            // 
            // picDrawPile
            // 
            resources.ApplyResources(this.picDrawPile, "picDrawPile");
            this.picDrawPile.Image = global::UNOGameInterface.Properties.Resources.pngtree_uno_card_png_image_9101654;
            this.picDrawPile.Name = "picDrawPile";
            this.picDrawPile.TabStop = false;
            this.picDrawPile.Click += new System.EventHandler(this.picDrawPile_Click);
            this.picDrawPile.MouseEnter += new System.EventHandler(this.picDrawPile_MouseEnter);
            this.picDrawPile.MouseLeave += new System.EventHandler(this.picDrawPile_MouseLeave);
            // 
            // Emoji
            // 
            this.Emoji.Image = global::UNOGameInterface.Properties.Resources._19822c18e912ad0ffb2ad2faed8a61af__1__removebg_preview1;
            resources.ApplyResources(this.Emoji, "Emoji");
            this.Emoji.Name = "Emoji";
            this.Emoji.TabStop = false;
            this.Emoji.Click += new System.EventHandler(this.pictureBox1_Click);
            this.Emoji.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.Emoji.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // Player2
            // 
            this.Player2.Image = global::UNOGameInterface.Properties.Resources.avatar_removebg_preview;
            resources.ApplyResources(this.Player2, "Player2");
            this.Player2.Name = "Player2";
            this.Player2.TabStop = false;
            // 
            // Player1
            // 
            this.Player1.Image = global::UNOGameInterface.Properties.Resources.avatar_removebg_preview;
            resources.ApplyResources(this.Player1, "Player1");
            this.Player1.Name = "Player1";
            this.Player1.TabStop = false;
            // 
            // Setting
            // 
            this.Setting.Image = global::UNOGameInterface.Properties.Resources.settings_glyph_black_icon_png_292947_removebg_preview;
            resources.ApplyResources(this.Setting, "Setting");
            this.Setting.Name = "Setting";
            this.Setting.TabStop = false;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::UNOGameInterface.Properties.Resources.uno_card_red_poster_fahn507dk0y40lko;
            this.Controls.Add(this.Setting);
            this.Controls.Add(this.Player1);
            this.Controls.Add(this.Player2);
            this.Controls.Add(this.Emoji);
            this.Controls.Add(this.picDrawPile);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picDrawPile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Emoji)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Player2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Player1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Setting)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox picDrawPile;
        private System.Windows.Forms.PictureBox Emoji;
        private System.Windows.Forms.PictureBox Player2;
        private System.Windows.Forms.PictureBox Player1;
        private System.Windows.Forms.PictureBox Setting;
    }
}


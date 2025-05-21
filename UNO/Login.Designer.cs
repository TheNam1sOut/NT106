namespace UNO
{
    partial class Login
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            userLabel = new Label();
            loginBtn = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            password = new TextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // userLabel
            // 
            userLabel.AutoSize = true;
            userLabel.BackColor = Color.Transparent;
            userLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            userLabel.ForeColor = Color.DimGray;
            userLabel.Location = new Point(338, 151);
            userLabel.Name = "userLabel";
            userLabel.Size = new Size(62, 15);
            userLabel.TabIndex = 1;
            userLabel.Text = "Username";
            // 
            // loginBtn
            // 
            loginBtn.BackColor = Color.LimeGreen;
            loginBtn.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            loginBtn.ForeColor = SystemColors.ButtonHighlight;
            loginBtn.Location = new Point(338, 253);
            loginBtn.Name = "loginBtn";
            loginBtn.Size = new Size(125, 55);
            loginBtn.TabIndex = 0;
            loginBtn.Text = "Login";
            loginBtn.UseVisualStyleBackColor = false;
            loginBtn.Click += loginBtn_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(338, 174);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 23);
            textBox1.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI Semibold", 19.8000011F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Window;
            label1.Location = new Point(338, 47);
            label1.Name = "label1";
            label1.Size = new Size(98, 37);
            label1.TabIndex = 5;
            label1.Text = "LOGIN";
            // 
            // password
            // 
            password.Location = new Point(338, 218);
            password.Name = "password";
            password.Size = new Size(125, 23);
            password.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.DimGray;
            label2.Location = new Point(338, 200);
            label2.Name = "label2";
            label2.Size = new Size(59, 15);
            label2.TabIndex = 7;
            label2.Text = "Password";
            // 
            // Login
            // 
            BackgroundImage = Properties.Resources.login;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(password);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(userLabel);
            Controls.Add(loginBtn);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Name = "Login";
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label userLabel;
        private Button loginBtn;
        private TextBox textBox1;
        private Label label1;
        private TextBox password;
        private Label label2;
    }
}

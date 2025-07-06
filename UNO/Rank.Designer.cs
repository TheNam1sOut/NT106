namespace UNO
{
    partial class Rank
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
            dataGridViewRanking = new DataGridView();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewRanking).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewRanking
            // 
            dataGridViewRanking.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewRanking.Location = new Point(12, 12);
            dataGridViewRanking.Name = "dataGridViewRanking";
            dataGridViewRanking.ReadOnly = true;
            dataGridViewRanking.Size = new Size(717, 361);
            dataGridViewRanking.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(12, 379);
            button1.Name = "button1";
            button1.Size = new Size(109, 59);
            button1.TabIndex = 1;
            button1.Text = "Menu";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Rank
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(dataGridViewRanking);
            Name = "Rank";
            Text = "Rank";
            Load += Rank_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewRanking).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewRanking;
        private Button button1;
    }
}
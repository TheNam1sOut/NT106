using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.IO;

namespace UNO
{
    public partial class Rank : Form
    {

        private string username;

        public Rank(string playerName)
        {
            InitializeComponent();
            username = playerName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            Menu menuForm = new Menu(username);
            menuForm.Show();
        }

        private async void Rank_Load(object sender, EventArgs e)
        {
            await LoadRankingData();
        }
        private async Task LoadRankingData()
        {
            await NetworkManager.Instance.SendAsync("GetRanking");

            void Handler(string message)
            {
                if (message.StartsWith("Ranking:"))
                {
                    NetworkManager.Instance.MessageReceived -= Handler;
                    string rankingData = message.Substring("Ranking:".Length).Trim();
                    this.Invoke((Action)(() => DisplayRanking(rankingData)));
                }
                else if (message.StartsWith("Error:"))
                {
                    NetworkManager.Instance.MessageReceived -= Handler;
                    this.Invoke((Action)(() =>
                        MessageBox.Show("Không thể tải dữ liệu bảng xếp hạng từ server.")
                    ));
                }
            }
            NetworkManager.Instance.MessageReceived += Handler;
        }
        private void DisplayRanking(string rankingData)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Tên người chơi", typeof(string));
            dt.Columns.Add("Điểm", typeof(long)); // Điểm có thể là số nguyên lớn

            if (!string.IsNullOrEmpty(rankingData))
            {
                string[] players = rankingData.Split(',');
                int stt = 1;
                foreach (string playerEntry in players)
                {
                    string[] parts = playerEntry.Split('|');
                    if (parts.Length == 2)
                    {
                        string playerName = parts[0].Trim();
                        if (long.TryParse(parts[1].Trim(), out long points))
                        {
                            dt.Rows.Add(stt++, playerName, points);
                        }
                    }
                }
            }

            dataGridViewRanking.DataSource = dt;
            // Tùy chỉnh DataGridView (ví dụ: chỉ đọc, tự động điều chỉnh cột)
            dataGridViewRanking.ReadOnly = true;
            dataGridViewRanking.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}

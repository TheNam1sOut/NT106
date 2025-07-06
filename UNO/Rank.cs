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

namespace UNO
{
    public partial class Rank : Form
    {

        private string? username;
        private TcpClient? tcpPlayer;
        public Rank(string playerName, TcpClient playerSocket)
        {
            InitializeComponent();
            username = playerName;
            tcpPlayer = playerSocket;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Menu menuForm = new Menu(username,tcpPlayer);
            menuForm.Show();
        }

        private async void Rank_Load(object sender, EventArgs e)
        {
            await LoadRankingData();
        }
        private async Task LoadRankingData()
        {
            try
            {
                // Gửi yêu cầu đến server để lấy dữ liệu bảng xếp hạng
                NetworkStream stream = tcpPlayer.GetStream();
                string requestMessage = "GetRanking\n"; // Tạo một tin nhắn yêu cầu mới
                byte[] buffer = Encoding.UTF8.GetBytes(requestMessage);
                await stream.WriteAsync(buffer, 0, buffer.Length);

                // Đọc phản hồi từ server
                // Server sẽ gửi về dữ liệu bảng xếp hạng, ví dụ: "Ranking: Player1|100,Player2|90,..."
                byte[] responseBuffer = new byte[4096]; // Kích thước buffer lớn hơn để chứa nhiều dữ liệu
                int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
                string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead).Trim();

                if (response.StartsWith("Ranking:"))
                {
                    string rankingData = response.Substring("Ranking:".Length).Trim();
                    DisplayRanking(rankingData);
                }
                else
                {
                    MessageBox.Show("Không thể tải dữ liệu bảng xếp hạng từ server.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải bảng xếp hạng: {ex.Message}");
            }
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

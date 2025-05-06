using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UNO
{
    public partial class Login : Form
    {
        public TcpClient player;
        public Login()
        {
            InitializeComponent();
        }

        private async Task ConnectServer()
        {
            try
            {
                player = new TcpClient();
                IPAddress serverIP = IPAddress.Parse("127.0.0.1"); //server IP
                player.Connect(serverIP, 10000);

                //sau khi đã kết nối thì lấy stream để thông báo cho server rằng người dùng đã kết nối
                NetworkStream stream = player.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes($"Player: {textBox1.Text.Trim()}");
                stream.Write(buffer, 0, buffer.Length);

                //đóng form đăng ký và mở form menu
                //this.Hide();
                Menu Form1 = new Menu(textBox1.Text.Trim(), player);
                Form1.Show();
                //this.Close();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error: " +  ex.Message);
            }
        }

        private async void loginBtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                return;
            }

            //trước hết là xử lí xem đã có người dùng hay chưa, mà chưa có CSDL nên để sau
            await ConnectServer();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            player?.Close();
            base.OnFormClosing(e);
        }
    }
}

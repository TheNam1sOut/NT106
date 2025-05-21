using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
                byte[] buffer = Encoding.UTF8.GetBytes($"Player: {textBox1.Text.Trim()}|{password.Text.Trim()}\n");
                stream.Write(buffer, 0, buffer.Length);
               



                // Đọc phản hồi từ server
                using var reader = new StreamReader(stream, Encoding.UTF8,leaveOpen:true);
                string response = await reader.ReadLineAsync();
                if (response.StartsWith("LoginOK: "))
                {
                  
                    // Mở form
                    this.Hide();
                    Menu Form1 = new Menu(textBox1.Text.Trim(), player);
                    Form1.Show();
                }

                else
                if (response.StartsWith("LoginFail: "))
                {
                    string reason = response.Substring("LoginFail: ".Length);
                    string message = reason switch
                    {
                        "WrongPassword" => "Mật khẩu không đúng.",
                        "InvalidFormat" => "Thông tin đăng nhập không hợp lệ.",
                        "AlreadyOnline" => "Tài khoản đang được sử dụng.",
                        _ => "Đăng nhập thất bại."
                    };

                    MessageBox.Show(message, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                

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

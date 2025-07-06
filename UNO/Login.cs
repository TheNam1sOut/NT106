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
            this.FormClosing += Login_FormClosing;
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


                        "WrongPassword" => "Mật khẩu không đúng.", // Giữ lại nếu bạn có cách khác để xử lí mật khẩu
                        "AlreadyOnline" => "Tài khoản đang được sử dụng.",
                        "UserAlreadyExists" => "Tài khoản đã tồn tại. Vui lòng đăng nhập hoặc dùng email/tên người dùng khác.",
                        "RegistrationFailed" => "Đăng ký thất bại. Vui lòng thử lại.",
                        "DatabaseSaveError" => "Lỗi lưu dữ liệu. Vui lòng thử lại.",
                        "InvalidFormat" => "Lỗi định dạng yêu cầu.",
                        "MissingCredentials" => "Vui lòng nhập đầy đủ tên người dùng và mật khẩu.",
                        "InternalServerError" => "Lỗi server nội bộ. Vui lòng thử lại sau.",
                        "MessageParseError" => "Lỗi xử lý tin nhắn từ server.",
                        _ => "Đăng nhập thất bại không xác định."
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

        private async void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Console.WriteLine("[DEBUG] Login form is closing, handling disconnection...");
                
                // Send disconnect message to server if connected
                if (player != null && player.Connected)
                {
                    NetworkStream stream = player.GetStream();
                    string disconnectMessage = $"Disconnect: {textBox1.Text.Trim()}\n";
                    byte[] buffer = Encoding.UTF8.GetBytes(disconnectMessage);
                    stream.Write(buffer, 0, buffer.Length);
                    
                    // Close the connection
                    stream.Close();
                    player.Close();
                }
                
                Console.WriteLine("[DEBUG] Login form disconnection handled successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error during login form closing: {ex.Message}");
            }
        }
    }
}

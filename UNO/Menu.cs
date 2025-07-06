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

namespace UNO
{
    public partial class Menu : Form
    {
        private string? username;
        private TcpClient? tcpPlayer;
        public Menu()
        {
            InitializeComponent();
        }

        public Menu(string playerName, TcpClient playerSocket)
        {
            InitializeComponent();
            username = playerName;
            tcpPlayer = playerSocket;
            
            // Add form closing event handler
            this.FormClosing += Menu_FormClosing;
            
            // Add click event handler for Thoát button
            button3.Click += ThoatButton_Click;
        }

        //vào một phòng mới với nút này
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Gửi server yêu cầu vào phòng với format: "Play now: {username}"
                NetworkStream stream = tcpPlayer.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes($"Play now: {username}");
                stream.Write(buffer, 0, buffer.Length);

                //đợi server thêm vào phòng mới để vào arena
                while (tcpPlayer.Connected)
                {
                    if (tcpPlayer == null || !tcpPlayer.Connected)
                    {
                        MessageBox.Show("Not connected to server. Please reconnect.");
                        return;
                    }

                    byte[] bufferReceive = new byte[1024];
                    int byteCount = stream.Read(bufferReceive, 0, buffer.Length);
                    if (byteCount > 0)
                    {
                        string message = Encoding.UTF8.GetString(bufferReceive);
                        if (message.StartsWith("Room: "))
                        {
                            MessageBox.Show($"Success added to new room: {message.Substring(6).Trim()}");
                            Arena form1 = new Arena(username, tcpPlayer, message.Substring(6).Trim());
                            this.Hide();
                            form1.Show();                        
                            break;
                        }
                    }
                }
                //this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }  
        }

        private async void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Console.WriteLine("[DEBUG] Menu form is closing, handling disconnection...");
                
                // Send disconnect message to server
                if (tcpPlayer != null && tcpPlayer.Connected)
                {
                    NetworkStream stream = tcpPlayer.GetStream();
                    string disconnectMessage = $"Disconnect: {username}\n";
                    byte[] buffer = Encoding.UTF8.GetBytes(disconnectMessage);
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                    
                    // Close the connection
                    stream.Close();
                    tcpPlayer.Close();
                }
                
                Console.WriteLine("[DEBUG] Menu form disconnection handled successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error during menu form closing: {ex.Message}");
            }
        }

        private async void ThoatButton_Click(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("[DEBUG] Thoát button clicked, handling disconnection...");
                
                // Send disconnect message to server
                if (tcpPlayer != null && tcpPlayer.Connected)
                {
                    NetworkStream stream = tcpPlayer.GetStream();
                    string disconnectMessage = $"Disconnect: {username}\n";
                    byte[] buffer = Encoding.UTF8.GetBytes(disconnectMessage);
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                    
                    // Close the connection
                    stream.Close();
                    tcpPlayer.Close();
                }
                
                Console.WriteLine("[DEBUG] Thoát button disconnection handled successfully");
                
                // Close the form
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error during Thoát button click: {ex.Message}");
                MessageBox.Show($"Error during logout: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

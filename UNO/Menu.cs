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
        private string username;

        public Menu(string playerName)
        {
            InitializeComponent();
            username = playerName;

            this.FormClosing += Menu_FormClosing;
            button3.Click += ThoatButton_Click;
        }

        // "Chơi ngay" button
        private async void button1_Click(object sender, EventArgs e)
        {
            await NetworkManager.Instance.SendAsync($"Play now: {username}");

            void Handler(string message)
            {
                // Console.WriteLine("[DEBUG] Menu received: " + message);
                // MessageBox.Show("Received from server: " + message); // For debugging
                if (message.StartsWith("Room: "))
                {
                    NetworkManager.Instance.MessageReceived -= Handler;
                    this.Invoke((Action)(() =>
                    {
                        MessageBox.Show($"Success added to new room: {message.Substring(6).Trim()}");
                        Arena form1 = new Arena(username, message.Substring(6).Trim());
                        this.Hide();
                        form1.Show();
                    }));
                }
            }
            NetworkManager.Instance.MessageReceived += Handler;
        }

        private async void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                await NetworkManager.Instance.SendAsync($"Disconnect: {username}");
                NetworkManager.Instance.Disconnect();
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
                await NetworkManager.Instance.SendAsync($"Disconnect: {username}");
                NetworkManager.Instance.Disconnect();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // BXH button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Rank rank = new Rank(username);
            rank.Show();
        }
    }
}

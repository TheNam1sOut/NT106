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
        string username;
        TcpClient tcpPlayer;
        public Menu()
        {
            InitializeComponent();
        }

        public Menu(string playerName, TcpClient playerSocket)
        {
            InitializeComponent();
            username = playerName;
            tcpPlayer = playerSocket;
        }

        //vào một phòng mới với nút này
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                NetworkStream stream = tcpPlayer.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes($"Play now: {username}");
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }  
        }
    }
}

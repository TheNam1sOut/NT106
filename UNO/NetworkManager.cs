using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UNO
{
    public class NetworkManager
    {
        private static NetworkManager _instance;
        public static NetworkManager Instance => _instance ??= new NetworkManager();

        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private CancellationTokenSource _cts;

        // Event for incoming messages
        public event Action<string> MessageReceived;

        private NetworkManager() { }

        public async Task ConnectAsync(string host, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(host, port);
            _stream = _client.GetStream();
            _reader = new StreamReader(_stream, Encoding.UTF8, false, 4096, true);
            _writer = new StreamWriter(_stream, Encoding.UTF8, 4096, true) { AutoFlush = true };
            _cts = new CancellationTokenSource();
            _ = Task.Run(() => Listen(_cts.Token));
        }

        public async Task SendAsync(string message)
        {
            if (_writer != null)
                await _writer.WriteLineAsync(message);
        }

        private async Task Listen(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var line = await _reader.ReadLineAsync();
                    if (line == null) break; // Disconnected
                    MessageReceived?.Invoke(line);
                }
            }
            catch (Exception ex)
            {
                // Optionally log or handle error
            }
        }

        public void Disconnect()
        {
            _cts?.Cancel();
            _stream?.Close();
            _client?.Close();
        }
    }
}

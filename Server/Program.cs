// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;

Server server = new Server();
server.StartServer();
Console.WriteLine("Server is running!");

public class Server
{
    private Thread serverThread;
    private Socket serverSocket;
    private bool isRunning = false;
    public Server()
    {

    }

    public void ServerThread()
    {   
        //lần lượt khởi tạo IPEndPoint và socket, và bind socket này với IPEndPoint của server
        //để có thể tạo ra một TCPListener
        IPAddress serverIP = IPAddress.Parse("127.0.0.1");
        IPEndPoint serverIPEP = new IPEndPoint(serverIP, 10000);

        serverSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp
        );

        serverSocket.Bind(serverIPEP);
        serverSocket.Listen(10); //cho phép tối đa 10 yêu cầu kết nối một lúc
        isRunning = true;

        //xử lí yêu cầu client tại đây
        while (isRunning)
        {

        }
    }

    public void StartServer()
    {
        serverThread = new Thread(ServerThread);
        serverThread.Start();
    }
}
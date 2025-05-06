// See https://aka.ms/new-console-template for more information
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

Server server = new Server();
server.StartServer();
Console.WriteLine("Server is running!");

public class Server
{
    private Thread serverThread;
    private Socket serverSocket;
    private bool isRunning = false;
    private int byteRecv = 0;
    private byte[] buffer = new byte[1024];

    //lưu trữ danh sách phòng để chứa người chơi
    List<Room> roomList = new List<Room>();

    public Server()
    {

    }

    private void HandleClient(Socket acceptedClient)
    {
        string username = "";
        while (acceptedClient.Connected)
        {
            try
            {
                byteRecv = acceptedClient.Receive(buffer);
                if (byteRecv == 0)
                {
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, byteRecv);

                if (message == null) break;

                //xử lí yêu cầu đăng nhập của người chơi
                if (message.StartsWith("Player: "))
                {
                    username = message.Substring(8).Trim();
                    Console.WriteLine($"Player {username} has connected!");
                }
                //xử lí yêu cầu vào phòng chơi của người chơi
                else if (message.StartsWith("Play now: "))
                {
                    username = message.Substring(10).Trim();

                    //chủ yếu kiểm tra xem người dùng đã vào được phòng hay chưa
                    string checkRoomInfo = string.Empty;

                    //nó tương tự như này: for (int i = 0; i < roomList.Count; i++)
                    foreach (Room room in roomList)
                    {
                        //tìm thấy phòng trống thông tin người chơi đầu
                        if (room.player[1].Item1 == string.Empty)
                        {
                            room.player[1] = (username, acceptedClient);
                            checkRoomInfo = $"Player {username} has joined in room {room.id}";
                            Console.WriteLine(checkRoomInfo);

                            //cuối cùng, gửi id phòng đến người chơi để client có thể tiến hành thay đổi
                            string sendPlayRequest = $"Room: {room.id}";
                            byte[] sendClient = Encoding.UTF8.GetBytes(sendPlayRequest);
                            acceptedClient.Send(sendClient);

                            break;
                        }
                        //tìm thấy phòng trống thông tin người chơi thứ hai
                        else if (room.player[2].Item1 == string.Empty)
                        {
                            room.player[2] = (username, acceptedClient);
                            checkRoomInfo = $"Player {username} has joined in room {room.id}";
                            Console.WriteLine(checkRoomInfo);

                            //cuối cùng, gửi id phòng đến người chơi để client có thể tiến hành thay đổi
                            string sendPlayRequest = $"Room: {room.id}";
                            byte[] sendClient = Encoding.UTF8.GetBytes(sendPlayRequest);
                            acceptedClient.Send(sendClient);

                            break;
                        }
                    }
                    //nếu chưa tìm thấy phòng, tạo phòng mới cho người chơi
                    if (checkRoomInfo.Length <= 0)
                    {
                        int roomCount = roomList.Count; //id phòng mới
                        Room newRoom = new Room(roomCount);
                        newRoom.player[1] = (username, acceptedClient);

                        roomList.Add(newRoom);
                        checkRoomInfo = $"Player {username} has joined in room {newRoom.id}";
                        Console.WriteLine(checkRoomInfo);

                        //cuối cùng, gửi id phòng đến người chơi để client có thể tiến hành thay đổi
                        string sendPlayRequest = $"Room: {newRoom.id}";
                        byte[] sendClient = Encoding.UTF8.GetBytes(sendPlayRequest);
                        acceptedClient.Send(sendClient);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        Console.WriteLine($"{username} has disconnected!");
    }

    private class Room
    {
        //thêm constructor tùy chỉnh sau
        public Room(int id)
        {
            this.id = id;
            player.Add(1, (string.Empty, null));
            player.Add(2, (string.Empty, null));
        }

        public int id;

        //một hashmap lưu trữ người chơi và socket mà họ dùng để kết nối tới server
        //<int, (string, Socket)>: 
        //int: id người chơi trong phòng đó (1 hoặc 2)
        //string: tên người chơi
        //Socket: socket người chơi sử dụng để kết nối tới server
        public Dictionary<int, (string, Socket)> player = new Dictionary<int, (string, Socket)>(); 
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
            try
            {
                Socket acceptedClient = serverSocket.Accept();

                //tạo một luồng mới cho từng client được kết nối
                //nếu không thì server không thể giao tiếp với nhiều hơn 1 client
                Thread clientThread = new Thread(() => HandleClient(acceptedClient));
                clientThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public void StartServer()
    {
        serverThread = new Thread(ServerThread);
        serverThread.Start();
    }
}
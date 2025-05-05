// See https://aka.ms/new-console-template for more information
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

    //lưu trữ danh sách phòng để chứa người chơi
    List<Room> roomList = new List<Room>();

    public Server()
    {

    }

    private void HandleClient()
    {

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
        int byteRecv = 0;
        byte[] buffer = new byte[1024];

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
                string username;

                //code này sẽ ở HandleClient() sau
                while (acceptedClient.Connected)
                {
                    byteRecv = acceptedClient.Receive(buffer);
                    if (byteRecv == 0)
                    {
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer);

                    if (message == null) break;
                    if (message.StartsWith("Player: "))
                    {
                        username = message.Substring(8).Trim();
                        Console.WriteLine($"Player {username} has connected!");
                    } 
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
                                break;
                            }
                            //tìm thấy phòng trống thông tin người chơi thứ hai
                            else if (room.player[2].Item1 == string.Empty)
                            {
                                room.player[2] = (username, acceptedClient);
                                checkRoomInfo = $"Player {username} has joined in room {room.id}";
                                Console.WriteLine(checkRoomInfo);
                                break;
                            }
                        }
                        //nếu chưa tìm thấy phòng, tạo phòng mới cho người chơi
                        if (checkRoomInfo.Length <= 0)
                        {
                            int roomCount = roomList.Count; //id phòng mới
                            Room newRoom = new Room(roomCount);
                            roomList.Add(newRoom);
                        }
                    }
                }
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
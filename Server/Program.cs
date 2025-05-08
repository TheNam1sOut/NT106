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
                                room.ClientId[acceptedClient] = 1;
                                checkRoomInfo = $"Player {username} has joined in room {room.id}";
                                Console.WriteLine(checkRoomInfo);

                                //cuối cùng, gửi id phòng đến người chơi để client có thể tiến hành thay đổi
                                string sendPlayRequest = $"Room: {room.id}";
                                byte[] sendClient = Encoding.UTF8.GetBytes(sendPlayRequest);
                                acceptedClient.Send(sendClient);
                            string status = $"{room.countrd[1]},{room.countrd[2]}";
                            byte[] init = Encoding.UTF8.GetBytes($"isPlay: {status}");
                            acceptedClient.Send(init, 0, init.Length, SocketFlags.None);
                            break;
                            }
                            //tìm thấy phòng trống thông tin người chơi thứ hai
                            else if (room.player[2].Item1 == string.Empty)
                            {
                                room.player[2] = (username, acceptedClient);
                                room.ClientId[acceptedClient] = 2;
                                checkRoomInfo = $"Player {username} has joined in room {room.id}";
                                Console.WriteLine(checkRoomInfo);

                                //cuối cùng, gửi id phòng đến người chơi để client có thể tiến hành thay đổi
                                string sendPlayRequest = $"Room: {room.id}";
                                byte[] sendClient = Encoding.UTF8.GetBytes(sendPlayRequest);
                                acceptedClient.Send(sendClient);
                            string status = $"{room.countrd[1]},{room.countrd[2]}";
                            byte[] init = Encoding.UTF8.GetBytes($"isPlay: {status}");
                            acceptedClient.Send(init, 0, init.Length, SocketFlags.None);
                            break;
                            }
         
                        }
                        //nếu chưa tìm thấy phòng, tạo phòng mới cho người chơi
                        if (checkRoomInfo.Length <= 0)
                        {
                            int roomCount = roomList.Count; //id phòng mới
                            Room newRoom = new Room(roomCount);
                            newRoom.player[1] = (username, acceptedClient);
                            newRoom.ClientId[acceptedClient] = 1;
                            roomList.Add(newRoom);
                            checkRoomInfo = $"Player {username} has joined in room {newRoom.id}";
                            Console.WriteLine(checkRoomInfo);

                            //cuối cùng, gửi id phòng đến người chơi để client có thể tiến hành thay đổi
                            string sendPlayRequest = $"Room: {newRoom.id}";
                            byte[] sendClient = Encoding.UTF8.GetBytes(sendPlayRequest);
                            acceptedClient.Send(sendClient);
                        }
                    }
                    // xử lý khi người chơi nhấn ready;
                    else if(message.StartsWith("Ready:"))
                    {
                        Room room = FindRoombyClientID(acceptedClient);
                        if (room!=null)
                        {
                            int ID = room.ClientId[acceptedClient];
                            room.countrd[ID] = 1;
                            Console.WriteLine(room.countrd[ID]);
                            room.sumcountrd = room.countrd[1]+room.countrd[2];
                            Console.WriteLine($"Player {username} has ready in room {room.id}");
                            string status = $"{room.countrd[1]},{room.countrd[2]}";
                            string messageToClients = $"isPlay: {status}";
                            Console.WriteLine(messageToClients);
                            byte[] data = Encoding.UTF8.GetBytes(messageToClients);
                            foreach (var sock in room.ClientId.Keys)
                            {
                                sock.Send(data, 0, data.Length, SocketFlags.None);
                            }
                        }
                        Console.WriteLine("ready");
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
                countrd = new int[3];
            }

            public int id;
            // mảng lưu trữ thông tin người chơi trong phòng đã nhấn ready hay chưa
            public int[] countrd;
            public int sumcountrd = 0; // số người chơi trong phòng
            // 
            public Dictionary<Socket,int> ClientId= new Dictionary<Socket, int>();
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
        // hàm tìm client theo socket
        private Room FindRoombyClientID(Socket ClientSocket)
        {
            return roomList.FirstOrDefault(room=>room.ClientId.ContainsKey(ClientSocket));
        }
        public void StartServer()
        {
            serverThread = new Thread(ServerThread);
            serverThread.Start();
        }
    }
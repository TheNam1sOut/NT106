using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

Server server = new Server();
server.StartServer();
Console.WriteLine("Server is running!");

public class Server
{
    //khởi tạo các biến cần thiết cho server
    private Thread serverThread;
    private Socket serverSocket;
    private bool isRunning = false;
    private int byteRecv = 0;
    private byte[] buffer = new byte[1024];

    //lưu trữ danh sách phòng để chứa người chơi
    List<Room> roomList = new List<Room>();
    private int Opponent(int playerId) => playerId == 1 ? 2 : 1;

    private void RefillDrawPile(Room room)
    {
        if (room.Dataqueue1.Count <= 1) return;  // only keep top
        var top = room.Dataqueue1.Dequeue();
        // move rest to draw pile
        while (room.Dataqueue1.Count > 0)
            room.Dataqueue.Enqueue(room.Dataqueue1.Dequeue());
        ShuffleQueue(room.Dataqueue);
        room.Dataqueue1.Enqueue(top);
        // broadcast new top
        SenUnoCardTop(room, "");
    }
    /// <summary>
    /// Hoán vị ngẫu nhiên các phần tử trong queue.
    /// </summary>
    private void ShuffleQueue<T>(Queue<T> queue)
    {
        // Chuyển queue thành list để dễ hoán vị
        var list = queue.ToList();
        queue.Clear();  // Xóa hết phần tử cũ

        var rnd = new Random();
        // Lấy ngẫu nhiên từng phần tử từ list rồi enqueue trở lại
        while (list.Count > 0)
        {
            int idx = rnd.Next(list.Count);
            queue.Enqueue(list[idx]);
            list.RemoveAt(idx);
        }
    }
    private void SenUnoCardTop(Room room, string unused)
    {
        if (room.Dataqueue1.Count == 0) return;
        var top = room.Dataqueue1.Peek();
        Console.WriteLine($"[DEBUG][CardTop] Card: {top}, Màu hiện tại: {room.pendingWildColor}");
        foreach (var sock in room.ClientId.Keys)
            sock.Send(Encoding.UTF8.GetBytes($"CardTop: {top}|{room.pendingWildColor}\n"));
    }

    public Server()
    {

    }

    //private void SendInitialQueues(Room room)
    //{
    //    // saves dataqueue and dataqueue1 as strings
    //    string drawPile = string.Join(",", room.Dataqueue);
    //    string discardPile = string.Join(",", room.Dataqueue1);

    //    //wraps them inside a message to send to the clients
    //    string message = $"Dataqueue: {drawPile}|{discardPile}\n";
    //    Console.WriteLine(message.Length);
    //    foreach (var sock in room.ClientId.Keys)
    //    {
    //        try
    //        {
    //            byte[] data = Encoding.UTF8.GetBytes(message);
    //            Console.WriteLine(message.Length);
    //            sock.Send(data, 0, data.Length, SocketFlags.None);
    //        }
    //        catch (SocketException ex)
    //        {
    //            Console.WriteLine($"Error sending queues to client: {ex.Message}");
    //        }
    //    }
    //}

    //as what the function says, when the room is first initialized, we send the clients the first cards
    private void SendInitialHand(Room room)
    {
        try
        {
            if (room.Dataqueue.Count > 0)
            {
                string topCard;
                do
                {
                    topCard = room.Dataqueue.Dequeue();
                    if (topCard == "DD" || topCard == "DP") room.Dataqueue.Enqueue(topCard);
                } while (topCard == "DD" || topCard == "DP");
                room.Dataqueue1.Enqueue(topCard);
                room.currentValue = topCard.Substring(1);
                room.pendingWildColor = topCard[0];
            }

            foreach (var sock in room.ClientId.Keys)
            {
                int playerId = room.ClientId[sock];
                List<string> hand = new List<string>();
                for (int i = 0; i < 6; i++)
                    hand.Add(room.Dataqueue.Dequeue());
                if (!room.playerHands.ContainsKey(playerId))
                    room.playerHands.Add(playerId, hand);
                else room.playerHands[playerId] = hand;
                string handMsg = "InitialHand: " + string.Join(",", hand) + "\n";
                var buf = Encoding.UTF8.GetBytes(handMsg);
                sock.Send(buf);
            }
            SenUnoCardTop(room, "");
            Broadcast(room, $"PendingDraw: 0\n");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private bool HasPlayableCards(Room room, int playerId)
    {
        if (!room.playerHands.ContainsKey(playerId)) return false;
        foreach (string card in room.playerHands[playerId])
        {
            if (IsPlayable(card, room))
                return true;
        }
        return false;
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
                            byte[] init = Encoding.UTF8.GetBytes($"isPlay: {status}\n");
                            acceptedClient.Send(init, 0, init.Length, SocketFlags.None);
                            string sendIdMessage = $"YourId: 1\n"; // Gửi ID cho player 1
                            acceptedClient.Send(Encoding.UTF8.GetBytes(sendIdMessage));
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
                            byte[] init = Encoding.UTF8.GetBytes($"isPlay: {status}\n");
                            acceptedClient.Send(init, 0, init.Length, SocketFlags.None);
                            string sendIdMessage = $"YourId: 2\n"; // Gửi ID cho player 2
                            acceptedClient.Send(Encoding.UTF8.GetBytes(sendIdMessage));
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
                        string sendIdMessage = $"YourId: {newRoom.ClientId[acceptedClient]}\n";
                        byte[] idData = Encoding.UTF8.GetBytes(sendIdMessage);
                        acceptedClient.Send(idData);
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
                else if (message.StartsWith("Ready:"))
                {
                    Room room = FindRoombyClientID(acceptedClient);
                    if (room != null)
                    {
                        int ID = room.ClientId[acceptedClient];
                        room.countrd[ID] = 1;
                        Console.WriteLine(room.countrd[ID]);
                        room.sumcountrd = room.countrd[1] + room.countrd[2];
                        Console.WriteLine($"Player {username} has ready in room {room.id}");
                        string status = $"{room.countrd[1]},{room.countrd[2]}";
                        string messageToClients = $"isPlay: {status}\n";
                        Console.WriteLine(messageToClients);
                        byte[] data = Encoding.UTF8.GetBytes(messageToClients);
                        foreach (var sock in room.ClientId.Keys)
                        {
                            sock.Send(data, 0, data.Length, SocketFlags.None);
                        }

                        if (room.rommbg == 0 && room.sumcountrd == 2)
                        {
                            Console.WriteLine(room.rommbg + room.sumcountrd);
                            room.rommbg = 1;
                            foreach (var sock in room.ClientId.Keys)
                            {
                                int id = room.ClientId[sock];
                                sock.Send(Encoding.UTF8.GetBytes($"YourId: {id}\n"));
                            }

                            Broadcast(room, $"Turn: {room.currentTurn}\n");
                            Broadcast(room, $"PendingDraw: {room.pendingDrawCards}\n");
                            SenUnoCardTop(room, "");
                            SendInitialHand(room);


                        }
                    }
                    Console.WriteLine("ready");
                }
                else if (message.StartsWith("PlayCard: "))
                {
                    Room room = FindRoombyClientID(acceptedClient);
                    int playerId = room.ClientId[acceptedClient];
                    var parts = message.Substring(10).Split('|');
                    string card = parts[0];
                    char color = parts.Length > 1 ? parts[1][0] : room.pendingWildColor;
                    // enqueue discard
                    room.Dataqueue1.Enqueue(card);
                    // update wild color & currentValue
                    room.pendingWildColor = (card == "DD" || card == "DP") ? color : card[0];
                    room.currentValue = card.Length > 1 ? card.Substring(1) : card;
                    // process +2/+4
                    if (card == "DP") { room.pendingDrawCards += 4; }
                    else if (card.EndsWith("P")) { room.pendingDrawCards += 2; }
                    else room.pendingDrawCards = 0;

                    // next turn (skip on skip/reverse managed similarly)
                    room.currentTurn = Opponent(playerId);

                    // send new top, turn, pending draw
                    Console.WriteLine($"[DEBUG][PlayCard] - Giá trị hiện tại: {room.currentValue}");
                    Console.WriteLine($"[DEBUG][PlayCard] - Lượt tiếp theo: {room.currentTurn}");
                    Console.WriteLine($"[DEBUG][PlayCard] - Màu hiện tại: {room.pendingWildColor}");
                    Console.WriteLine($"[DEBUG][PlayCard] - Số card pen : {room.pendingDrawCards}");
                    Broadcast(room, $"CardTop: {card}|{room.pendingWildColor}\n");
                    Broadcast(room, $"Turn: {room.currentTurn}\n");
                    Broadcast(room, $"PendingDraw: {room.pendingDrawCards}\n");
                }
                else if (message.StartsWith("DrawCard"))
                {
                    var room = FindRoombyClientID(acceptedClient);
                    int playerId = room.ClientId[acceptedClient];


                    if (room.Dataqueue.Count == 0) RefillDrawPile(room);

                    var drawnCard = room.Dataqueue.Dequeue();
                    room.playerHands[playerId].Add(drawnCard);
                    acceptedClient.Send(Encoding.UTF8.GetBytes($"DrawCard: {drawnCard}\n"));
                    // Giảm số lá phạt nếu đang xử lý phạt
                    if (room.pendingDrawCards > 0)
                    {
                        room.pendingDrawCards--;
                        Broadcast(room, $"PendingDraw: {room.pendingDrawCards}\n");
                    }
                    else
                    {
                        room.pendingDrawCards = 0;
                        Broadcast(room, $"PendingDraw: 0\n");
                    }
                    // broadcast new top
                }


                //else if (message.StartsWith("New Deck: "))
                //{
                //    Room room = FindRoombyClientID(acceptedClient);

                //    string[] newDeck = message.Substring("New Deck: ".Length).Split(',');
                //    room.Dataqueue.Clear();
                //    room.Dataqueue1.Clear();
                //    foreach (string card in newDeck)
                //    {
                //        room.Dataqueue.Enqueue(card);
                //    }
                //    SendInitialQueues(room);
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        Console.WriteLine($"{username} has disconnected!");
    }

    private void Broadcast(Room room, string msg)
    {
        var data = Encoding.UTF8.GetBytes(msg);
        foreach (var sock in room.ClientId.Keys)
            sock.Send(data);
    }

    private bool IsPlayable(string card, Room room)
    {
        char cardColor = (card == "DD" || card == "DP") ? room.pendingWildColor : card[0];
        string cardValue = (card == "DD" || card == "DP") ? card : card.Substring(1);
        // Kiểm tra theo màu hiện tại hoặc giá trị
        return cardColor == room.pendingWildColor
               || cardValue == room.currentValue
               || card == "DD"
               || card == "DP";
    }

    private class Room
    {
        //thêm constructor tùy chỉnh sau
        public int currentTurn;
        public int pendingDrawCards = 0; //stack card cần rút
        public char pendingWildColor = 'W'; // Màu mặc định cho Wild
        public string currentValue; // Giá trị bài hiện tại
        public Dictionary<int, List<string>> playerHands = new Dictionary<int, List<string>>();
        public bool mustPlayColor = false;
        public char forcedColor;
        public Room(int id)
        {
            currentTurn = 1;
            this.id = id;
            player.Add(1, (string.Empty, null));
            player.Add(2, (string.Empty, null));
            countrd = new int[3];
            InitializeCardQueue();
        }
        public int rommbg; // lưu trữ thông tin phòng đã bắt đầu chơi hay chưa
        public int id;
        // mảng lưu trữ thông tin người chơi trong phòng đã nhấn ready hay chưa
        public int[] countrd;
        public int sumcountrd = 0; // số người chơi trong phòng
                                   // 
        public Dictionary<Socket, int> ClientId = new Dictionary<Socket, int>();
        //một hashmap lưu trữ người chơi và socket mà họ dùng để kết nối tới server
        //<int, (string, Socket)>: 
        //int: id người chơi trong phòng đó (1 hoặc 2)
        //string: tên người chơi
        //Socket: socket người chơi sử dụng để kết nối tới server
        public Dictionary<int, (string, Socket)> player = new Dictionary<int, (string, Socket)>();
        public Queue<string> Dataqueue = new Queue<string>(); // bo bai chua danh
        public Queue<string> Dataqueue1 = new Queue<string>();// bo bai da danh


        public void InitializeCardQueue()
        {
            string cardsDirectory = "..\\..\\..\\Resources\\UNOCards\\Uno.txt";

            // Read data from file and enqueue
            Dataqueue = ReadFileAndEnqueue(cardsDirectory);
            //bị lỗi
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uno.txt");

            // Shuffle the queue elements
            ShuffleQueue(Dataqueue);
        }
        public void ShuffleQueue(Queue<string> queue)
        {
            // Convert queue to list
            List<string> dataList = new List<string>(queue);
            // Use Fisher-Yates algorithm to shuffle list
            Random random = new Random();
            int n = dataList.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                string value = dataList[k];
                dataList[k] = dataList[n];
                dataList[n] = value;
            }
            // Clear the queue
            queue.Clear();
            // Enqueue shuffled elements back to the queue
            foreach (string element in dataList)
            {
                queue.Enqueue(element);
            }
        }
        public Queue<string> ReadFileAndEnqueue(string filePath)
        {
            Queue<string> dataQueue = new Queue<string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Remove newline character and split data into elements
                    string[] elements = line.Split(',');
                    // Enqueue elements
                    foreach (string element in elements)
                    {
                        dataQueue.Enqueue(element);
                    }
                }
            }
            return dataQueue;
        }

        // Method to shuffle queue elements


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
        return roomList.FirstOrDefault(room => room.ClientId.ContainsKey(ClientSocket));
    }

    public void StartServer()
    {
        serverThread = new Thread(ServerThread);
        serverThread.Start();
    }
}
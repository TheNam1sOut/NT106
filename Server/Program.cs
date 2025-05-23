    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using static System.Net.Mime.MediaTypeNames;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Data.SqlClient;
    using System.Collections.Generic;
using System.Collections.Concurrent;


    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())  // lấy thư mục hiện tại
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
    string connStr = config.GetConnectionString("MyDb");  // đọc chuỗi kết nối từ file
    using (var conn = new SqlConnection(connStr))
    {
        try
        {
            conn.Open();
            Console.WriteLine("✅ Kết nối SQL Server thành công!");
            string sql = "SELECT * FROM Game";
            using (var cmd = new SqlCommand(sql, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int game_id = reader.GetInt32(0);           // cột 0 = Id
                   
                        int winner_id = reader.GetInt32(1);           // cột 2 = Age

                        Console.WriteLine($"ID: {game_id}, Tuổi: {winner_id}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Lỗi kết nối: " + ex.Message);
        }
    }

    Server server = new Server();
    server.StartServer();
    Console.WriteLine("Server is running!");

    public class Server
    {
        //khởi tạo các biến cần thiết cho server
        private readonly string _connStr;
        private Thread serverThread;
        private Socket serverSocket;
        private bool isRunning = false;
        private int byteRecv = 0;
        private byte[] buffer = new byte[1024];

        //lưu trữ danh sách phòng để chứa người chơi
        List<Room> roomList = new List<Room>();
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
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            _connStr = config.GetConnectionString("MyDb");
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
                // Lấy 6 lá bài đầu
                var queue = new ConcurrentQueue<string>();
                for (int i = 0; i < 6; i++)
                {
                    string card = room.Dataqueue.Dequeue();
                    queue.Enqueue(card);
                }
                // Gán queue mới vào playerHands
                room.playerHands[playerId] = queue;

                // Gửi danh sách bài cho client (giữ thứ tự)
                string handMsg = "InitialHand: " + string.Join(",", queue) + "\n";
                sock.Send(Encoding.UTF8.GetBytes(handMsg));
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
    private bool HasCounterCard(Room room, int playerId)
    {
        // Nếu không có tay bài, trả false
        if (!room.playerHands.TryGetValue(playerId, out var queue))
            return false;

        // Lấy lá trên cùng của discard pile
        if (room.Dataqueue1.Count == 0)
            return false;
        string topCard = room.Dataqueue1.Last();

        // Nếu top là +4, chỉ cho chồng +4
        if (topCard == "DP")
        {
            // Dùng Any để check trong snapshot
            return queue.Any(c => c == "DP");
        }
        // Nếu top là +2 (endswith 'P' nhưng != DP), cho chồng +2 hoặc +4
        else if (topCard.EndsWith("P"))
        {
            return queue.Any(c => c.EndsWith("P"));
        }

        // Không phải +2/+4 => không áp dụng chồng
        return false;
    }


    private async void HandleClient(Socket acceptedClient)
        {
            int currentPlayerId = 0;
            Room rooms = null;
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
                    
                        var parts = message.Substring(8).Split('|');
                        string name = parts[0].Trim();
                        string password = parts[1].Trim();
                        Console.WriteLine($"Player {name} has connected!");

                        // kiểm tra xem người chơi đã đăng nhập hay chưa
                        using var conn = new SqlConnection(_connStr);
                        conn.Open();
                        string checkSql = "SELECT player_id, password,status FROM player WHERE username = @username";
                        using var checkCmd = new SqlCommand(checkSql, conn);
                        checkCmd.Parameters.AddWithValue("@username", name);
                        using var reader = checkCmd.ExecuteReader();

                        int playerId;
                        if (reader.Read())
                        {
                            string passwordInDb = reader.GetString(1);
                            bool isonline = reader.GetBoolean(2);
                            if (isonline)
                            {
                        
                                byte[] data = Encoding.UTF8.GetBytes("LoginFail: AlreadyOnline\n");
                                acceptedClient.Send(data);
                                continue;
                            }
                            if (password != passwordInDb)
                            {
                         
                                byte[] data = Encoding.UTF8.GetBytes("LoginFail: WrongPassword\n");
                                acceptedClient.Send(data);
                                continue;
                            }

                            playerId = reader.GetInt32(0);
                            reader.Close();
                            // Đánh dấu đang online
                            var updateCmd = new SqlCommand("UPDATE player SET status = 1 WHERE player_id = @id", conn);
                            updateCmd.Parameters.AddWithValue("@id", playerId);
                            updateCmd.ExecuteNonQuery();
                            currentPlayerId = playerId;
                        }
                        else
                        {
                            reader.Close();
                            string insertSql = @"
                            INSERT INTO player(username, password, status, point) 
                            VALUES (@username, @password, 1, 0);
                            SELECT SCOPE_IDENTITY();";
                            using var insertCmd = new SqlCommand(insertSql, conn);
                            insertCmd.Parameters.AddWithValue("@username", name);
                            insertCmd.Parameters.AddWithValue("@password", password);
                            playerId = Convert.ToInt32(insertCmd.ExecuteScalar());
                        }

                          currentPlayerId = playerId;
                        byte[] dataOk = Encoding.UTF8.GetBytes($"LoginOK: {playerId}|{name}\n");
                        acceptedClient.Send(dataOk);
                        continue;
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
                                string status = $"{room.countrd[1]},{room.countrd[2]},{room.countrd[3]},{room.countrd[4]}";
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
                                string status = $"{room.countrd[1]},{room.countrd[2]},{room.countrd[3]},{room.countrd[4]}";
                                byte[] init = Encoding.UTF8.GetBytes($"isPlay: {status}\n");
                                acceptedClient.Send(init, 0, init.Length, SocketFlags.None);
                                string sendIdMessage = $"YourId: 2\n"; // Gửi ID cho player 2
                                acceptedClient.Send(Encoding.UTF8.GetBytes(sendIdMessage));
                                break;
                            }
                            else if (room.player[3].Item1 == string.Empty)
                            {
                                room.player[3] = (username, acceptedClient);
                                room.ClientId[acceptedClient] = 3;
                                checkRoomInfo = $"Player {username} has joined in room {room.id}";
                                Console.WriteLine(checkRoomInfo);

                                //cuối cùng, gửi id phòng đến người chơi để client có thể tiến hành thay đổi
                                string sendPlayRequest = $"Room: {room.id}";
                                byte[] sendClient = Encoding.UTF8.GetBytes(sendPlayRequest);
                                acceptedClient.Send(sendClient);
                                string status = $"{room.countrd[1]},{room.countrd[2]},{room.countrd[3]},{room.countrd[4]}";
                                byte[] init = Encoding.UTF8.GetBytes($"isPlay: {status}\n");
                                acceptedClient.Send(init, 0, init.Length, SocketFlags.None);
                                string sendIdMessage = $"YourId: 3\n"; // Gửi ID cho player 3
                                acceptedClient.Send(Encoding.UTF8.GetBytes(sendIdMessage));
                                break;
                            }
                            else if (room.player[4].Item1 == string.Empty)
                            {
                                room.player[4] = (username, acceptedClient);
                                room.ClientId[acceptedClient] = 4;
                                checkRoomInfo = $"Player {username} has joined in room {room.id}";
                                Console.WriteLine(checkRoomInfo);

                                //cuối cùng, gửi id phòng đến người chơi để client có thể tiến hành thay đổi
                                string sendPlayRequest = $"Room: {room.id}";
                                byte[] sendClient = Encoding.UTF8.GetBytes(sendPlayRequest);
                                acceptedClient.Send(sendClient);
                                string status = $"{room.countrd[1]},{room.countrd[2]},{room.countrd[3]},{room.countrd[4]}";
                                byte[] init = Encoding.UTF8.GetBytes($"isPlay: {status}\n");
                                acceptedClient.Send(init, 0, init.Length, SocketFlags.None);
                                string sendIdMessage = $"YourId: 4\n"; // Gửi ID cho player 4
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
                            room.sumcountrd = room.countrd[1] + room.countrd[2] + room.countrd[3] + room.countrd[4];
                            Console.WriteLine($"Player {username} has ready in room {room.id}");
                            string status = $"{room.countrd[1]},{room.countrd[2]},{room.countrd[3]},{room.countrd[4]}";
                            string messageToClients = $"isPlay: {status}\n";
                            Console.WriteLine(messageToClients);
                            byte[] data = Encoding.UTF8.GetBytes(messageToClients);
                            foreach (var sock in room.ClientId.Keys)
                            {
                                sock.Send(data, 0, data.Length, SocketFlags.None);
                            }

                            if (room.rommbg == 0 && room.sumcountrd == 4)
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
                        Console.WriteLine($"[DEBUG] Sending message: {message}");
                        Console.WriteLine($"[DEBUG] Received raw PlayCard message: '{message.Trim()}'");
                        Room room = FindRoombyClientID(acceptedClient);
                        int playerId = room.ClientId[acceptedClient];
                        var parts = message.Substring(10).Split('|');
                        string cardRaw = parts[0].Trim().Split('|')[0];
                        string card = cardRaw;
                        Console.WriteLine($"[DEBUG] [PlayCard] Nhận từ player{playerId}: {message.Trim()}");
                        char color = parts.Length > 1 ? parts[1][0] : room.pendingWildColor;
                        // enqueue discard
                        room.Dataqueue1.Enqueue(card);
                        // update wild color & currentValue
                        room.pendingWildColor = (card == "DD" || card == "DP") ? color : card[0];
                    room.currentValue = (card == "DD" || card == "DP") ? card : card.Substring(1);

                    // process +2/+4
                    if (card == "DP")
                        {
                            // Nếu đang có pendingDrawCards (>0), dù là do +2 hay +4, DP sẽ chồng tiếp 4 lá
                            if (room.pendingDrawCards > 0)
                                room.pendingDrawCards += 4;
                            else
                                room.pendingDrawCards = 4;
                        }
                        else if (card.EndsWith("P"))
                        {
                            if (room.pendingDrawCards > 0 )
                                room.pendingDrawCards += 2;
                            else if (room.pendingDrawCards == 0)
                                room.pendingDrawCards = 2;
                        }
                        else
                        {
                            room.pendingDrawCards = 0;
                        }


                        // xu li la dao nguoc 
                        if (card == "RD" || card == "GD" || card == "BD" || card == "YD")
                        {
                            room.isReversed = !room.isReversed;
                            Console.WriteLine($"[DEBUG] Reverse status: {room.isReversed}");
                        }
                        int next = nextplayer(playerId, room.isReversed);
                        // xử lý khi gặp lá skip
                        if (card.EndsWith("C"))
                        {
                            next = nextplayer(next, room.isReversed);
                        }
                        room.currentTurn = next;
                        // send new top, turn, pending draw
                        Console.WriteLine($"[DEBUG][PlayCard] - Giá trị hiện tại: {room.currentValue}");
                        Console.WriteLine($"[DEBUG][PlayCard] - Lượt tiếp theo: {room.currentTurn}");
                        Console.WriteLine($"[DEBUG][PlayCard] - Màu hiện tại: {room.pendingWildColor}");
                        Console.WriteLine($"[DEBUG][PlayCard] - Số card pen : {room.pendingDrawCards}");
                        Console.WriteLine($"[DEBUG] card nhận được: {card}");


                        Broadcast(room, $"CardTop: {card}|{room.pendingWildColor}\n");
                        Broadcast(room, $"Turn: {room.currentTurn}\n");
                        Broadcast(room, $"PendingDraw: {room.pendingDrawCards}\n");

                    if (room.playerHands.TryGetValue(playerId, out var queue))
                    {
                        // Tạo danh sách tạm để chứa các lá bài không phải lá vừa đánh
                        var temp = new List<string>();
                        bool removed = false;

                        // Lấy từng lá từ queue gốc, bỏ lá đã đánh, và lưu lại những lá còn lại
                        while (queue.TryDequeue(out var c))
                        {
                            if (!removed && (c == card ||
                     (card == "DD" && c.StartsWith("DD")) ||
                     (card == "DP" && c.StartsWith("DP"))))
                            {
                                removed = true;
                                continue; // Bỏ qua lá đã đánh
                            }
                            temp.Add(c);
                        }

                        // Thêm lại các lá còn lại vào queue (giữ thứ tự)
                        foreach (var c in temp)
                            queue.Enqueue(c);
                    }

                    int remaining = room.playerHands[playerId].Count;
                        Broadcast(room, $"Remaining: {playerId}:{remaining}\n");

                        // Nếu người chơi còn 1 lá và chưa gọi UNO trước đó
                        if (remaining == 1 && !room.UnoCalled[playerId])
                        {
                            room.CatchOccurred = false;
                            string catchMsg = $"CatchWindow: {playerId}\n";
                            byte[] data = Encoding.UTF8.GetBytes(catchMsg);

                            Console.WriteLine($"[DEBUG] Gửi CatchWindow cho các người chơi khác (loại trừ player {playerId})");
                            foreach (var sock in room.ClientId.Keys)
                            {
                                int receiverId = room.ClientId[sock];
                                if (receiverId != playerId)
                                {
                                    Console.WriteLine($"[DEBUG] Gửi CatchWindow đến player {receiverId}");
                                    sock.Send(data);
                                }
                            }
                            Console.WriteLine($"[DEBUG] CatchWindow message đã gửi.");
                        }
                        else Console.WriteLine($"[DEBUG] Không gửi CatchWindow: playerId={playerId}, remaining={remaining}, UnoCalled={room.UnoCalled[playerId]}");


                        // Nếu người chơi đã đánh hết bài
                        if (remaining == 0)
                        {
                            // Xác định người thắng và thông báo cho tất cả client
                            string winner = room.player[playerId].Item1;
                            Broadcast(room, $"PlayerWin: {winner}\n");
                            // (Không cần xử lý catch vì game đã kết thúc)
                        }

                        // Sau khi Broadcast Turn và PendingDraw

                        if (room.pendingDrawCards > 0)
                        {
                            int nextId = room.currentTurn;
                            // Nếu người chơi kế tiếp không có lá +2/+4 phù hợp thì tự động rút bài
                            if (!HasCounterCard(room, nextId))
                            {
                                var nextSock = room.player[nextId].Item2;
                            Console.WriteLine($"[DEBUG] next={nextId}, hasCounter={HasCounterCard(room, nextId)}, pending={room.pendingDrawCards}");

                            string notify = $"AutoDrawCount: {room.pendingDrawCards}\n";
                                nextSock.Send(Encoding.UTF8.GetBytes(notify)); 
                                for (int i = 0; i < room.pendingDrawCards; i++)
                                {
                                    if (room.Dataqueue.Count == 0) RefillDrawPile(room);
                                    string newCard = room.Dataqueue.Dequeue();
                                room.playerHands[nextId].Enqueue(newCard);

                                Socket sock = room.player[nextId].Item2;
                                    sock.Send(Encoding.UTF8.GetBytes($"DrawCard: {newCard}\n"));
                                    Task.Delay(200).Wait();
                                }
                                // Reset lại PendingDraw về 0 và thông báo cho tất cả client
                                room.pendingDrawCards = 0;
                                Broadcast(room, $"PendingDraw: 0\n");
                            }
                        }

                    }
                    else if (message.StartsWith("DrawCard"))
                    {
                        var room = FindRoombyClientID(acceptedClient);
                        if (room.pendingDrawCards > 0)
                        {
                            continue;
                        }
                        {
                            int playerId = room.ClientId[acceptedClient];


                            if (room.Dataqueue.Count == 0) RefillDrawPile(room);

                            var drawnCard = room.Dataqueue.Dequeue();
                        room.playerHands[playerId].Enqueue(drawnCard);

                        acceptedClient.Send(Encoding.UTF8.GetBytes($"DrawCard: {drawnCard}\n"));
                        }
                   

                        // broadcast new top
                    }
                    else if (message.StartsWith("UnoCall: "))
                    {
                        Room room = FindRoombyClientID(acceptedClient);
                        int playerId = room.ClientId[acceptedClient];
                        room.UnoCalled[playerId] = true;
                        Console.WriteLine($"[DEBUG] Player {playerId} gọi UNO.");
                        continue;
                    }
                    else if (message.StartsWith("CatchUno: "))
                    {
                        Room room = FindRoombyClientID(acceptedClient);
                        int catcher = room.ClientId[acceptedClient];
                        int targetId = int.Parse(message.Substring("CatchUno: ".Length).Trim());
                        // Nếu mục tiêu chưa gọi UNO và chưa ai bắt được:
                        if (!room.UnoCalled[targetId] && !room.CatchOccurred)
                        {
                            room.CatchOccurred = true;
                            // Phạt target rút thêm 2 lá
                            for (int i = 0; i < 2; i++)
                            {
                                if (room.Dataqueue.Count == 0) RefillDrawPile(room);
                                string newCard = room.Dataqueue.Dequeue();
                            room.playerHands[targetId].Enqueue(newCard);

                            var targetSocket = room.player[targetId].Item2;
                                targetSocket.Send(Encoding.UTF8.GetBytes($"DrawCard: {newCard}\n"));
                            }
                        }
                        Console.WriteLine($"[DEBUG] Player {catcher} bắt lỗi player {targetId}");
                        continue;
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
            Console.WriteLine($"{currentPlayerId} >0");
            // xử lí khi người chơi thoát
            if (currentPlayerId>0)
            {
                using var conn=new SqlConnection(_connStr);
                conn.Open();
                string updateSql = "UPDATE player SET status = 0 WHERE player_id = @id";
                using var updateCmd = new SqlCommand(updateSql, conn);
                updateCmd.Parameters.AddWithValue("@id", currentPlayerId);
                updateCmd.ExecuteNonQuery();
            }
            acceptedClient.Close();
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
        public ConcurrentDictionary<int, ConcurrentQueue<string>> playerHands
            = new ConcurrentDictionary<int, ConcurrentQueue<string>>();
        public Dictionary<int, string> PlayerNames { get; } = new Dictionary<int, string>();
            public bool mustPlayColor = false;
            public char forcedColor;
            public Dictionary<int, bool> UnoCalled = new Dictionary<int, bool>();
            public bool CatchOccurred = false;
            public bool isReversed { get; set; } = false; 
            public Room(int id)
            {
                currentTurn = 1;
                this.id = id;
                player.Add(1, (string.Empty, null));
                player.Add(2, (string.Empty, null));
                player.Add(3, (string.Empty, null));
                player.Add(4, (string.Empty, null));
            for (int i = 1; i <= 4; i++)
                playerHands.TryAdd(i, new ConcurrentQueue<string>());
            // Khởi tạo UnoCalled cho các player 1–4
            for (int i = 1; i <= 4; i++)
                    UnoCalled[i] = false;

                countrd = new int[5];
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

            // lấy bài        
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
            // trộn bài 
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
        // hàm chơi theo vòng 
        private int nextplayer(int currentPlayer, bool isReverse)
        {
            if (isReverse)
            {
                return currentPlayer == 1 ? 4 : currentPlayer - 1;
            }
            return currentPlayer == 4 ? 1 : currentPlayer + 1;
        }
        public void StartServer()
        {
            serverThread = new Thread(ServerThread);
            serverThread.Start();
        }
    }
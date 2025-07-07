using BCrypt.Net; // Add this using directive for BCrypt
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

// Ensure you have added the BCrypt.Net-Next NuGet package to your project.

Server server = new Server();
server.StartServer();
Console.WriteLine("Server is running!");

public class Server
{
    //khởi tạo các biến cần thiết cho server
    private readonly string _connStr;
    private readonly FirestoreDb db;
    private Thread serverThread;
    private Socket serverSocket;
    private bool isRunning = false;
    private int byteRecv = 0;
    private byte[] buffer = new byte[1024];
    private readonly ConcurrentDictionary<Socket, (string Uid, string Username)> authenticatedPlayers = new ConcurrentDictionary<Socket, (string, string)>();
    // Track client threads and cancellation tokens for proper cleanup
    private readonly ConcurrentDictionary<Socket, (Thread Thread, CancellationTokenSource CancellationTokenSource)> clientThreads = new ConcurrentDictionary<Socket, (Thread, CancellationTokenSource)>();
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
    public async Task UpdateAllPlayersStatusToOffline()
    {
        if (db == null)
        {
            Console.WriteLine("[ERROR] Firestore database not initialized. Cannot update player statuses.");
            return;
        }

        try
        {
            Console.WriteLine("[INFO] Updating all player statuses to offline (0)...");
            CollectionReference playersRef = db.Collection("players");
            QuerySnapshot snapshot = await playersRef.GetSnapshotAsync();

            if (snapshot.Documents.Count == 0)
            {
                Console.WriteLine("[INFO] No player documents found to update.");
            }
            else
            {
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    if (document.Exists)
                    {
                        DocumentReference playerDocRef = playersRef.Document(document.Id);
                        await playerDocRef.UpdateAsync("status", 0);
                        
                    }
                }
           
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to update all player statuses to offline: {ex.Message}");
            Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
        }
    }

    public Server()
    {
        string serviceAccountFileName = "gameuno-4db86-firebase-adminsdk-fbsvc-e9eb161dd1.json";
        string currentDirectory = Directory.GetCurrentDirectory(); // Lấy thư mục hiện hành của ứng dụng
        string serviceAccountPath = Path.Combine(currentDirectory, serviceAccountFileName);
        // Kiểm tra xem tệp có tồn tại không
        if (!File.Exists(serviceAccountPath))
        {
            Console.WriteLine($"Lỗi: Không tìm thấy tệp khóa dịch vụ Firebase: {serviceAccountPath}");
            Console.WriteLine("Vui lòng tải xuống tệp .json từ Firebase Console (Project settings -> Service accounts -> Generate new private key) và đặt vào cùng thư mục với ứng dụng server của bạn.");
            return; // Dừng khởi tạo nếu không tìm thấy tệp
        }

        try
        {
            GoogleCredential credential = GoogleCredential.FromFile(serviceAccountPath);

            // FirebaseApp.Create(new AppOptions() // No longer strictly needed for just Firestore, but good to keep if you might use other Firebase services.
            // {
            //     Credential = credential,
            // });

            db = new FirestoreDbBuilder
            {
                ProjectId = "gameuno-4db86", 
                Credential = credential 
            }.Build();

            Console.WriteLine("Firebase Firestore khởi tạo thành công!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi nghiêm trọng khi kết nối Firebase hoặc khởi tạo Firestore: {ex.Message}");
            Console.WriteLine($"Chi tiết: {ex.StackTrace}");
        }
    }
    public async void TestFirestoreConnection()
    {
        if (db == null)
        {
            Console.WriteLine("Firestore database is not initialized.");
            return;
        }

        try
        {
            // Tạo một collection tạm thời và thêm một document
            CollectionReference testCollection = db.Collection("test_connections");
            DocumentReference testDoc = testCollection.Document("server_status");

            // Cập nhật hoặc tạo document với timestamp
            await testDoc.SetAsync(new { last_connected = FieldValue.ServerTimestamp, status = "online" });

            // Đọc lại document để xác nhận
            DocumentSnapshot snapshot = await testDoc.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                Console.WriteLine("Kiểm tra kết nối Firebase Firestore thành công!");
                Console.WriteLine($"Trạng thái server cuối cùng kết nối: {snapshot.GetValue<Timestamp>("last_connected").ToDateTime()}");
            }
            else
            {
                Console.WriteLine("Kiểm tra kết nối Firebase Firestore thất bại: Không thể đọc lại document.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi kiểm tra kết nối Firebase Firestore: {ex.Message}");
            Console.WriteLine($"Chi tiết: {ex.StackTrace}");
        }
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


    private async void HandleClient(Socket acceptedClient, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[DEBUG] Client thread started for socket {acceptedClient.Handle}");
        
        int currentPlayerId = 0;
        Room rooms = null;
        string username = "";
        string authenticatedUserUid = null; // Changed from FirebaseUid as we're not using Firebase Auth UIDs directly.
                                            // We'll use the username as the document ID for players.

        if (authenticatedPlayers.TryGetValue(acceptedClient, out var playerInfo))
        {
            authenticatedUserUid = playerInfo.Uid; // This will now be the username as the doc ID
            username = playerInfo.Username;
        }

        while (acceptedClient.Connected && !cancellationToken.IsCancellationRequested)
        {
            rooms = FindRoombyClientID(acceptedClient);
            try
            {
                if (rooms != null && rooms.rommbg == 1 && rooms.currentTurnStartTime.HasValue && rooms.ClientId.TryGetValue(acceptedClient, out int currentPlayerIdInRoom))
                {
                    if (rooms.currentTurn == currentPlayerIdInRoom)
                    {
                        TimeSpan elapsed = DateTime.UtcNow - rooms.currentTurnStartTime.Value;
                        if (elapsed.TotalSeconds > Room.TURN_TIME_LIMIT_SECONDS)
                        {
                            try 
                            {
                                Console.WriteLine($"[DEBUG] Người chơi {authenticatedPlayers[acceptedClient].Username} (ID: {rooms.ClientId[acceptedClient]}) trong phòng {rooms.id} đã hết thời gian. Tự động rút bài.");

                                acceptedClient.Send(Encoding.UTF8.GetBytes("Timeout: Bạn không đánh bài hoặc rút bài kịp thời. Tự động rút một lá.\n"));

                                if (rooms.Dataqueue.Count == 0) RefillDrawPile(rooms);
                                string newCard = rooms.Dataqueue.Dequeue();
                                rooms.playerHands[rooms.ClientId[acceptedClient]].Enqueue(newCard);
                                acceptedClient.Send(Encoding.UTF8.GetBytes($"DrawCard: {newCard}\n"));

                                int remaining = rooms.playerHands[rooms.ClientId[acceptedClient]].Count;
                                Broadcast(rooms, $"Remaining: {rooms.ClientId[acceptedClient]}:{remaining}\n");

                                rooms.pendingDrawCards = 0;
                                Broadcast(rooms, $"PendingDraw: 0\n");

                                int next = nextplayer(rooms.ClientId[acceptedClient], rooms.isReversed);
                                rooms.currentTurn = next;
                                Broadcast(rooms, $"Turn: {rooms.currentTurn}\n");

                                rooms.currentTurnStartTime = DateTime.UtcNow;
                                Broadcast(rooms, $"TurnTimerStart: {rooms.currentTurn}|{Room.TURN_TIME_LIMIT_SECONDS}\n");

                                // Sau khi xử lý timeout, tiếp tục vòng lặp
                                // Không cần `continue;` ở đây nếu bạn cấu trúc lại như dưới
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[SERVER ERROR] Lỗi khi xử lý phạt người chơi hết giờ {authenticatedPlayers[acceptedClient].Username}: {ex.Message}");
                            }
                        }
                    }
                }

               
                // Check for cancellation before receiving data
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"[DEBUG] Cancellation requested for client thread");
                    break;
                }
                if (!acceptedClient.Poll(100000, SelectMode.SelectRead))
                {
                    // Không có dữ liệu, tiếp tục vòng lặp để kiểm tra timer hoặc chờ
                    continue;
                }
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
                    try
                    {
                        string[] parts = message.Substring("Player: ".Length).Split('|');
                        if (parts.Length != 2)
                        {
                            acceptedClient.Send(Encoding.UTF8.GetBytes("LoginFail: InvalidFormat\n"));
                            continue; // Use continue to keep the client connected for other messages
                        }

                        string userAttemptedUsername = parts[0].Trim();
                        string userAttemptedPassword = parts[1].Trim();

                        if (string.IsNullOrEmpty(userAttemptedUsername) || string.IsNullOrEmpty(userAttemptedPassword))
                        {
                            acceptedClient.Send(Encoding.UTF8.GetBytes("LoginFail: MissingCredentials\n"));
                            continue;
                        }

                        DocumentReference userDocRef = db.Collection("players").Document(userAttemptedUsername);
                        DocumentSnapshot snapshot = await userDocRef.GetSnapshotAsync();

                        if (snapshot.Exists)
                        {
                            // User exists, check status and password
                            long status = snapshot.GetValue<long>("status");
                            string storedHashedPassword = snapshot.GetValue<string>("passwordHash");
                            string storedUsername = snapshot.GetValue<string>("username"); // Get username from Firestore

                            if (status == 1)
                            {
                                // User is already online
                                Console.WriteLine($"LoginFail for {userAttemptedUsername}: AlreadyOnline.");
                                acceptedClient.Send(Encoding.UTF8.GetBytes("LoginFail: AlreadyOnline\n"));
                                continue;
                            }
                            else if (status == 0)
                            {
                                // User is offline, check password
                                if (BCrypt.Net.BCrypt.Verify(userAttemptedPassword, storedHashedPassword))
                                {
                                    // Password matches, log in
                                    var updates = new Dictionary<string, object>
                                    {
                                        { "status", 1 },
                                        { "lastLogin", FieldValue.ServerTimestamp }
                                    };
                                    await userDocRef.UpdateAsync(updates);
                                    authenticatedPlayers.TryAdd(acceptedClient, (userAttemptedUsername, storedUsername));
                                    authenticatedUserUid = userAttemptedUsername;
                                    username = storedUsername;
                                    Console.WriteLine($"Successfully logged in {userAttemptedUsername}. Status updated to online.");
                                    acceptedClient.Send(Encoding.UTF8.GetBytes($"LoginOK: {userAttemptedUsername}\n"));
                                }
                                else
                                {
                                    // Incorrect password
                                    Console.WriteLine($"LoginFail for {userAttemptedUsername}: WrongPassword.");
                                    acceptedClient.Send(Encoding.UTF8.GetBytes("LoginFail: WrongPassword\n"));
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            // User does not exist, create new account (registration)
                            Console.WriteLine($"User {userAttemptedUsername} not found. Creating new account.");
                            try
                            {
                                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userAttemptedPassword);

                                var newUserData = new Dictionary<string, object>
                                {
                                    { "username", userAttemptedUsername }, // Using the attempted username as display name and document ID
                                    { "passwordHash", hashedPassword },
                                    { "status", 1 }, // Set status to online upon creation
                                    { "lastLogin", FieldValue.ServerTimestamp },
                                    { "createdAt", FieldValue.ServerTimestamp },
                                    { "points", 0 } // Initialize points to 0
                                };

                                await userDocRef.SetAsync(newUserData);
                                authenticatedPlayers.TryAdd(acceptedClient, (userAttemptedUsername, userAttemptedUsername));
                                authenticatedUserUid = userAttemptedUsername;
                                username = userAttemptedUsername;
                                Console.WriteLine($"Successfully created and logged in new user: {userAttemptedUsername}");
                                acceptedClient.Send(Encoding.UTF8.GetBytes($"LoginOK: {userAttemptedUsername}\n"));
                            }
                            catch (Exception createEx)
                            {
                                Console.WriteLine($"Error creating user {userAttemptedUsername} in Firestore: {createEx.Message}");
                                acceptedClient.Send(Encoding.UTF8.GetBytes($"LoginFail: RegistrationFailed|{createEx.Message}\n"));
                                continue;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"General error in Player message handling: {ex.Message}");
                        acceptedClient.Send(Encoding.UTF8.GetBytes($"LoginFail: InternalServerError\n"));
                        continue;
                    }
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
                        int roomCount = FindNextAvailableRoomId(); //id phòng mới
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
                            Console.WriteLine($"[DEBUG] Starting new game in room {room.id}. rommbg={room.rommbg}, sumcountrd={room.sumcountrd}");
                            room.rommbg = 1;
                            foreach (var sock in room.ClientId.Keys)
                            {
                                int id = room.ClientId[sock];
                                sock.Send(Encoding.UTF8.GetBytes($"YourId: {id}\n"));
                            }

                            Broadcast(room, $"Turn: {room.currentTurn}\n");
                            Broadcast(room, $"PendingDraw: {room.pendingDrawCards}\n");
                            SenUnoCardTop(room, "");
                            // Tạo chuỗi tên, giữ đúng thứ tự 1→4
                            string namesMsg = $"PlayerNames: {room.player[1].Item1}|{room.player[2].Item1}|{room.player[3].Item1}|{room.player[4].Item1}\n";
                            Broadcast(room, namesMsg);

                            SendInitialHand(room);
                            room.currentTurnStartTime = DateTime.UtcNow; // Bắt đầu timer cho người chơi 1 (currentTurn mặc định là 1)
                            Broadcast(room, $"TurnTimerStart: {room.currentTurn}|{Room.TURN_TIME_LIMIT_SECONDS}\n"); // Thông báo cho client
                            foreach (var kv in room.ClientId)
                            {
                                int pid = kv.Value;
                                int count = room.playerHands[pid].Count;
                                string msg = $"Remaining: {pid}:{count}\n";
                                Broadcast(room, msg);
                            }

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
                        if (room.pendingDrawCards > 0)
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
                    room.currentTurnStartTime = DateTime.UtcNow;
                    Broadcast(room, $"TurnTimerStart: {room.currentTurn}|{Room.TURN_TIME_LIMIT_SECONDS}\n"); // Thông báo cho client
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
                    string msg = $"Remaining: {playerId}:{remaining}\n";
                    foreach (var sock in room.ClientId.Keys)
                        sock.Send(Encoding.UTF8.GetBytes(msg));

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
                        try
                        {
                            DocumentReference winnerDocref = db.Collection("players").Document(winner);
                            DocumentSnapshot winnerSnapshot = await winnerDocref.GetSnapshotAsync();
                            if (winnerSnapshot.Exists)
                            {
                                // Cập nhật số trận thắng
                                long currentPoints = 0;
                                if (winnerSnapshot.ContainsField("points"))
                                {
                                    currentPoints = winnerSnapshot.GetValue<long>("points");
                                }
                                await winnerDocref.UpdateAsync("points", currentPoints + 1);
                                Console.WriteLine($"[Firestore] Đã cập nhật điểm cho {winner}. Tổng điểm mới: {currentPoints + 1}");
                            }
                            else
                            {
                                Console.WriteLine($"Không tìm thấy người chơi {winner} trong Firestore để cập nhật số trận thắng.");
                            }
                        }
                        catch (Exception firestoreEX)
                        {
                            Console.WriteLine($"[ERROR] Lỗi khi cập nhật điểm cho người chơi {winner}: {firestoreEX.Message}");
                        }
                        DeleteRoom(room);
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
                            remaining = room.playerHands[nextId].Count;
                            Broadcast(room, $"Remaining: {nextId}:{remaining}\n");
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
                        room.currentTurnStartTime = DateTime.UtcNow; // Người chơi được thêm 10 giây để đánh lá bài mới
                        Broadcast(room, $"TurnTimerStart: {room.currentTurn}|{Room.TURN_TIME_LIMIT_SECONDS}\n"); // Thông báo cho client
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
                        Broadcast(room, "HideCatchWindow:\n");
                    }
                    Console.WriteLine($"[DEBUG] Player {catcher} bắt lỗi player {targetId}");
                    continue;

                }
                else if (message.StartsWith("Chat: "))
                {
                    Room room = FindRoombyClientID(acceptedClient);
                    int playerId = room.ClientId[acceptedClient];
                    string msgContent = message.Substring(6).Trim();
                    string playerName = room.player[playerId].Item1;

                    string pendingMessage = $"Chat: {playerName}|{msgContent}\n";
                    Broadcast(room, pendingMessage);
                }
                else if (message.StartsWith("Disconnect: "))
                {
                    await HandleClientDisconnection(acceptedClient, username, authenticatedUserUid);
                    return; // Exit the loop since client is disconnecting
                }
                else if (message.StartsWith("GetRanking"))
                {
                    Console.WriteLine("[DEBUG] Client requested ranking data.");
                    try
                    {
                        // Lấy dữ liệu bảng xếp hạng từ Firestore
                        // Truy vấn collection "players", sắp xếp theo "points" giảm dần
                        Query rankingQuery = db.Collection("players")
                                   .OrderByDescending("points")
                                   .Limit(10); // Lấy top 10 người chơi, có thể điều chỉnh

                        QuerySnapshot rankingSnapshot = await rankingQuery.GetSnapshotAsync();

                        List<string> rankingEntries = new List<string>();
                        foreach (DocumentSnapshot document in rankingSnapshot.Documents)
                        {
                            if (document.Exists)
                            {
                                string playerName = document.GetValue<string>("username");
                                long points = 0;
                                if (document.ContainsField("points"))
                                {
                                    points = document.GetValue<long>("points");
                                }
                                rankingEntries.Add($"{playerName}|{points}");
                            }
                        }

                        string rankingMessage = "Ranking: " + string.Join(",", rankingEntries) + "\n";
                        byte[] data = Encoding.UTF8.GetBytes(rankingMessage);
                        acceptedClient.Send(data);
                        Console.WriteLine($"[DEBUG] Sent ranking data: {rankingMessage.Trim()}");
                    }
                    catch (Exception rankingEx)
                    {
                        Console.WriteLine($"[ERROR] Error fetching ranking data: {rankingEx.Message}");
                        acceptedClient.Send(Encoding.UTF8.GetBytes($"Error: Cannot retrieve ranking data.\n"));
                    }

                }
            }
            catch (Exception ex)
            {
                // Check if this is a cancellation exception
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"[DEBUG] Thread cancelled, exiting gracefully");
                    break;
                }

                Console.WriteLine($"[ERROR] Client connection error: {ex.Message}");
                // Handle unexpected disconnection
                await HandleClientDisconnection(acceptedClient, username, authenticatedUserUid);
            }
        }
        
        // Final cleanup when client thread ends
        Console.WriteLine($"[DEBUG] Client thread ending for {username}");
        
        // Clean up thread tracking if not already done
        if (clientThreads.TryRemove(acceptedClient, out var threadInfo))
        {
            Console.WriteLine($"[DEBUG] Removed client thread tracking for {username}");
            // Dispose the cancellation token source
            threadInfo.CancellationTokenSource.Dispose();
        }
    }



    private void Broadcast(Room room, string msg)
    {
        var data = Encoding.UTF8.GetBytes(msg);
        foreach (var sock in room.ClientId.Keys)
            sock.Send(data);
    }

    private void DeleteRoom(Room oldRoom)
    {
        Console.WriteLine($"[DEBUG] Deleting room {oldRoom.id} after game completion");
        
        try
        {
            // Simply remove the old room from the room list
            roomList.Remove(oldRoom);
            
            Console.WriteLine($"[DEBUG] Room {oldRoom.id} has been deleted successfully");
            Console.WriteLine($"[DEBUG] Remaining rooms: {roomList.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to delete room {oldRoom.id}: {ex.Message}");
            Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
        }
    }

    private int FindNextAvailableRoomId()
    {
        // Find the lowest available room ID
        int nextId = 0;
        while (roomList.Any(room => room.id == nextId))
        {
            nextId++;
        }
        
        Console.WriteLine($"[DEBUG] Next available room ID: {nextId}");
        return nextId;
    }

    private async Task HandleClientDisconnection(Socket clientSocket, string username, string authenticatedUserUid)
    {
        Console.WriteLine($"[DEBUG] Handling disconnection for {username}");
        
        try
        {
            // Find the room the client is in
            Room room = FindRoombyClientID(clientSocket);
            
            if (room != null)
            {
                int playerId = room.ClientId[clientSocket];
                string playerName = room.player[playerId].Item1;
                
                Console.WriteLine($"[DEBUG] Player {playerName} (ID: {playerId}) is disconnecting from room {room.id}");
                
                // Check if the game is in progress
                if (room.rommbg == 1)
                {
                    // Game is in progress, handle as player leaving during match
                    await HandlePlayerLeavingDuringMatch(room, playerId, playerName, authenticatedUserUid);
                }
                else
                {
                    // Game hasn't started yet, just remove the player
                    RemovePlayerFromRoom(room, playerId, clientSocket);
                }
            }
            
            // Update database status to offline
            if (!string.IsNullOrEmpty(authenticatedUserUid))
            {
                try
                {
                    DocumentReference userDoc = db.Collection("players").Document(authenticatedUserUid);
                    await userDoc.UpdateAsync("status", 0);
                    Console.WriteLine($"Đã cập nhật trạng thái thành offline cho người dùng: {authenticatedUserUid}");
                }
                catch (Exception dbEx)
                {
                    Console.WriteLine($"Lỗi khi cập nhật trạng thái offline cho {authenticatedUserUid}: {dbEx.Message}");
                }
            }
            
            // Remove from authenticated players
            authenticatedPlayers.TryRemove(clientSocket, out _);
            
            // Terminate and remove the client thread
            if (clientThreads.TryRemove(clientSocket, out var threadInfo))
            {
                Console.WriteLine($"[DEBUG] Terminating client thread for {username}");
                
                // Cancel the cancellation token to signal the thread to stop
                threadInfo.CancellationTokenSource.Cancel();
                
                // Check if thread is still alive and wait for it to terminate
                if (threadInfo.Thread.IsAlive)
                {
                    try
                    {
                        // Wait for the thread to terminate gracefully (max 5 seconds)
                        if (threadInfo.Thread.Join(5000))
                        {
                            Console.WriteLine($"[DEBUG] Client thread terminated gracefully for {username}");
                        }
                        else
                        {
                            Console.WriteLine($"[WARNING] Client thread did not terminate gracefully for {username}, forcing termination");
                            // Force termination if graceful termination fails
                            threadInfo.Thread.Interrupt();
                        }
                    }
                    catch (Exception threadEx)
                    {
                        Console.WriteLine($"[ERROR] Error terminating client thread for {username}: {threadEx.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"[DEBUG] Client thread already terminated for {username}");
                }
                
                // Dispose the cancellation token source
                threadInfo.CancellationTokenSource.Dispose();
            }
            else
            {
                Console.WriteLine($"[WARNING] Client thread not found for {username}");
            }
            
            // Close the socket
            clientSocket.Close();
            
            Console.WriteLine($"[DEBUG] Disconnection handling completed for {username}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Error handling client disconnection: {ex.Message}");
            Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
        }
    }

    private async Task HandlePlayerLeavingDuringMatch(Room room, int leavingPlayerId, string leavingPlayerName, string authenticatedUserUid)
    {
        Console.WriteLine($"[DEBUG] Player {leavingPlayerName} left during match in room {room.id}");
        
        // Get remaining players
        var remainingPlayers = new List<(int id, string name, Socket socket)>();
        foreach (var kvp in room.player)
        {
            int playerId = kvp.Key;
            string playerName = kvp.Value.Item1;
            Socket socket = kvp.Value.Item2;
            
            if (playerId != leavingPlayerId && socket != null && socket.Connected)
            {
                remainingPlayers.Add((playerId, playerName, socket));
            }
        }
        
        // Remove the leaving player from the room
        RemovePlayerFromRoom(room, leavingPlayerId, room.player[leavingPlayerId].Item2);
        
        // If there are remaining players, notify them about the disconnection
        if (remainingPlayers.Count > 0)
        {
            Console.WriteLine($"[DEBUG] {remainingPlayers.Count} players remaining, notifying about disconnection");
            
            // Notify all remaining players about the disconnection
            foreach (var (playerId, playerName, socket) in remainingPlayers)
            {
                try
                {
                    // Send disconnection notification message
                    string disconnectMessage = $"PlayerDisconnected: {leavingPlayerName}\n";
                    byte[] buffer = Encoding.UTF8.GetBytes(disconnectMessage);
                    socket.Send(buffer);
                    
                    Console.WriteLine($"[DEBUG] Notified player {playerName} about disconnection");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Error notifying player {playerName}: {ex.Message}");
                }
            }
        }
        
        // Delete the room since the match is over
        Console.WriteLine($"[DEBUG] Deleting room {room.id} due to player disconnection");
        DeleteRoom(room);
    }

    private void RemovePlayerFromRoom(Room room, int playerId, Socket clientSocket)
    {
        try
        {
            // Remove from ClientId dictionary
            room.ClientId.Remove(clientSocket);
            
            // Clear the player slot
            room.player[playerId] = (string.Empty, null);
            
            // Reset ready status for this player
            room.countrd[playerId] = 0;
            room.sumcountrd = room.countrd[1] + room.countrd[2] + room.countrd[3] + room.countrd[4];
            
            // Clear player hand
            if (room.playerHands.ContainsKey(playerId))
            {
                room.playerHands[playerId].Clear();
            }
            
            // Reset Uno called status
            room.UnoCalled[playerId] = false;
            
            Console.WriteLine($"[DEBUG] Removed player {playerId} from room {room.id}");
            
            // Update ready status for remaining players
            string status = $"{room.countrd[1]},{room.countrd[2]},{room.countrd[3]},{room.countrd[4]}";
            string messageToClients = $"isPlay: {status}\n";
            Broadcast(room, messageToClients);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Error removing player from room: {ex.Message}");
        }
    }

    private void LogThreadStatistics()
    {
        int activeThreads = clientThreads.Count(kvp => kvp.Value.Thread.IsAlive);
        int totalThreads = clientThreads.Count;
        
        Console.WriteLine($"[DEBUG] Thread Statistics - Active: {activeThreads}, Total: {totalThreads}");
        
        // Log details of each thread
        foreach (var kvp in clientThreads)
        {
            Socket socket = kvp.Key;
            var threadInfo = kvp.Value;
            Console.WriteLine($"[DEBUG] Thread {threadInfo.Thread.ManagedThreadId} - Alive: {threadInfo.Thread.IsAlive}, Socket: {socket.Connected}");
        }
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
        public DateTime? currentTurnStartTime; // Thời điểm bắt đầu lượt chơi hiện tại
        public const int TURN_TIME_LIMIT_SECONDS = 15; // Giới hạn 10 giây
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
                var cancellationTokenSource = new CancellationTokenSource();
                Thread clientThread = new Thread(() => HandleClient(acceptedClient, cancellationTokenSource.Token));
                clientThreads.TryAdd(acceptedClient, (clientThread, cancellationTokenSource));
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
        TestFirestoreConnection();
        UpdateAllPlayersStatusToOffline();
    }

    public void StopServer()
    {
        Console.WriteLine("[DEBUG] Stopping server and cleaning up client threads...");
        
        // Stop accepting new connections
        isRunning = false;
        
        // Close server socket
        if (serverSocket != null && serverSocket.Connected)
        {
            serverSocket.Close();
        }
        
        // Terminate all client threads
        foreach (var kvp in clientThreads)
        {
            Socket socket = kvp.Key;
            var threadInfo = kvp.Value;
            
            try
            {
                // Cancel the cancellation token to signal the thread to stop
                threadInfo.CancellationTokenSource.Cancel();
                
                if (threadInfo.Thread.IsAlive)
                {
                    Console.WriteLine($"[DEBUG] Terminating client thread {threadInfo.Thread.ManagedThreadId}");
                    
                    // Wait for graceful termination
                    if (!threadInfo.Thread.Join(3000)) // 3 second timeout
                    {
                        Console.WriteLine($"[WARNING] Forcing termination of client thread {threadInfo.Thread.ManagedThreadId}");
                        threadInfo.Thread.Interrupt();
                    }
                }
                
                // Dispose the cancellation token source
                threadInfo.CancellationTokenSource.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error terminating client thread {threadInfo.Thread.ManagedThreadId}: {ex.Message}");
            }
            
            try
            {
                if (socket.Connected)
                {
                    socket.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error closing client socket: {ex.Message}");
            }
        }
        
        // Clear collections
        clientThreads.Clear();
        authenticatedPlayers.Clear();
        roomList.Clear();
        
        Console.WriteLine("[DEBUG] Server stopped and all resources cleaned up");
    }
}
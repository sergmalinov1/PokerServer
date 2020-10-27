using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{

    public enum GameStatus
    {
        waitPlayers = 1,
        start,
        rates, 
        distribution,
        finish,
        
    }

    class Room
    {
        public int MaxPlayersInRoom { get; private set; }
        public Dictionary<int, Client> playersInRoom = new Dictionary<int, Client>();
        public List<Client> spectators = new List<Client>();
        public int idActivePlayer;

        //GameManager PART
        public GameStatus gameStatus = GameStatus.waitPlayers; //pre_start, start, rates, distribution,  pre_finish, finish
        public Deck deck;


        public Room()
        {
            MaxPlayersInRoom = 4;

            this.deck = new Deck();
            this.deck.Shuffle();
           // this.deck.ShowAllCard();
        }

        public void JoinTheRoom(int _clientId)
        {
            if (playersInRoom.Count >= MaxPlayersInRoom)
            {
                //send net_mest
                return;
            }
            else
            {               
                //sent _clientId ti v igre
            }

            int freePlace = FoundFreePlace();
            if (freePlace < 0 && freePlace > MaxPlayersInRoom)
                return;

            Client client = Server.clients[_clientId];
         //   Player player = client.player;

           // Console.WriteLine($"JoinTheRoom " + client.username);

            playersInRoom.Add(freePlace, client);

            if (spectators.Contains(client))
                spectators.Remove(client);

            int placeNum = freePlace;//= playersInRoom.IndexOf(client);

            ServerSend.NewPlayerJoins(client, placeNum);
            //ShowAllInRoom();

            if (gameStatus == GameStatus.distribution)
                gameStatus = GameStatus.start;
        }

        public void LeaveTheRoom(Client _cl)
        {
            if (Server.room.spectators.Contains(_cl))
            {
                Server.room.spectators.Remove(_cl);
              //  Console.WriteLine($"spectators leaveTheRoom  " + _cl.username);
            }


            foreach (KeyValuePair<int, Client> kvp in playersInRoom)
            {
                if (kvp.Value == _cl)
                {
                 //   Console.WriteLine($"player leaveTheRoom  " + _cl.username);

                    int numKey = kvp.Key;

                    ServerSend.PlayerLeaveTheRoom(kvp.Value, numKey);

                    playersInRoom.Remove(numKey);

                }
            }

            if (gameStatus == GameStatus.distribution)
                gameStatus = GameStatus.waitPlayers;
            //ShowAllInRoom();
        }

        public void DataForNewPlayer(int _toClient)
        {
            foreach (KeyValuePair<int, Client> kvp in playersInRoom)
            {
                int placeNum = kvp.Key;
              //  string userName = kvp.Value.username;

                ServerSend.PlayerInRoom(_toClient, kvp.Value, placeNum);
                  
            }
        }

        private void ShowAllInRoom()
        {
            Console.WriteLine($"------ShowAllInRoom-----");

            if (playersInRoom == null)
                return;

            foreach (KeyValuePair<int, Client> kvp in playersInRoom)            
            {
                int placeNum = kvp.Key;
                string userName = kvp.Value.username;

                Console.WriteLine($"player: " + userName + ", num: " + placeNum.ToString());
            }
            Console.WriteLine($"-----------------");
        }

        private int FoundFreePlace()
        {
            for (int i = 0; 1 < MaxPlayersInRoom; i++)
            {
                if (!playersInRoom.ContainsKey(i))
                    return i;
            }
            return -1;
        }


        //========GAME MANAGER======
        public void GameLogicUpdate()
        {
            switch(gameStatus)
            {
                case GameStatus.waitPlayers:
                    if (playersInRoom.Count >= 1)
                        gameStatus = GameStatus.start;
                    break;

                case GameStatus.start:
                    if (playersInRoom.Count >= 1)
                    {
                       
                        foreach (KeyValuePair<int, Client> kvp in playersInRoom)
                        {
                            Card card1 = deck.GetNextCard();
                            Card card2 = deck.GetNextCard();
                            int _toClient = kvp.Value.id;

                            kvp.Value.playerStatus = PlayerStatus.inGame;
                            Console.WriteLine("InGame: " + kvp.Value.username);

                            //Sent client Cards, status
                            ServerSend.Preflop(_toClient, card1, card2);
                            Console.WriteLine("Preflop: " + card1.ToString() + "-- " + card2.ToString());
                           

                        }
                        gameStatus = GameStatus.rates;

                        idActivePlayer = playersInRoom[0].id;

                        ServerSend.ActivPlayer(idActivePlayer);

                        //Sent chat to all - Game start
                    }

                    else
                        gameStatus = GameStatus.waitPlayers;

                    break;

                case GameStatus.distribution:
                    if (playersInRoom.Count < 2)
                    {
                        gameStatus = GameStatus.waitPlayers;
                    }
                        // Console.WriteLine("Distribution!");
                        //Send each by 2 card
                        //Sent first action to rate
                        // gameStatus = GameStatus.rates;
                    break;

                case GameStatus.rates:
                    if (playersInRoom.Count < 1)
                    {
                        gameStatus = GameStatus.waitPlayers;
                    }

                    // отправить первому в списке чтоб делал ставку



                    //first rates
                    // send all rates


                    // if all rate - gameStatus = GameStatus.rates; break;

                    //Send next action to rate
                    break;


                default:
                    Console.WriteLine("неизвестный статус");
                    break;
            }
                
        }

        public void PlayerBet(int _idPlayer, int _rate)
        { 
            if(_idPlayer != idActivePlayer)
            {
                Console.WriteLine($"Ne tot igrok poxodil");
                return;
            }
             
            //делаем проверки на сделанную ставку
            //отправляем все уведомление игрок сделал ставку такуето
            //передаем ход следующему игроку
            //отправляем всем уведомление ходит игрок под номер 2
        }
    }
}

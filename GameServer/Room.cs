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

        //GameManager PART
        public GameStatus gameStatus = GameStatus.waitPlayers; //pre_start, start, rates, distribution,  pre_finish, finish
        public Deck deck;


        public Room()
        {
            MaxPlayersInRoom = 4;

            this.deck = new Deck();
      


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

            Console.WriteLine($"JoinTheRoom " + client.username);

            playersInRoom.Add(freePlace, client);

            if (spectators.Contains(client))
                spectators.Remove(client);

            int placeNum = freePlace;//= playersInRoom.IndexOf(client);

            ServerSend.NewPlayerToAll(client, placeNum);
            //ShowAllInRoom();

            if (gameStatus == GameStatus.distribution)
                gameStatus = GameStatus.start;
        }

        public void LeaveTheRoom(Client _cl)
        {
            if (Server.room.spectators.Contains(_cl))
            {
                Server.room.spectators.Remove(_cl);
                Console.WriteLine($"spectators leaveTheRoom  " + _cl.username);
            }


            foreach (KeyValuePair<int, Client> kvp in playersInRoom)
            {
                if (kvp.Value == _cl)
                {
                    Console.WriteLine($"player leaveTheRoom  " + _cl.username);

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

                ServerSend.NewPlayer(_toClient, kvp.Value, placeNum);
                  
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
                    if (playersInRoom.Count >= 2)
                        gameStatus = GameStatus.start;
                    break;

                case GameStatus.start:
                    if (playersInRoom.Count >= 2)
                    {
                        gameStatus = GameStatus.distribution;

                        foreach (KeyValuePair<int, Client> kvp in playersInRoom)
                        {
                            kvp.Value.playerStatus = PlayerStatus.inGame;
                            Console.WriteLine("InGame: " + kvp.Value.username);
                            
                        }

                        //Sent chat to all - Game start
                    }

                    else
                        gameStatus = GameStatus.waitPlayers;

                    break;

                case GameStatus.distribution:
                    Console.WriteLine("Distribution!");
                    //Send each by 2 card
                    //Sent first action to rate
                    // gameStatus = GameStatus.rates;
                    break;

                case GameStatus.rates:
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
    }
}

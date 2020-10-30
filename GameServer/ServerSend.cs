using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class ServerSend
    {
        #region tcp
        private static void SendTCPData(int _toClient, Packet _packet)
        {

            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);

        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (Server.clients[i].tcp.socket != null)
                {
                    // Console.WriteLine(" SendTCPDataToAll: " + Server.clients[i].id);
                    Server.clients[i].tcp.SendData(_packet);
                }


            }
        }
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {

                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        private static void SendTCPDataToAllInRoom(Packet _packet, int _roomNum = 0)
        {
        
            _packet.WriteLength();


            foreach (KeyValuePair<int, Client> kvp in Server.room.playersInRoom)
            {              
                kvp.Value.tcp.SendData(_packet);
            }

            foreach (Client cl in Server.room.spectators)  //spectators
            {
                cl.tcp.SendData(_packet);
            }

        }

        private static void SendTCPDataToAllInRoom(int _exceptClient, Packet _packet, int _roomNum = 0)
        {

            _packet.WriteLength();


            foreach (KeyValuePair<int, Client> kvp in Server.room.playersInRoom)  //gamers
            {
                if (kvp.Value.id != _exceptClient)
                {
                    kvp.Value.tcp.SendData(_packet);
                }
            }

          /*  foreach (Client cl in Server.room.playersInRoom)
            {
                if (cl.id != _exceptClient)
                {
                    cl.tcp.SendData(_packet);
                }
            }*/

            foreach (Client cl in Server.room.spectators)  //spectators
            {
                if (cl.id != _exceptClient)
                {
                    cl.tcp.SendData(_packet);
                }
            }

        }
        #endregion

        #region udp

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }
        #endregion

        #region Packets
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void AuthAnswer(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.authAnswer))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void PlayerInRoom(int _toClient, Client _player, int _placeNum)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerInRoom))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write((int)_player.playerStatus);
                _packet.Write(_placeNum);

                SendTCPData(_toClient, _packet); 
            }
        } //для нового пользователя отправляеем список всех пользователей в комнтае

        public static void NewPlayerJoins(Client _player, int _placeNum)
        {

           // Console.WriteLine("_placeNum: " + _placeNum);


            using (Packet _packet = new Packet((int)ServerPackets.newPlayerJoins))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_placeNum);

                SendTCPDataToAllInRoom(_packet);
            }
        } //отправляем всем сообщение что новый игрок присоеденился
       
        public static void StartNewGame()
        {
            using (Packet _packet = new Packet((int)ServerPackets.startNewGame))
            {
                string cmd = "start";
                _packet.Write(cmd);
                SendTCPDataToAllInRoom(_packet);
            }
        }

        public static void PlayerInGame(int _idPlayer) //отправляем всем. что данный игрок играет в раздаче
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerInGame))
            {
                _packet.Write(_idPlayer);
                SendTCPDataToAllInRoom(_idPlayer, _packet);
            }
        }

        public static void ActivPlayer(int _idActivePlayer)  //отправляем всем ID активного играка. тот который ходит
        {
            using (Packet _packet = new Packet((int)ServerPackets.idActivePlayer))
            {
                _packet.Write(_idActivePlayer);
                SendTCPDataToAllInRoom(_packet);

            }
        }

        public static void PlayerStatus(int _idPlayer, PlayerStatus _playerStatus)
        {
            Console.WriteLine("PlayerStatus");
            using (Packet _packet = new Packet((int)ServerPackets.playerStatus))
            {
                _packet.Write(_idPlayer);
                _packet.Write((int)_playerStatus);
                SendTCPDataToAllInRoom(_packet);
            }
        }


        public static void Preflop(int _toClient, Card card1, Card card2)
        {
            using (Packet _packet = new Packet((int)ServerPackets.preflop))
            {
                //int firstCardSuit = (int)card1.Suit;
               // int firstCardType = (int)card1.Type;
               // int secondCardSuit = (int)card2.Suit;
              //  int secondCardType = (int)card2.Type;

                _packet.Write((int)card1.Suit);
                _packet.Write((int)card1.Type);
                _packet.Write((int)card2.Suit);
                _packet.Write((int)card2.Type);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void CardOnDeck(Card card)
        {
            using (Packet _packet = new Packet((int)ServerPackets.cardOnDesk))
            {

                _packet.Write((int)card.Suit);
                _packet.Write((int)card.Type);


                SendTCPDataToAllInRoom(_packet);
            }
        }


      

        public static void PlayerBet(int _idPlayer, int _rate)
        {
          //  Console.WriteLine("PlayerBet");
            using (Packet _packet = new Packet((int)ServerPackets.playerBet))
            {
                _packet.Write(_idPlayer);
                _packet.Write(_rate);
                SendTCPDataToAllInRoom(_packet);
            }
        }

        public static void WinResult(string msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.gameResult))
            {
                _packet.Write(msg);
                SendTCPDataToAllInRoom(_packet);
            }
        }
       
        #endregion



        public static void SendChatMsg(Client _player, string _msg)
        {

            using (Packet _packet = new Packet((int)ServerPackets.chatMsgSend))
            {
                _packet.Write(_player.username);
                _packet.Write(_msg);

                SendTCPDataToAllInRoom(_player.id, _packet);

            }
        }
        public static void PlayerLeaveTheRoom(Client _player, int _placeNum)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerLeaveRoom))
            {
                _packet.Write(_player.username);
                _packet.Write(_placeNum);

                SendTCPDataToAllInRoom(_packet);

            }

        }



        /* public static void NewSpectators(int _toClient, Client _player)
        {

            using (Packet _packet = new Packet((int)ServerPackets.newSpectator))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);

                SendTCPData(_toClient, _packet);
            }
        }*/

    }
}

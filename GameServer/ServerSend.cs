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

        public static void NewSpectators(int _toClient, Client _player)
        {

            using (Packet _packet = new Packet((int)ServerPackets.newSpectator))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void NewPlayer(int _toClient, Client _player, int _placeNum)
        {
            using (Packet _packet = new Packet((int)ServerPackets.newPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_placeNum);

                SendTCPData(_toClient, _packet); 
            }
        }

        public static void NewPlayerToAll(Client _player, int _placeNum)
        {

           // Console.WriteLine("_placeNum: " + _placeNum);


            using (Packet _packet = new Packet((int)ServerPackets.newPlayerToAll))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_placeNum);

                SendTCPDataToAllInRoom(_packet);
            }
        }

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

        public static void AllDataByRoom(Client _player, int _placeNum)
        {

        }

        #endregion
    }
}

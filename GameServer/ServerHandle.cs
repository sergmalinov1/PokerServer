using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();
            string _login = _packet.ReadString();
            string _pass = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }

          //  Console.WriteLine("Login: " + _login + ", Password: " + _pass);

            Server.clients[_fromClient].Authorization(_login, _pass);       
        }

        public static void ChatMsgReceived(int _fromClient, Packet _packet)
        {
            string _user = _packet.ReadString();
            string _msg = _packet.ReadString();


         //   Player player = Server.clients[_fromClient].player;

    

            ServerSend.SendChatMsg(Server.clients[_fromClient], _msg);
        }


        public static void PlayerEnterTheRoom(int _fromClient, Packet _packet)
        {
            string _join = _packet.ReadString();
            string _roomNum = _packet.ReadString();

            Server.clients[_fromClient].SendIntoGame();
           
        }

        public static void PlayerJoinTheRoom(int _fromClient, Packet _packet)
        {
            Console.WriteLine("PlayerJoinTheRoom");
            string _join = _packet.ReadString();
            string _roomNum = _packet.ReadString();

            Server.room.JoinTheRoom(_fromClient);

          

            //rooms[roomNum].JoinTheRoom(_fromClient);

        }

    }
}

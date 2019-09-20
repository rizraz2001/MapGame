using AspNetCoreWebSocket;
using MapGameCore.GameModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapGameCore.Services
{
    public class GameUserManager : IGameUserManager
    {
        
        WebSocketHandler webSocketHandler;
        private List<User> users;
        private User host;

        public event Action<User> UserDisconnected;

        public GameUserManager(WebSocketHandler webSocketHandler,User host)
        {
            this.webSocketHandler = webSocketHandler;
            webSocketHandler.UserDisconnected += OnDisconnected;
            this.host = host;
            users = new List<User>();
        }
        private void OnDisconnected(string socketId)
        {
            if (host.SocketId == socketId)
            {
                host.IsConnected = false;
                UserDisconnected?.Invoke(host);
                //SendAllMessage($"user : {host.Name} has disconnected");
            }
            foreach (var user in users)
            {
                if (user.SocketId == socketId)
                {
                    user.IsConnected = false;
                    UserDisconnected?.Invoke(user);
                    //SendAllMessage($"user : {user.Name} has disconnected");
                }
            }
        }
        public JoinGameStatus AddUser(User user)
        {
            if (host.Name == user.Name)
            {
                if (host.IsConnected == false)
                {
                    host.SocketId = user.SocketId;
                    host.IsConnected = true;
                    return JoinGameStatus.Reconnected;
                }
                else
                {
                    return JoinGameStatus.Failed;
                }
            }
            foreach (var item in users)
            {
                if (item.Name == user.Name)
                {
                    if (item.IsConnected == false)
                    {
                        item.SocketId = user.SocketId;
                        item.IsConnected = true;
                        return JoinGameStatus.Reconnected;
                    }
                    else
                    {
                        return JoinGameStatus.Failed;
                    }
                }
            }
            users.Add(user);
            return JoinGameStatus.Connected;
        }
        public User GetUser(string userName)
        {
            if (host.Name == userName)
                return host;

            foreach (var user in users)
            {
                if (user.Name == userName)
                    return user;
            }
            return null;
        }
        public User GetHost()
        {
            return host;
        }
        public List<User> GetAllUsers()
        {
            var AllUsers = new List<User>();
            foreach (var user in users)
                AllUsers.Add(user);
            AllUsers.Add(host);
            return AllUsers;
        }
    }
}

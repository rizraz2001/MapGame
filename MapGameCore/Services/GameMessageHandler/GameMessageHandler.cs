using AspNetCoreWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapGameCore.Services
{
    public class GameMessageHandler : IGameMessageHandler
    {
        WebSocketHandler webSocketHandler;
        IGameUserManager gameUserManager;
        public GameMessageHandler(IGameUserManager gameUserManager, WebSocketHandler webSocketHandler)
        {
            this.gameUserManager = gameUserManager;
            this.webSocketHandler = webSocketHandler;
        }

        public void SendAdminMessage(string message)
        {
            SendMessage(gameUserManager.GetHost().Name, message);
        }

        public async void SendAllMessage(string message)
        {
            var users = gameUserManager.GetAllUsers();
            foreach (var user in users)
            {
                if (user.IsConnected)
                    await webSocketHandler.SendMessageAsync(user.SocketId, message);
            }
        }

        public async void SendMessage(string userName, string message)
        {
            User user = gameUserManager.GetUser(userName);
            if (user != null)
            {
                await webSocketHandler.SendMessageAsync(user.SocketId, message);
            }
        }
    }
}

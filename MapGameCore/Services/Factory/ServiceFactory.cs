using AspNetCoreWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapGameCore.Services
{
    public static class ServiceFactory
    {
        public static IGameUserManager CreateGameUserManager(WebSocketHandler webSocketHandler, User host)
        {
            return new GameUserManager(webSocketHandler, host);
        }
        public static IGameMessageHandler CreateGameMessageHandler(IGameUserManager gameUserManager, WebSocketHandler webSocketHandler)
        {
            return new GameMessageHandler(gameUserManager, webSocketHandler);
        }
    }
}

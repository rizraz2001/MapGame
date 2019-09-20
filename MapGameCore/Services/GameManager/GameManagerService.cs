using AspNetCoreWebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapGameCore.Services
{
    public class GameManagerService : IGameManagerService, IGameManagerCommand
    {
        private WebSocketHandler webSocketHandler;
        private ConcurrentDictionary<int, MapGame> games;

        public GameManagerService(WebSocketHandler webSocketHandler)
        {
            this.webSocketHandler = webSocketHandler;
            games = new ConcurrentDictionary<int, MapGame>();
        }

        public bool CreateGame(int gameId, User host)
        {
            try
            {
                IGameUserManager gameUserManager = ServiceFactory.CreateGameUserManager(webSocketHandler, host);
                IGameMessageHandler gameMessageHandler = ServiceFactory.CreateGameMessageHandler(gameUserManager,webSocketHandler);
                var game = new MapGame(gameId,gameUserManager, gameMessageHandler);
                var isSuccess = games.TryAdd(gameId, game);
                return isSuccess;
            }
            catch
            {
                return false;
            }
        }

        public async void GetCommand(string message, string socketId)
        {
            JObject result;
            string gameId, userName;
            JObject jsonToken = JsonConvert.DeserializeObject<JObject>(message);
            var command = jsonToken["Command"].ToString();
            switch (command)
            {
                case "createGame":
                    gameId = jsonToken["GameId"].ToString();
                    userName = jsonToken["UserName"].ToString();
                    var isGameCreated = CreateGame(int.Parse(gameId), new User() { Name = userName, SocketId = socketId });
                    if(isGameCreated)
                    {
                        result = new JObject() {
                            {"command" , "createGame" } , {"isSuccess" ,true}
                        };
                    }
                    else
                    {
                        result = new JObject() {
                            {"command" , "createGame" } , {"isSuccess" ,false}
                        };
                    }
                    await webSocketHandler.SendMessageAsync(socketId, JsonConvert.SerializeObject(result));
                    break;
                case "joinGame":
                    gameId = jsonToken["GameId"].ToString();
                    userName = jsonToken["UserName"].ToString();
                    var isJoinedGame = JoinGame(int.Parse(gameId), new User() { Name = userName, SocketId = socketId });
                    if(isJoinedGame)
                    {
                        result = new JObject() {
                            {"command" , "joinGame" } , {"isSuccess" ,true}
                        };
                    }
                    else
                    {
                        result = new JObject() {
                            {"command" , "joinGame" } , {"isSuccess" ,false}
                        };
                    }
                    await webSocketHandler.SendMessageAsync(socketId, JsonConvert.SerializeObject(result));
                    break;
                case "gameCommand":
                    break;
                default:
                    break;
            }

        }

        public bool JoinGame(int gameId, User user)
        {
            MapGame game;
            bool isSuccess = games.TryGetValue(gameId, out game);
            if (isSuccess)
            {
                bool isJoinSuccess = game.JoinGame(user);
                return isJoinSuccess;
            }
            return false;
        }
    }
}

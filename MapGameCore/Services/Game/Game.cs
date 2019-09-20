using AspNetCoreWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapGameCore.GameModel;

namespace MapGameCore.Services
{
    public abstract class Game : IGame
    {
        protected GameStatus gameStatus;
        protected IGameMessageHandler gameMessageHandler;
        protected IGameUserManager gameUserManager;
        public Game(int gameId,IGameUserManager gameUserManager, IGameMessageHandler gameMessageHandler)
        {
            gameStatus = GameStatus.Idle;
            this.gameUserManager = gameUserManager;
            this.gameMessageHandler = gameMessageHandler;
            this.gameUserManager.UserDisconnected += OnUserDisconnected;
        }

        private void OnUserDisconnected(User user)
        {
            gameMessageHandler.SendAllMessage($"user : {user.Name} has disconnected");
        }
        public virtual void Start()
        {
            gameStatus = GameStatus.Started;
        }
        public virtual void Stop()
        {
            gameStatus = GameStatus.Idle;
        }
        public virtual void Pause()
        {
            gameStatus = GameStatus.Paused;
        }
        public bool JoinGame(User user)
        {
            var connectStatus = gameUserManager.AddUser(user);
            if (connectStatus == JoinGameStatus.Connected)
            {
                gameMessageHandler.SendAllMessage($"user : {user.Name} has connected");
                return true;
            }
            else if(connectStatus == JoinGameStatus.Reconnected)
            {
                gameMessageHandler.SendAllMessage($"user : {user.Name} has reconnected");
                return true;
            }
               return false;
        }
        public abstract void CommandRecieved(User user, string message);
    }

    
}

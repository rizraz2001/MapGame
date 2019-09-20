using AspNetCoreWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapGameCore.Services
{
    public class MapGame : Game
    {
        public MapGame(int gameId,IGameUserManager gameUserManager, IGameMessageHandler gameMessageHandler) : base(gameId,gameUserManager, gameMessageHandler)
        {

        }

        public void StartNextLevel()
        {

        }

        public void SetPlayerCoordinate()
        {

        }

        public override void CommandRecieved(User user, string message)
        {

        }

        public override void Start()
        {
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
        }

        public override void Pause()
        {
            base.Pause();
        }
    }
}

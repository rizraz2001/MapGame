using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapGameCore.GameModel
{
    public class CreateGameMessage : Message
    {
        public int GameId { get; set; }
    }
}

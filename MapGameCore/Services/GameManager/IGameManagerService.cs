using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapGameCore.Services
{
    public interface IGameManagerService
    {
        bool CreateGame(int gameId, User host);
        bool JoinGame(int gameId, User user);
    }
}

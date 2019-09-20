using MapGameCore.GameModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapGameCore.Services
{
    public interface IGameUserManager
    {
        event Action<User> UserDisconnected;
        User GetUser(string userName);
        User GetHost();
        List<User> GetAllUsers();
        JoinGameStatus AddUser(User user);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapGameCore.Services
{
    public interface IGame
    {
         bool JoinGame(User user);
         void Start();
         void Stop();
         void Pause();
         void CommandRecieved(User user, string message);
    }
}

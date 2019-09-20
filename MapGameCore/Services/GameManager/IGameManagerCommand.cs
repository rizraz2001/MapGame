using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapGameCore.Services
{
    interface IGameManagerCommand
    {
         void GetCommand(string message,string socketId);
    }
}

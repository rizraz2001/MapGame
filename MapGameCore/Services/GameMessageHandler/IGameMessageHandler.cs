using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapGameCore.Services
{
    public interface IGameMessageHandler
    {
        void SendMessage(string userName, string message);
        void SendAdminMessage(string message);
        void SendAllMessage(string message);
    }
}

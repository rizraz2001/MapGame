using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapGameCore
{
    public class User
    {
        public string Name { get; set; }
        public string SocketId { get; set; }
        public int Points { get; set; } = 0;
        public bool IsConnected { get; set; } = true;
    }
}

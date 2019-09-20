using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreWebSocket
{
    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        private ConcurrentDictionary<string, long> _lastPing = new ConcurrentDictionary<string, long>();

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(s => s.Key == id).Value;
        }

        public string GetSocketIdBySocket(WebSocket socket)
        {
            return _sockets.FirstOrDefault(s => s.Value == socket).Key;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public ConcurrentDictionary<string, long> GetLastPings()
        {
            return _lastPing;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(s => s.Value == socket).Key;
        }

        public async Task AddSocket(WebSocket socket)
        {
            await Task.Factory.StartNew(() =>
            {
                string socketId = CreateConnectionId();
                while (!_sockets.TryAdd(socketId, socket) || !_lastPing.TryAdd(socketId, UnixTimeNow()))
                {
                    socketId = CreateConnectionId();
                }
            });
        }

        public async Task RemoveSocket(string id)
        {
            try
            {
                _sockets.TryRemove(id, out WebSocket socket);
                _lastPing.TryRemove(id, out long lastPing);
                await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }

        public long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalMilliseconds;
        }
    }
}

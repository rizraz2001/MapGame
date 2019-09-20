using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Text;

namespace AspNetCoreWebSocket
{
    public abstract class WebSocketHandler
    {
        protected WebSocketConnectionManager WebSocketConnectionManager { get; set; }
        public event Action<string> UserDisconnected;

        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
           await WebSocketConnectionManager.AddSocket(socket);
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            UserDisconnected.Invoke(WebSocketConnectionManager.GetId(socket));
            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));
        }

        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                  offset: 0,
                                                                  count: message.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageAsync(string socketId, string message)
        {
            try
            {
                await SendMessageAsync(WebSocketConnectionManager.GetSocketById(socketId), message);
            }
            catch
            {
            }
        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var user in WebSocketConnectionManager.GetAll())
            {
                if (user.Value.State == WebSocketState.Open)
                    await SendMessageAsync(user.Value, message);
            }
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}

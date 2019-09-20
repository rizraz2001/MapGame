using MapGameCore;
using MapGameCore.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreWebSocket
{
    public class MessageHandler : WebSocketHandler
    {
        IGameManagerCommand gameManagerService;
        public MessageHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
            gameManagerService = new GameManagerService(this);
            //this.integratedOrderBookHandler = integratedOrderBookHandler;
            //integratedOrderBookHandler.OrderBookUpdateItem += OnOrderBookUpdateItem;
            //this.integratedOrderBookHandler.Start();

            //pingScheduler = new Task(PingScheduler);
            //pingScheduler.Start();
        }

        //{"Method":"ping","Timestamp":"1566819473000"}
       

        public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            using (var stream = new MemoryStream())
            {
                stream.Write(buffer, 0, result.Count);
                byte[] bytesCopy = new byte[stream.Length];
                if (stream.Length != 0)
                {
                    Array.Copy(stream.GetBuffer(), bytesCopy, stream.Length);
                    stream.SetLength(0);
                }
                var resultAsString = Encoding.UTF8.GetString(bytesCopy);
                return Task.Run(() => { HandleMessage(resultAsString, socket); });
            }
        }

        //{"Method":"orderbook","Pair":"ETH-BTC"}
        //{"Method":"orderbook","Pair":"ETH-BTC","Depth":1}
        private void HandleMessage(string request, WebSocket socket)
        {
            try
            {
                JObject jsonToken = JsonConvert.DeserializeObject<JObject>(request);
                var method = jsonToken["Method"].ToString();
                switch (method)
                {
                    case "mapGame":
                        gameManagerService.GetCommand(request, WebSocketConnectionManager.GetSocketIdBySocket(socket));
                        break;
                    default:
                        break;
                }
            }
            catch
            {

            }
        }

        public override Task OnDisconnected(WebSocket socket)
        {
            return base.OnDisconnected(socket);

            //return Task.Run(() =>
            //{
            //    foreach (var pair in orderBookSubscriptions.Keys)
            //    {
            //        orderBookSubscriptions[pair].Remove(socket);
            //    }
            //});
        }

        private class WebSocketRequest
        {
            public string Method { get; set; }
            public string Command { get; set; }
        }

        private class OrderBookDepth
        {
            public Guid Guid { get; set; }
            public int Depth { get; set; }
        }

        private class PairDetph
        {
            [JsonProperty(PropertyName = "Pair")]
            public string Pair { get; set; }
            [JsonProperty(PropertyName = "Asks")]
            public object[] Asks { get; set; }
            [JsonProperty(PropertyName = "Bids")]
            public object[] Bids { get; set; }

            public void SetDepth(int depth)
            {
                Asks = Asks.Take(depth).ToArray();
                Bids = Bids.Take(depth).ToArray();
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}

using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace ServerWebAPI.Services
{
    public class WebSocketService
    {
        private ConcurrentDictionary<Guid, WebSocket> websocketPool = new();

        public void AddWS(Guid userId, WebSocket websocket)
        {
            websocketPool.TryAdd(userId, websocket);
            Console.WriteLine($"———————————————————————————————");
            Console.WriteLine($"Current WS Pool：");
            foreach (Guid id in websocketPool.Keys)
            {
                Console.WriteLine($"                {id}");
            }
            Console.WriteLine($"Added SocketID：{userId}");
            Console.WriteLine($"———————————————————————————————");
        }

        public void RemoveWS(Guid userId)
        {
            websocketPool.TryRemove(userId, out _);
            Console.WriteLine($"———————————————————————————————");
            Console.WriteLine($"Current WS Pool：");
            foreach (Guid id in websocketPool.Keys)
            {
                Console.WriteLine($"                {id}");
            };
            Console.WriteLine($"Removed SocketID：{userId}");
            Console.WriteLine($"———————————————————————————————");
        }

        public WebSocket? GetWS(Guid userId)
        {
            if (websocketPool.TryGetValue(userId, out WebSocket? websocket)
                && (websocket.State == WebSocketState.Open || websocket.State == WebSocketState.CloseReceived))
                return websocket;
            else
                return null;
        }

        public void RemoveInvalidStateWS()
        {
            foreach (Guid id in websocketPool.Keys.ToArray()) // 获取websocketPool快照的Keys
            {
                if (websocketPool.TryGetValue(id, out WebSocket? websocket)
                    && websocket.State != WebSocketState.Open && websocket.State != WebSocketState.CloseReceived)
                {
                    websocketPool.TryRemove(id, out _);
                }
            }
            Console.WriteLine($"———————————————————————————————");
            Console.WriteLine($"Removed Invalid State！");
            Console.WriteLine($"Current WS Pool：");
            foreach (Guid id in websocketPool.Keys)
            {
                Console.WriteLine($"                {id}");
            };
            Console.WriteLine($"———————————————————————————————");
        }

        public ConcurrentDictionary<Guid, WebSocket> GetAllWS()
        {
            return websocketPool;
        }
    }
}

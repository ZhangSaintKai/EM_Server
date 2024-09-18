using Microsoft.AspNetCore.Mvc;
using ServerWebAPI.BLL;
using ServerWebAPI.Models;
using ServerWebAPI.Services;
using System.Net.WebSockets;
using System.Text;

namespace ServerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WebSocketController : ControllerBase
    {
        private readonly WebSocketService _wss;
        private readonly UserBLL _userBLL;

        public WebSocketController(WebSocketService wss, UserBLL userBLL)
        {
            _wss = wss;
            _userBLL = userBLL;
        }

        [HttpGet]
        public async Task Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await HandleWebSocketAsync(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            try
            {
                //byte[]? buffer = new byte[4 * 1024]; // 缓冲区用于接收数据
                byte[]? buffer = new byte[128]; // 缓冲区用于接收数据
                WebSocketReceiveResult? receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                TUser? user = new();
                // 循环接收客户端发送的消息，直到收到关闭消息
                while (!receiveResult.CloseStatus.HasValue)
                {
                    // 如果收到文本消息且消息已接收完整
                    if (receiveResult.MessageType == WebSocketMessageType.Text && receiveResult.EndOfMessage)
                    {
                        // 解析接收到的文本消息
                        string messageToken = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                        //从接收到的消息中获取Token，查询对应用户Id作为WebSocketID添加到WS池
                        user = await _userBLL.GetByToken(messageToken);
                        if (user == null)
                        {
                            await webSocket.SendAsync( new ArraySegment<byte>(Encoding.UTF8.GetBytes("登录信息无效")), WebSocketMessageType.Text, true, CancellationToken.None);
                            //await webSocket.CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, CancellationToken.None);
                            throw new Exception("登录信息无效");
                        }
                        else
                        {
                            _wss.AddWS(Guid.Parse(user.UserId), webSocket);
                            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("服务端已标识WS")), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                    // 等待下一条消息
                    receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                // 收到关闭消息后，关闭WebSocket连接
                await webSocket.CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, CancellationToken.None);
                _wss.RemoveWS(Guid.Parse(user.UserId));
                Console.WriteLine($"WebSocket Closed: {receiveResult.CloseStatus.Value}，{receiveResult.CloseStatusDescription}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"建立WebSocket连接异常: {e.Message}");
            }
        }


    }
}

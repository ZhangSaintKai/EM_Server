using Microsoft.AspNetCore.Mvc;
using ServerWebAPI.BLL;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;
using ServerWebAPI.Schemas;

namespace ServerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PrivateMessageController : ControllerBase
    {
        private readonly PrivateMessageBLL _messageBLL;

        public PrivateMessageController(PrivateMessageBLL messageBLL)
        {
            _messageBLL = messageBLL;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(string conversationId)
        {
            try
            {
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                List<PrivateMessageEx> pcmsgList = await _messageBLL.GetList(user.UserId, conversationId);
                return Ok(pcmsgList);

            }
            catch (Exception e)
            {
                return StatusCode(500, "错误" + e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Send(SendPrivateMessage body)
        {
            try
            {
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                string conversationId = body.ConversationId, messageType = body.MessageType, content = body.Content;
                string? source = body.Source, replyFor = body.ReplyFor;
                await _messageBLL.Send(user.UserId, conversationId, messageType, content, source, replyFor);
                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(500, "错误" + e.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Read(string conversationId)
        {
            try
            {
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                await _messageBLL.Read(user.UserId, conversationId);
                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(500, "错误" + e.Message);
            }
        }

    }
}

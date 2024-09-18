using Microsoft.AspNetCore.Mvc;
using ServerWebAPI.BLL;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;

namespace ServerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PrivateConversationController : ControllerBase
    {
        private readonly PrivateConversationBLL _conversationBLL;
        public PrivateConversationController(PrivateConversationBLL conversationBLL)
        {
            _conversationBLL = conversationBLL;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                List<PrivateConversationEx> conversationExList = await _conversationBLL.GetListByUserID(user.UserId);
                return Ok(conversationExList);
            }
            catch (Exception e)
            {
                return StatusCode(500, "错误，" + e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByID(string conversationId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(conversationId)) return UnprocessableEntity("会话ID不能为空");
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                PrivateConversationEx? conversation = await _conversationBLL.GetByIDUserID(conversationId, user.UserId);
                return Ok(conversation);
            }
            catch (Exception e)
            {
                return StatusCode(500, "错误，" + e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string otherUserId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(otherUserId)) return UnprocessableEntity("私聊对象用户ID不能为空");
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                PrivateConversationEx? conversation = await _conversationBLL.Create(user.UserId, otherUserId);
                return Ok(conversation);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

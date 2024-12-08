using Microsoft.AspNetCore.Mvc;
using ServerWebAPI.BLL;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;

namespace ServerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ContactController : ControllerBase
    {
        private readonly ContactBLL _contactBLL;

        public ContactController(ContactBLL contactBLL)
        {
            _contactBLL = contactBLL;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                List<ContactEx> result = await _contactBLL.GetListByUserID(user.UserId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, "错误" + e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByID(string contactId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(contactId))
                    return UnprocessableEntity("联系人ID不能为空");
                ContactEx? contact = await _contactBLL.GetByID(contactId);
                return Ok(contact);
            }
            catch (Exception e)
            {
                return StatusCode(500, "错误，" + e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckContact(string targetUserId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(targetUserId))
                    return UnprocessableEntity("联系人ID不能为空");
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                ContactEx? contact = await _contactBLL.GetBy2UserID(user.UserId, targetUserId);
                return Ok(contact != null);
            }
            catch (Exception e)
            {
                return StatusCode(500, "错误，" + e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string contactUserId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(contactUserId)) return UnprocessableEntity("联系人用户ID不能为空");
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                ContactEx? contact = await _contactBLL.Create(user.UserId, contactUserId);
                return Ok(contact);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

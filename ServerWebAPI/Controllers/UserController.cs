using Microsoft.AspNetCore.Mvc;
using ServerWebAPI.BLL;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;
using ServerWebAPI.Schemas;
using System.Text.RegularExpressions;

namespace ServerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UserBLL _userBLL;
        private readonly ContactBLL _contactBLL;

        public UserController(ILogger<UserController> logger, UserBLL userBLL, ContactBLL contactBLL)
        {
            _logger = logger;
            _userBLL = userBLL;
            _contactBLL = contactBLL;
        }

        [HttpPost]
        public async Task<IActionResult> Register(Login body)
        {
            try
            {
                string username = body.Username,
                    password = body.Password;
                if (string.IsNullOrEmpty(username))
                    return UnprocessableEntity("用户名不能为空");
                if (username.Contains(' '))
                    return UnprocessableEntity("用户名不能包含空格");
                if (string.IsNullOrEmpty(password))
                    return UnprocessableEntity("密码不能为空");
                if (password.Contains(' '))
                    return UnprocessableEntity("密码不能包含空格");
                Regex regex = new("^[a-zA-Z0-9_]{6,20}$");
                if (!regex.IsMatch(username))
                    return UnprocessableEntity("用户名不符合规则");
                if (!regex.IsMatch(password))
                    return UnprocessableEntity("密码不符合规则");
                await _userBLL.Register(username, password);
                return Ok("注册成功");
            }
            catch (Exception e)
            {
                return StatusCode(500, "注册失败，" + e.Message);
            }
        }

        // public IActionResult Login([FromBody] TUser user)
        // public IActionResult Login(dynamic user)
        [HttpPost]
        public async Task<IActionResult> Login(Login body)
        {
            try
            {
                string username = body.Username,
                    password = body.Password;
                Regex regex = new("^[a-zA-Z0-9_]{6,20}$");
                if (!regex.IsMatch(username))
                    return UnprocessableEntity("用户名不符合规则");
                if (!regex.IsMatch(password))
                    return UnprocessableEntity("密码不符合规则");
                TUser? user = await _userBLL.Login(username, password);
                return Ok(user);
            }
            catch (Exception e)
            {
                if (e.Message == "unregistered")
                    return Ok(e.Message);
                return StatusCode(500, "登录失败，" + e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetSelfInfo()
        {
            try
            {
                TUser? user = HttpContext.Items["User"] as TUser; // TUser是HttpContext.Items["User"]返回类型object的子类
                if (user == null) return Unauthorized("HttpContext.Items[User] IS NULL");
                return Ok(user);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                    return UnprocessableEntity("关键字不能为空");
                List<VUserProfile> userList = await _userBLL.SearchByEMID(searchText);
                return Ok(userList);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"查找用户失败，{e.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    return UnprocessableEntity("用户ID不能为空");
                VUserProfile? user = await _userBLL.GetByProfileUserID(userId);
                if (HttpContext.Items["User"] is not TUser self) return Unauthorized("HttpContext.Items[User] IS NULL");
                bool isContact = false;
                if (user != null)
                {
                    ContactEx? contact = await _contactBLL.GetBy2UserID(self.UserId, user.UserId);
                    isContact = contact != null;
                }
                return Ok(new
                {
                    user,
                    isContact
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, "获取用户简介失败，" + e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAvatar([FromBody] string avatar)
        {
            try
            {
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                if (string.IsNullOrWhiteSpace(avatar))
                    return UnprocessableEntity("头像文件资源ID不能为空");
                user.Avatar = avatar;
                await _userBLL.UpdateProfile(user);
                return Ok("修改成功");
            }
            catch (Exception e)
            {
                return StatusCode(500, "修改失败，" + e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNickName([FromBody] string nickname)
        {
            try
            {
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                if (string.IsNullOrEmpty(nickname))
                    return UnprocessableEntity("昵称不能为空");
                Regex regex = new("^[\u4e00-\u9fa5a-zA-Z0-9_]{6,20}$");
                if (!regex.IsMatch(nickname))
                    return UnprocessableEntity("昵称不符合规则");
                user.NickName = nickname;
                await _userBLL.UpdateProfile(user);
                return Ok("修改成功");
            }
            catch (Exception e)
            {
                return StatusCode(500, "修改失败，" + e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEMID([FromBody] string emid)
        {
            try
            {
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                if (string.IsNullOrEmpty(emid))
                    return UnprocessableEntity("EMID不能为空");
                Regex regex = new("^[a-zA-Z0-9_]{6,20}$");
                if (!regex.IsMatch(emid))
                    return UnprocessableEntity("EMID不符合规则");
                user.Emid = emid;
                await _userBLL.UpdateProfile(user);
                return Ok("修改成功");
            }
            catch (Exception e)
            {
                return StatusCode(500, "修改失败，" + e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePassword body)
        {
            try
            {
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                string originalPassword = body.OriginalPassword,
                    newPassword = body.NewPassword;
                Regex regex = new Regex("^[a-zA-Z0-9_]{6,20}$");
                if (!regex.IsMatch(originalPassword))
                    return UnprocessableEntity("原密码不符合规则");
                if (!regex.IsMatch(newPassword))
                    return UnprocessableEntity("新密码不符合规则");
                await _userBLL.UpdatePassword(user.UserId, originalPassword, newPassword);
                return Ok("修改成功");
            }
            catch (Exception e)
            {
                return StatusCode(500, "修改失败，" + e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePublicKey([FromBody] string publicKey)
        {
            try
            {
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                if (string.IsNullOrWhiteSpace(publicKey))
                    return UnprocessableEntity("公钥不能为空");
                user.PublicKey = publicKey;
                await _userBLL.UpdateProfile(user);
                return Ok("修改成功");
            }
            catch (Exception e)
            {
                return StatusCode(500, "修改失败，" + e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (HttpContext.Items["User"] is not TUser user) return Unauthorized("HttpContext.Items[User] IS NULL");
                await _userBLL.Logout(user);
                return Ok("已退出登录");
            }
            catch (Exception e)
            {
                return StatusCode(500, "操作失败，" + e.Message);
            }
        }


        //
        [HttpGet]
        public async Task<IActionResult> GetProfileList()
        {
            List<VUserProfile> userList = await _userBLL.GetProfileList();
            return Ok(userList);
        }

    }
}

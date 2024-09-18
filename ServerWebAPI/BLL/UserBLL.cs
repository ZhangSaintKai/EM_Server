using ServerWebAPI.Commons.Algorithm;
using ServerWebAPI.DAL;
using ServerWebAPI.Models;
using ServerWebAPI.Services;

namespace ServerWebAPI.BLL
{
    public class UserBLL
    {
        private readonly UserDAL _userDAL;
        private readonly UseSHA _useSHA;
        private readonly WebSocketService _wss;

        // 依赖注入 UserDAL实例
        public UserBLL(UserDAL userDAL, UseSHA useSHA, WebSocketService webSocketService)
        {
            _userDAL = userDAL;
            _useSHA = useSHA;
            _wss = webSocketService;
        }

        public async Task Register(string username, string password)
        {
            bool exist = await _userDAL.IsExist(username);
            if (exist) throw new Exception("用户名已存在");
            // 散列密码
            password = _useSHA.NoSaltToString(password);
            TUser user = new()
            {
                UserId = Guid.NewGuid().ToString(),
                Username = username,
                Password = password,
                Emid = username,
                NickName = username,
                PublicKey = _useSHA.NoSaltToString(Guid.NewGuid().ToString()), // 暂占位
                Avatar = "default-avatar.png",
                Token = _useSHA.WithSaltToString(username, Guid.NewGuid().ToString()),
                FileToken = _useSHA.NoSaltToString(Guid.NewGuid().ToString()),
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
            };
            await _userDAL.Create(user);
        }

        public async Task<TUser?> Login(string username, string password)
        {
            //根据用户名查不到说明未注册
            bool exist = await _userDAL.IsExist(username);
            if (!exist) throw new Exception("unregistered");
            // 散列密码
            password = _useSHA.NoSaltToString(password);
            TUser? user = await _userDAL.GetByUsernamePassword(username, password);
            // 用户名加上了密码查不到用户，说明密码错误
            if (user == null) throw new Exception("密码错误");
            // 更新用户Token
            user.Token = _useSHA.WithSaltToString(username, Guid.NewGuid().ToString());
            user.FileToken = _useSHA.NoSaltToString(Guid.NewGuid().ToString());
            user.UpdateTime = DateTime.Now;
            await _userDAL.Update(user);
            return user;
        }

        public async Task<TUser?> GetByToken(string token)
        {
            return await _userDAL.GetByToken(token);
        }

        public async Task<TUser?> GetByFileToken(string fileToken)
        {
            return await _userDAL.GetByFileToken(fileToken);
        }

        public async Task<List<VUserProfile>> SearchByEMID(string emid)
        {
            return await _userDAL.GetProfileListByEMID(emid);
        }

        public async Task<VUserProfile?> GetByProfileUserID(string userId)
        {
            return await _userDAL.GetProfileByUserID(userId);
        }

        public async Task UpdateProfile(TUser user)
        {
            await _userDAL.Update(user);
        }

        public async Task UpdatePassword(string userId, string originalPassword, string newPassword)
        {
            TUser? user = await _userDAL.GetByUserID(userId) ?? throw new Exception("目标用户不存在");
            originalPassword = _useSHA.NoSaltToString(originalPassword);
            if (user.Password != originalPassword)
                throw new Exception("原密码错误");
            user.Password = _useSHA.NoSaltToString(newPassword);
            await _userDAL.Update(user);
        }

        public async Task Logout(TUser user)
        {
            user.Token = null;
            _wss.RemoveWS(Guid.Parse(user.UserId));
            await _userDAL.Update(user);
        }


        //
        public async Task Delete(TUser user)
        {
            await _userDAL.Delete(user);
        }

        public async Task<List<VUserProfile>> GetProfileList()
        {
            return await _userDAL.GetProfileList();
        }


    }
}

using Microsoft.EntityFrameworkCore;
using ServerWebAPI.Models;

namespace ServerWebAPI.DAL
{
    public class UserDAL
    {
        private readonly DbEmContext _emContext;

        // 依赖注入 DbContext实例
        public UserDAL(DbEmContext emContext)
        {
            _emContext = emContext;
        }

        public async Task<bool> IsExist(string username)
        {
            return await _emContext.TUsers.AnyAsync(u => u.Username == username);
        }

        public async Task Create(TUser user)
        {
            _emContext.TUsers.Add(user);
            await _emContext.SaveChangesAsync();
        }

        public async Task<List<VUserProfile>> GetProfileList()
        {
            IQueryable<VUserProfile> query = _emContext.VUserProfiles;
            //IEnumerable<User> userEnumerable = await query.ToListAsync();
            //List<User> userList = await query.ToListAsync();
            return await query.ToListAsync();
        }

        public async Task<TUser?> GetByUsernamePassword(string username, string password)
        {
            IQueryable<TUser> query = _emContext.TUsers.Where(t => t.Username == username && t.Password == password);
            return await query.SingleOrDefaultAsync();
        }

        public async Task<TUser?> GetByToken(string token)
        {
            IQueryable<TUser> query = _emContext.TUsers.Where(u => u.Token == token);
            return await query.SingleOrDefaultAsync();

        }

        public async Task<TUser?> GetByFileToken(string fileToken)
        {
            IQueryable<TUser> query = _emContext.TUsers.Where(u => u.FileToken == fileToken);
            return await query.SingleOrDefaultAsync();

        }

        public async Task<List<VUserProfile>> GetProfileListByEMID(string emid)
        {
            IQueryable<VUserProfile> query = _emContext.VUserProfiles.Where(u => u.Emid == emid);
            return await query.ToListAsync();
        }

        public async Task<VUserProfile?> GetProfileByUserID(string userId)
        {
            IQueryable<VUserProfile> query = _emContext.VUserProfiles.Where(u => u.UserId == userId);
            return await query.SingleOrDefaultAsync();
        }

        public async Task<TUser?> GetByUserID(string userId)
        {
            return await _emContext.TUsers.Where(u => u.UserId == userId).SingleOrDefaultAsync();
        }

        public async Task Update(TUser user)
        {
            _emContext.TUsers.Update(user); // 需要主键
            await _emContext.SaveChangesAsync(true); // 参数true表示将对数据库进行事务性提交
        }

        public async Task Delete(TUser user)
        {
            _emContext.TUsers.Remove(user); // 需要主键
            await _emContext.SaveChangesAsync();
        }



    }
}

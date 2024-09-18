using Microsoft.EntityFrameworkCore;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;

namespace ServerWebAPI.DAL
{
    public class ContactDAL
    {
        private readonly DbEmContext _emContext;

        public ContactDAL(DbEmContext emContext)
        {
            _emContext = emContext;
        }

        public async Task<ContactEx?> GetBy2UserID(string userId, string contactUserId)
        {
            IQueryable<ContactEx> query =
                from c in _emContext.TContacts
                join u in _emContext.VUserProfiles on c.ContactUserId equals u.UserId into userJoin
                from u in userJoin.DefaultIfEmpty()
                where c.UserId == userId && c.ContactUserId == contactUserId
                select new ContactEx
                {
                    ContactId = c.ContactId,
                    ContactUser = u ?? null,
                    Remark = c.Remark ?? null,
                    CreateTime = c.CreateTime,
                    UpdateTime = c.UpdateTime
                };
            return await query.SingleOrDefaultAsync();
        }

        public async Task Create(TContact contact)
        {
            _emContext.TContacts.Add(contact);
            await _emContext.SaveChangesAsync();
        }

        public async Task<List<ContactEx>> GetListByUserID(string userId)
        {
            IQueryable<ContactEx> query =
                from c in _emContext.TContacts
                join u in _emContext.VUserProfiles on c.ContactUserId equals u.UserId into userJoin
                from u in userJoin.DefaultIfEmpty()
                where c.UserId == userId
                select new ContactEx
                {
                    ContactId = c.ContactId,
                    ContactUser = u ?? null,
                    Remark = c.Remark ?? null,
                    CreateTime = c.CreateTime,
                    UpdateTime = c.UpdateTime
                };
            return await query.ToListAsync();

        }
        public async Task<ContactEx?> GetByID(string contactId)
        {
            IQueryable<ContactEx> query =
                from c in _emContext.TContacts
                join u in _emContext.VUserProfiles on c.ContactUserId equals u.UserId into userJoin
                from u in userJoin.DefaultIfEmpty()
                where c.ContactId == contactId
                select new ContactEx
                {
                    ContactId = c.ContactId,
                    ContactUser = u ?? null,
                    Remark = c.Remark ?? null,
                    CreateTime = c.CreateTime,
                    UpdateTime = c.UpdateTime
                };
            return await query.SingleOrDefaultAsync();
        }


    }
}

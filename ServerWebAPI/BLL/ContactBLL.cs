using ServerWebAPI.DAL;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;

namespace ServerWebAPI.BLL
{
    public class ContactBLL
    {
        private readonly ContactDAL _contactDAL;
        private readonly UserBLL _userBLL;

        public ContactBLL(ContactDAL contactDAL, UserBLL userBLL)
        {
            _contactDAL = contactDAL;
            _userBLL = userBLL;
        }

        public async Task<List<ContactEx>> GetListByUserID(Guid userId)
        {
            return await _contactDAL.GetListByUserID(userId);
        }

        public async Task<ContactEx?> GetByID(Guid contactId)
        {
            return await _contactDAL.GetByID(contactId);
        }

        public async Task<ContactEx?> GetBy2UserID(Guid userId, Guid contactUserId)
        {
            return await _contactDAL.GetBy2UserID(userId, contactUserId);
        }

        public async Task<ContactEx?> Create(Guid userId, Guid contactUserId)
        {
            VUserProfile? user = await _userBLL.GetByProfileUserID(contactUserId) ?? throw new Exception("联系人用户不存在");
            //排重
            ContactEx? contact = await _contactDAL.GetBy2UserID(userId, contactUserId);
            //不存在联系人时，创建新联系人
            if (contact == null)
            {
                Guid contactId = Guid.NewGuid();
                TContact newContact = new()
                {
                    ContactId = contactId,
                    UserId = userId,
                    ContactUserId = contactUserId,
                    Remark = user.NickName,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                };
                await _contactDAL.Create(newContact);
                contact = await _contactDAL.GetByID(contactId);
            }
            return contact;
        }

    }
}

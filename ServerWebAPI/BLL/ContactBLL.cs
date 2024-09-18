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

        public async Task<List<ContactEx>> GetListByUserID(string userId)
        {
            return await _contactDAL.GetListByUserID(userId);
        }

        public async Task<ContactEx?> GetByID(string contactId)
        {
            return await _contactDAL.GetByID(contactId);
        }

        public async Task<ContactEx?> GetBy2UserID(string userId, string contactUserId)
        {
            return await _contactDAL.GetBy2UserID(userId, contactUserId);
        }

        public async Task<ContactEx?> Create(string userId, string contactUserId)
        {
            VUserProfile? user = await _userBLL.GetByProfileUserID(contactUserId) ?? throw new Exception("联系人用户不存在");
            //排重
            ContactEx? contact = await _contactDAL.GetBy2UserID(userId, contactUserId);
            //不存在联系人时，创建新联系人
            if (contact == null)
            {
                string contactId = Guid.NewGuid().ToString();
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

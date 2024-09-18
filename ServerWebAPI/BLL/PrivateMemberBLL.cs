using ServerWebAPI.DAL;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;

namespace ServerWebAPI.BLL
{
    public class PrivateMemberBLL
    {
        private readonly PrivateMemberDAL _memberDAL;

        public PrivateMemberBLL(PrivateMemberDAL memberDAL)
        {
            _memberDAL = memberDAL;
        }

    }
}

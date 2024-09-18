using Microsoft.EntityFrameworkCore;
using ServerWebAPI.Models;
using ServerWebAPI.ModelsEx;
using System;

namespace ServerWebAPI.DAL
{
    public class PrivateMemberDAL
    {
        private DbEmContext _emContext;

        public PrivateMemberDAL(DbEmContext emContext)
        {
            _emContext = emContext;
        }
    }
}

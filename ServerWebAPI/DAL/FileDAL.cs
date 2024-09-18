using Microsoft.EntityFrameworkCore;
using ServerWebAPI.Models;

namespace ServerWebAPI.DAL
{
    public class FileDAL
    {
        private readonly DbEmContext _emContext;

        public FileDAL(DbEmContext emContext)
        {
            _emContext = emContext;
        }

        public async Task<bool> IsExist(string fileId)
        {
            return await _emContext.TFiles.AnyAsync(f => f.FileId == fileId);
        }

        public async Task Create(TFile file)
        {
            _emContext.TFiles.Add(file);
            await _emContext.SaveChangesAsync();
        }

        public async Task<TFile?> GetById(string fileId)
        {
            IQueryable<TFile> query = _emContext.TFiles.Where(f => f.FileId == fileId);
            return await query.SingleOrDefaultAsync();
        }
    }
}

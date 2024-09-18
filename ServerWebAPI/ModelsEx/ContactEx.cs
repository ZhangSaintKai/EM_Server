using ServerWebAPI.Models;

namespace ServerWebAPI.ModelsEx
{
    public class ContactEx
    {

        public string ContactId { get; set; } = null!;

        public VUserProfile? ContactUser { get; set; }

        public string? Remark { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

    }
}

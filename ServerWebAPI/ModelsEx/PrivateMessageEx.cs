using ServerWebAPI.Models;

namespace ServerWebAPI.ModelsEx
{
    public class PrivateMessageEx : TPrivateMessage
    {
        public new string MessageId { get; set; } = null!;
    }
}

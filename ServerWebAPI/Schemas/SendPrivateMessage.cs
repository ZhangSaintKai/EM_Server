namespace ServerWebAPI.Schemas
{
    public class SendPrivateMessage
    {
        public Guid ConversationId { get; set; }

        public string MessageType { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string Signature { get; set; } = null!;

        public Guid? Source { get; set; }

        public Guid? ReplyFor { get; set; }
    }
}

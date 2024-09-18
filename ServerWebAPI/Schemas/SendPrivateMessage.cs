namespace ServerWebAPI.Schemas
{
    public class SendPrivateMessage
    {
        public string ConversationId { get; set; } = null!;

        public string MessageType { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? Source { get; set; }

        public string? ReplyFor { get; set; }
    }
}

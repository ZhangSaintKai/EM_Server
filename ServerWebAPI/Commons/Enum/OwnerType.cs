namespace ServerWebAPI.Commons.Enum
{
    public enum OwnerType
    {
        /// <summary>
        /// 默认：公开，无限制
        /// </summary>
        Public,

        /// <summary>
        /// 指定会话内
        /// </summary>
        Conversation,

        /// <summary>
        /// 指定会话成员
        /// </summary>
        Member,

        /// <summary>
        /// 指定用户
        /// </summary>
        User
    }
}

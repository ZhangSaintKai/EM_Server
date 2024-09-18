namespace ServerWebAPI.Schemas
{
    public class UpdatePassword
    {
        public string OriginalPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}

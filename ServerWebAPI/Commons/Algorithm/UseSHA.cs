using System.Security.Cryptography;
using System.Text;

namespace ServerWebAPI.Commons.Algorithm
{
    public class UseSHA
    {
        private readonly IConfiguration _configuration;
        public UseSHA(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string NoSaltToString(string inputStr)
        {
            using SHA256 sha256 = SHA256.Create();
            // 将输入字符串转换为字节数组
            byte[] passwordBytes = Encoding.UTF8.GetBytes(inputStr);
            // 计算哈希值
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            // 将哈希值转换为字符串
            StringBuilder stringBuilder = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes) { stringBuilder.AppendFormat("{0:x2}", b); }
            return stringBuilder.ToString();
        }

        public string WithSaltToString(string inputStr, string? salt)
        {
            if (string.IsNullOrWhiteSpace(salt))
                salt = _configuration.GetRequiredSection("HashSalt")["Default"];
            string saltedInput = string.Concat(inputStr, salt);
            using SHA512 sha512 = SHA512.Create();
            // 将输入字符串转换为字节数组
            byte[] passwordBytes = Encoding.UTF8.GetBytes(saltedInput);
            // 计算哈希值
            byte[] hashBytes = sha512.ComputeHash(passwordBytes);
            // 将哈希值转换为字符串
            StringBuilder stringBuilder = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes) { stringBuilder.AppendFormat("{0:x2}", b); }
            return stringBuilder.ToString();
        }
    }
}

using System.Security.Cryptography;
using System.Text;

namespace UserApi.BLL
{
    public static class EncryptionManager
    {
       public static string HashID(string email)
        {
            string salt = "450d0b0db2bcf4adde5032eca1a7c416e560cf44";
            var saltBytes = Encoding.UTF8.GetBytes(salt);
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(email));


                // Create array which will hold hash and original salt bytes.
                byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                    saltBytes.Length];

                // Copy hash bytes into resulting array.
                for (int i = 0; i < hashBytes.Length; i++)
                    hashWithSaltBytes[i] = hashBytes[i];

                // Append salt bytes to the result.
                for (int i = 0; i < saltBytes.Length; i++)
                    hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

                // Convert result into a base64-encoded string.
                string hashValue = Convert.ToBase64String(hashWithSaltBytes);

                return hashValue;

            }
        }

        public static string HashPassword(string PlainTextPass)
        {
            byte[] salt = new byte[16];
            Random random = new Random();
            random.NextBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(PlainTextPass, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;

        }
    }
}

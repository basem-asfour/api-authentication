using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using XSystem.Security.Cryptography;

namespace UserApi.Repository
{

    public class UserRepository : IUserRepository<Model.UserInfo>
    {
        Model.DatabaseContext db;
        public UserRepository(Model.DatabaseContext _db)
        {
            db = _db;
        }
        public void Add(Model.UserInfo entity)
        {
            entity.UserId =BLL.EncryptionManager.HashID(entity.Email);
            entity.Password = BLL.EncryptionManager.HashPassword(entity.UserName);
            db.UserInfos?.Add(entity);
            db.SaveChanges();
        }
        public Model.UserInfo? Login(ViewModel.LoginVM loginVM)
        {
            var user = db.UserInfos?.SingleOrDefault(x => x.Email == loginVM.Email);
            if (user != null)
            {
                string savedPasswordHash = user.Password;
                /* Extract the bytes */
                byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
                /* Get the salt */
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);
                /* Compute the hash on the password the user entered */
                var pbkdf2 = new Rfc2898DeriveBytes(loginVM.Password, salt, 100000);
                byte[] hash = pbkdf2.GetBytes(20);
                /* Compare the results */
                for (int i = 0; i < 20; i++)
                    if (hashBytes[i + 16] != hash[i])
                    {
                        //Wrong Password
                        return null;
                    }

            }
            return user;
        }


        public void Edit(string id, Model.UserInfo entity)
        {
            db.Update(entity);
            db.SaveChanges();

        }

        public Model.UserInfo? Find(string id)
        {
            var user = db.UserInfos?.SingleOrDefault(x => x.UserId == id);
            return user;
        }
        public IList<Model.UserInfo>? List()
        {
            return db.UserInfos?.ToList();
        }
    }

}

using CRUD_.Net_Core_Web_API.Interfaces;
using CRUD_.Net_Core_Web_API.Models;
using CRUD_.Net_Core_Web_API.Models.DB;

namespace CRUD_.Net_Core_Web_API.Repositories
{
    public class UserRepo :IAuth<User>
    {
        private readonly ProjectDbContext db;
        public UserRepo(ProjectDbContext db)
        {
            this.db = db;
        }

        public User Authenticate(string username, string password)
        {
            var userdata = db.Users.FirstOrDefault(
                u =>
                    u.Username.Equals(username) &&
                    u.Password.Equals(password)
            );
            if (userdata != null)
            {
                return userdata;
            }
            else
            {
                return null;
            }
        }
    }
}

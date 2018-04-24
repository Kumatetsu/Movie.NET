using ClassLibrary1.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Dao
{
    class UserDao : IUserDao
    {

        public User CreateUser(User user)
        {
            DataModelContainer ctx = new DataModelContainer();
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return user;
        }

        public User UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public List<User> getAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUser(int uid)
        {
            throw new NotImplementedException();
        }
    }
}

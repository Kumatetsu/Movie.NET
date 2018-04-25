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
            DataModelContainer ctx = new DataModelContainer();
            User updated = ctx.Users.Where(u => u.Id == user.Id).FirstOrDefault();
            updated = user;
            ctx.SaveChanges();
            return updated;
        }

        public bool DeleteUser(User user)
        {
            DataModelContainer ctx = new DataModelContainer();
            ctx.Users.Remove(user);
            ctx.SaveChanges();
            return true;
        }

        public List<User> getAllUsers()
        {
            DataModelContainer ctx = new DataModelContainer();
            List<User> users = ctx.Users.ToList();

            return users;
        }

        public User GetUser(int uid)
        {
            DataModelContainer ctx = new DataModelContainer();
            User query = ctx.Users.Where(u => u.Id == uid).FirstOrDefault();

            return query;
        }
    }
}

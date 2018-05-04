using ModelMovieNet.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMovieNet.Dao
{
    class UserDao : IUserDao
    {

        public User CreateUser(User user)
        {
            DataModelContainer ctx = new DataModelContainer();
            Console.WriteLine("user in create: " + user.Login);
            ctx.UserSet.Add(user);
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
            DataModelContainer ctx = new DataModelContainer();
            return ctx.UserSet.ToList();
        }

        public User GetUser(int uid)
        {
            DataModelContainer ctx = new DataModelContainer();
            return ctx.UserSet.Where(u => u.Id == uid).Select(u => u).FirstOrDefault();
        }

        public User LogUser(User user)
        {
            DataModelContainer ctx = new DataModelContainer();
            return ctx.UserSet
                .Where(u => u.Login == user.Login && user.Password == u.Password)
                .Select(u => u).FirstOrDefault();
        }
    }
}

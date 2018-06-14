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
            DataModelContainer ctx = DataModelContainer.GetDb();
            Console.WriteLine("user in create: " + user.Login);
            ctx.UserSet.Add(user);
            ctx.SaveChanges();
            return user;
        }

        public User UpdateUser(User user)
        {
            Console.WriteLine("User passed to update: " + user.ToString());
            DataModelContainer ctx = DataModelContainer.GetDb();
            User toUpdate = ctx.UserSet.Where(u => u.Id == user.Id).FirstOrDefault();
            Console.WriteLine("In UpdateUser, return of update method: " + toUpdate.ToString());
            toUpdate.Firstname = user.Firstname;
            toUpdate.Lastname = user.Lastname;
            toUpdate.Login = user.Login;
            toUpdate.Password = user.Password;
            if (toUpdate.Equals(user))
            {
                Console.WriteLine("Update ok");
                ctx.SaveChanges();
                return toUpdate;
            }
            else
            {
                throw new Exception("Update failed");
            }
        }

        public bool DeleteUser(User user)
        {
            DataModelContainer ctx = DataModelContainer.GetDb();
            User toDelete = ctx.UserSet.Where(u => u.Id == user.Id).FirstOrDefault();
            ctx.UserSet.Remove(toDelete);
            ctx.SaveChanges();
            return true;
        }

        public List<User> getAllUsers()
        {
            DataModelContainer ctx = DataModelContainer.GetDb();
            return ctx.UserSet.ToList();
        }

        public User GetUser(int uid)
        {
            DataModelContainer ctx = DataModelContainer.GetDb();
            return ctx.UserSet.Where(u => u.Id == uid).FirstOrDefault();
        }

        public User LogUser(User user)
        {
            DataModelContainer ctx = DataModelContainer.GetDb();
            return ctx.UserSet
                .Where(u => u.Login == user.Login && user.Password == u.Password)
                .Select(u => u).FirstOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Interface
{
    public interface IUserDao
    {
        User CreateUser(User user);
        User UpdateUser(User user);
        Boolean DeleteUser(User user);
        User GetUser(int uid);
        List<User> getAllUsers();
    }
}

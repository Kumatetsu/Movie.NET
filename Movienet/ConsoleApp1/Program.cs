using System;
using ClassLibrary1;
using ClassLibrary1.Interface;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceFacade serviceFacade = ClassLibrary1.Factory.ServiceFacadeFactory.GetServiceFacade();
            IUserDao uDao = serviceFacade.GetUserDao();

            User user = new User();

            user.firstname = "Jean";
            user.lastname = "Billaud";
            user.login = "Kumatetsu";
            user.password = "root";

            user = uDao.CreateUser(user);

            Console.WriteLine(user);
            Console.Read();
        }
    }
}

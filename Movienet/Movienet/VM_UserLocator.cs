using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movienet
{
    public class VM_UserLocator
    {
        public static VM_User VM_User_Instance { get; set; }
        public static VM_DisplayUsers VM_DisplayUsers { get; set; }
        public VM_UserLocator()
        {
            VM_User_Instance = new VM_User();
            VM_DisplayUsers = new VM_DisplayUsers();
        }
    }
}

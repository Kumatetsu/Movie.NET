using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movienet
{
    public class VM_CommentLocator
    {
        public static VM_Comment VM_Comment_Instance { get; set; }
        public static VM_DisplayComments VM_DisplayComments { get; set; }
        public VM_CommentLocator()
        {
            VM_Comment_Instance = new VM_Comment();
            VM_DisplayComments = new VM_DisplayComments();
        }
    }
}

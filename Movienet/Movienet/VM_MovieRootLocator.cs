using Movienet.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movienet
{
    public class VM_MovieRootLocator
    {
        public static VM_MovieRootFrame VM_MovieRootFrame { get; set; }
        public VM_MovieRootLocator()
        {
            VM_MovieRootFrame = new VM_MovieRootFrame();
        }
    }
}

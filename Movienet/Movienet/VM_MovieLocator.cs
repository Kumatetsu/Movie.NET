using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movienet
{
    public class VM_MovieLocator
    {
        public static VM_Movies VM_Movie_Instance { get; set; }
        public static VM_DisplayMovies VM_DisplayMovies { get; set; }
        public VM_MovieLocator()
        {
            VM_Movie_Instance = new VM_Movies();
            VM_DisplayMovies = new VM_DisplayMovies();
        }
    }
}

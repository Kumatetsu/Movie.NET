using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMovieNet.Interface
{
    public interface IServiceFacade
    {
        ICommentDao GetCommentDao();
        IMovieDao GetMovieDao();
        IUserDao GetUserDao();
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Interface
{
    interface IServiceFacade
    {
        ICommentDao GetCommentDao();
        IFilmDao GetFilmDao();
        IUserDao GetUserDao();
    }
}
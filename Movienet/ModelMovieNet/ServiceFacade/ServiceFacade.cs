using ModelMovieNet.Factory;
using ModelMovieNet.Interface;

namespace ModelMovieNet.ServiceFacade
{
    class ServiceFacade : IServiceFacade
    {

        private IUserDao uDao    = null;
        private IMovieDao mDao    = null;
        private ICommentDao cDao = null;

        public ServiceFacade() {
            cDao = AbstractDaoFactory.getFactory().getCommentDao();
            mDao = AbstractDaoFactory.getFactory().getMovieDao();
            uDao = AbstractDaoFactory.getFactory().GetUserDao();
        }

        public ICommentDao GetCommentDao()
        {
            return cDao;
        }

        public IMovieDao GetMovieDao()
        {
            return mDao;
        }

        public IUserDao GetUserDao()
        {
            return uDao;
        }
    }
}

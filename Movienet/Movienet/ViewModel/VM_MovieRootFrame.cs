using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ModelMovieNet;
using System;
using System.Windows.Controls;
using static Movienet.State_Machine;

namespace Movienet.ViewModel
{
    public class VM_MovieRootFrame: ViewModelBase
    {
        /**
         * Application pages
         * */
        private Page _list;
        public Page List
        {
            get
            {
                return _list;
            }
            set
            {
                _list = value;
                RaisePropertyChanged("List");
            }
        }
        private Page _detail;
        public Page Detail
        {
            get
            {
                return _detail;
            }
            set
            {
                _detail = value;
                RaisePropertyChanged("Detail");
            }
        }
        private Page _search;
        public Page Search
        {
            get
            {
                return _search;
            }
            set
            {
                _search = value;
                RaisePropertyChanged("Search");
            }
        }

        private Page _comments;

        public Page Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        private Page _commentDetail;

        public Page CommentDetail
        {
            get { return _commentDetail; }
            set { _commentDetail = value; }
        }

        private User _sessionUser;

        public User SessionUser
        {
            get { return _sessionUser; }
            set { _sessionUser = value; }
        }


        // Information
        private String _info;

        // Navigation
        public RelayCommand OpenCreateMovie { get; set; }

        /**
         * Constructor
         * */
        public VM_MovieRootFrame() : base()
        {
            List = new DisplayMovies();
            Detail = new MovieDetail();
            Search = new SearchMovies();
            Comments = new DisplayComments();
            CommentDetail = new AddComment();
            MessengerInstance.Register<STATE>(this, "state_changed", StateChangedAck);
            MessengerInstance.Register<User>(this, "SessionUser", SetSessionUser);
            OpenCreateMovie = new RelayCommand(() => GoToAddMovie());
            MessengerInstance.Send("VM_MovieRootFrame", "Context");
            if (SessionUser != null)
            {
                Comments = new DisplayComments();
                CommentDetail = new AddComment();
            }
        }

        /**
         * Information
         * */
        public String Info
        {
            get
            {
                return _info;
            }
            set
            {
                _info = value;
                RaisePropertyChanged("Info");
            }
        }

        /**
         * State catcher
         * Building views after state changed
         * Views are supposed to ask the state on build
         * */
        protected void StateChangedAck(STATE state)
        {
            Console.WriteLine("VM_MovieRootFrame VM_STATE CHANGED");
            switch (state)
            {
                case STATE.ADD_MOVIE:
                    Info = "VM_MovieRootFrame Handling ADD_MOVIE state";
                    Console.WriteLine(Info);
                    Detail = new AddMovie();
                    break;
                case STATE.SELECT_MOVIE:
                    Info = "VM_MovieRootFrame Handling SELECT_MOVIE state";
                    Console.WriteLine(Info);
                    Detail = new MovieDetail();
                    break;
                case STATE.UPDATE_MOVIE:
                    Info = "VM_MovieRootFrame Handling UPDATE_MOVIE state";
                    Console.WriteLine(Info);
                    break;
                case STATE.DELETE_MOVIE:
                    Info = "VM_MovieRootFrame Handling DELETE_MOVIE state";
                    Console.WriteLine(Info);
                    break;
                case STATE.NEED_AUTHENTICATION:
                    Info = "VM_MovieRootFrame Handling NEED_AUTHENTICATION state";
                    Console.WriteLine(Info);
                    Detail = new Authentication();
                    break;
                default:
                    break;
            }

        }

        /**
         * Display an AddUser view on Detail page
         * */
        private void GoToAddMovie()
        {
            Info = "VM_MovieRootFrame set context to Add and movie to new one";
            Console.WriteLine(Info);
            Movie m = new Movie();
            MessengerInstance.Send(m, "SetMovie");
            MessengerInstance.Send(STATE.ADD_MOVIE, "SetState");
        }

        void SetSessionUser(User sessionUser)
        {
            SessionUser = sessionUser;
            if (Comments == null)
                Comments = new DisplayComments();
            if (CommentDetail == null)
                CommentDetail = new AddComment();
        }

    }
}

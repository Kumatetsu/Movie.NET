using GalaSoft.MvvmLight;
using ModelMovieNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movienet
{
    /**
     * Class used to communicate a state and a user variable
     * among view models.
     * The principe is to update those variable throught messenger
     * To get them, the good way is to subscribe to CurrentUser and CurrentState channels
     * then to ask a trigger sending a string to Context channel
     * */
    public class State_Machine: ViewModelBase
    {
        public enum STATE {
            NONE,
            ADD_USER,
            SELECT_USER,
            UPDATE_USER,
            DELETE_USER,
            ADD_MOVIE,
            SELECT_MOVIE,
            UPDATE_MOVIE,
            DELETE_MOVIE,
            SELECT_COMMENT,
            UPDATE_COMMENT,
            ADD_COMMENT,
            DELETE_COMMENT,
            NEED_AUTHENTICATION
        };
        private static STATE vm_state;
        private static User currentUser   = null;
        private static Movie currentMovie = null;
        private static Comment currentComment = null;
        private static User sessionUser   = null;

        public State_Machine()
        {
            /**
             * The class subscribe to two setters and a request for the actual state
             * */
            MessengerInstance.Register<STATE>(this, "SetState", SetState);
            MessengerInstance.Register<User>(this, "SetUser", SetUser);
            MessengerInstance.Register<Movie>(this, "SetMovie", SetMovie);
            MessengerInstance.Register<Comment>(this, "SetComment", SetComment);
            MessengerInstance.Register<User>(this, "SetSessionUser", SetSessionUser);
            MessengerInstance.Register<string>(this, "Context", SendContext);
        }

        protected STATE VM_State
        {
            get { return vm_state; }
            private set
            {
                vm_state = value;
                MessengerInstance.Send(VM_State, "state_changed");
            }
        }

        protected User Session
        {
            get { return sessionUser; }
            private set
            {
                Console.WriteLine("User_State_Machine: Setting session User");
                sessionUser = value;
                MessengerInstance.Send(Session, "SessionUser");
            }
        }

        protected User CurrentUser
        {
            get { return currentUser; }
            private set
            {
                currentUser = value;
            }
        }

        protected Movie CurrentMovie
        {
            get { return currentMovie; }
            set
            {
                currentMovie = value;
            }
        }

        protected Comment CurrentComment
        {
            get { return currentComment; }
            set
            {
                currentComment = value;
            }
        }

        /**
         * Possible upper logic for the class wich
         * implement it (Actually, VM_RootFrame)
         * */
        public virtual void StateChangedAck(STATE state)
        {
            Console.WriteLine("STATE_MACHINE Ack");
        }

        /**
         * Setters messenger callbacks
         * */
        void SetState(STATE state)
        {
            Console.WriteLine("State_Machine: set State to: " + state);
            VM_State = state;
        }
        
        void SetUser(User user)
        {
            CurrentUser = user;
        }

        void SetMovie(Movie movie)
        {
            Console.WriteLine("State Machine set movie to: " + movie.ToString());
            CurrentMovie = movie;
        }

        void SetComment(Comment comment)
        {
            CurrentComment = comment;
        }

        void SetSessionUser(User user)
        {
            Console.WriteLine("State Machine set session user: " + ((user != null) ? user.Login : "INVALID USER"));
            Session = user;
        }

        /**
         * When a viewmodel send a messenger on 'Context'
         * User_State_Machine send CurrentUser and CurrentState
         * */
        void SendContext(string ask)
        {
            Console.WriteLine("State_Machine: send context to " + ask);
            Console.WriteLine("State_Machine: currentUser " + CurrentUser?.ToString() + " " + VM_State);
            Console.WriteLine("State_Machine: currentMovie " + CurrentMovie?.ToString() + " " + VM_State);
            Console.WriteLine("State_Machine: currentComment" + CurrentComment?.ToString() + " " + VM_State);
            MessengerInstance.Send(CurrentUser, "CurrentUser");
            MessengerInstance.Send(CurrentMovie, "CurrentMovie");
            MessengerInstance.Send(CurrentComment, "CurrentComment");
            MessengerInstance.Send(VM_State, "CurrentState");
            MessengerInstance.Send(Session, "CurrentSessionUser");
        }
    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ModelMovieNet;
using ModelMovieNet.Factory;
using ModelMovieNet.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Movienet.State_Machine;

namespace Movienet
{
    public class VM_Comment: ViewModelBase
    {

        /**
         * Access to Dao, can be inherited
         * */
        private static IServiceFacade Services { get; } = ServiceFacadeFactory.GetServiceFacade();
        private static ICommentDao cDao { get; } = Services.GetCommentDao();

        public RelayCommand Add { get; set; } = null;
        public RelayCommand Update { get; set; } = null;
        public RelayCommand Delete { get; set; } = null;
        public RelayCommand GoToUpdate { get; set; } = null;

        private Comment _comment;
        private Page _form;
        private STATE _state;
        private ICollection<Comment> _comments;
        private IDictionary<string, string> _errors { get; set; } = new Dictionary<string, string>()
        {
            { "Id", "Can't update or delete if id is 0" },
            { "Message", "This field can't be empty" },
            { "Note", "This field can't be empty" },
            { "Movie", "No movie linked to this comment" },
            { "User", "No user linked to this comment" },
        };

        private string _info;

        public VM_Comment()
        {
            Comment = new Comment();
            Add = new RelayCommand(AddComment, CanAdd);
            Update = new RelayCommand(EditComment, CanEdit);
            Delete = new RelayCommand(DeleteComment, CanDelete);
            GoToUpdate = new RelayCommand(OpenUpdateForm);
            Info = "Informations: ";
            /**
             * State synchronisation
             * */
            MessengerInstance.Register<STATE>(this, "state_changed", HandleStateChange);
            MessengerInstance.Register<User>(this, "CurrentSessionUser", SetUser);
            MessengerInstance.Register<Movie>(this, "CurrentMovie", SetMovie);
            MessengerInstance.Register<Comment>(this, "CurrentComment", SetComment);
            MessengerInstance.Register<STATE>(this, "CurrentState", SetState);
            // After registering, we ask for current objects state and user
            MessengerInstance.Send("VM_Comment", "Context");
        }

        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;
                RaisePropertyChanged("Info");
            }
        }

        void SetUser(User user)
        {
            User = user;
        }

        void SetMovie(Movie movie)
        {
            Movie = movie;
        }

        private User _user;

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                CommentUser = value;
            }           
        }

        private Movie _movie;

        public Movie Movie
        {
            get { return _movie; }
            set {
                _movie = value;
                CommentMovie = value;
            }
        }


        public Comment Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                RaiseCUD("Comment");
                if (_comment != null)
                {
                    Hydrate(_comment.Id, _comment.Message, _comment.Note, User, Movie);
                    MessengerInstance.Send("SetComment", Comment);
                }
            }
        }

        public int Id
        {
            get { return Comment.Id; }
            set
            {
                Comment.Id = value;
                RaiseCUD("Id");
            }
        }

        public string Message
        {
            get { return Comment.Message; }
            set
            {
                Comment.Message = value;
                RaiseCUD("Message");
            }
        }

        public int Note
        {
            get { return Comment.Note; }
            set
            {
                Comment.Note = value;
                RaiseCUD("Note");
            }
        }

        public User CommentUser
        {
            get { return Comment.User; }
            set
            {
                if (value == null)
                {
                    Console.WriteLine("Setting Comment User, value is null");
                    return;
                }
                Console.WriteLine("set comment.user: " + value.ToString());
                Comment.User = value;
                RaiseCUD("User");
            }
        }

        public Movie CommentMovie
        {
            get { return Comment.Movie; }
            set
            {
                Comment.Movie = value;
                RaiseCUD("Movie");
            }
        }

        /**
         * Page containing the update form
         * */
        public Page Form
        {
            get
            {
                return _form;
            }
            set
            {
                _form = value;
                RaisePropertyChanged("Form");
            }
        }

        /**
         * Shortcut to inline User Props affectation
         * */
        private void Hydrate(int _id, string _me, int _no, User _u, Movie _m)
        {
            Id = _id;
            Message = _me;
            Note = _no;
            CommentUser = _u;
            CommentMovie = _m;
            Console.WriteLine("Actual hydrated Comment: " + Comment.ToString());
        }

        void SetComment(Comment comment)
        {
            Console.WriteLine("VM_Comment: Call to SetComment, messenger callback");
            Console.WriteLine("Actual Comment in VM_Comment: " + Comment.ToString());
            if (comment == null)
            {
                Console.WriteLine("Received comment is null");
            }
            else
            {
                Console.WriteLine("VM_Comment: Callback for messenger with token CurrentComment.");
                Comment = comment;
            }
        }

        /**
         * Internal state
         * */
        public STATE State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                Console.WriteLine("State set to: " + _state);
                Info = "VM_Comment: State set to " + State;
                RaisePropertyChanged("State");
            }
        }
        /**
         *  The state is a trivial state machine
         *  Allow us to manage the add/update context.
         * */
        public void SetState(STATE state)
        {
            Console.WriteLine("VM_Comment: Call to SetState, messenger callback in ");
            State = state;
        }

        /**
         * When state change, we send a request of Context to
         * User_State_Machine to received the updated state
         * We pull information
         * */
        public void HandleStateChange(STATE _)
        {
            MessengerInstance.Send("VM_Comment after received state_changed", "Context");
        }

        /**
         * RaiseCRUD, allow to regroup EventRaising logic
         * */////////////////////////////////////////////
        void RaiseCU(string prop)
        {
            if (Add != null)
                Add.RaiseCanExecuteChanged();
            if (Update != null)
                Update.RaiseCanExecuteChanged();
            RaisePropertyChanged(prop);
        }
        void RaiseCUD(string prop)
        {
            if (Add != null)
                Add.RaiseCanExecuteChanged();
            if (Update != null)
                Update.RaiseCanExecuteChanged();
            // if (Delete != null)
            //     Delete.RaiseCanExecuteChanged();
            RaisePropertyChanged(prop);
        }
        //////////////////////////////////////////////////

        /**
         *  This View Model know how to validate
         *  the object he represent.
         * */
        protected Boolean IsValidComment()
        {
            Console.WriteLine("Call on IsValidComment");
            Boolean check = false;
            Info = "";
            if (Comment != null)
            {
                Console.WriteLine("IsValidComment: Comment != null");
                check = (
                    ((Comment.Message != null && Comment.Message.Length != 0) || TriggerFormError("Message")) &&
                    ((Comment.Note >= 0) || TriggerFormError("Note")) &&
                    (Comment.User != null || TriggerFormError("User")) &&
                    (Comment.Movie != null || TriggerFormError("Movie"))
                );
            }
            else { Console.WriteLine("IsValidComment:  Comment is null"); }
            return check;
        }

        /**
         * Test if there is a selected comment
         * And if this comment exist in dao
         * */
        protected Boolean IsExistingComment()
        {
            Console.WriteLine("Call on IsExistingComment");
            if (Comment == null || Id == 0)
            {
                Info = "Comment is null or don't have ID";
                Console.WriteLine(Info);
                return false;
            }
            try
            {
                Comment check = cDao.GetComment(Id);
                return (check != null && check.Id == Id) ? true : TriggerFormError("Id");
            }
            catch (Exception e)
            {
                Info = "Testing user existence failed: " + e.Message;
                Console.WriteLine(Info);
                return false;
            }
        }
        /**
         * Update information message with static _errors dictionnary
         * */
        private Boolean TriggerFormError(string prop)
        {
            Console.WriteLine("Trigger " + prop + " error: " + _errors[prop]);
            Info = "[" + prop + "]: " + _errors[prop];
            return false;
        }

        void OpenUpdateForm()
        {
            Form = new UpdateComment();
        }

        /**
         *  Add Comment method used in Add RelayCommand
         *  With CanAdd validator
         * */
        void AddComment()
        {
            Console.WriteLine("In Add Comment from VM_Comment");
            Info = "Adding a Comment";

            try
            {
                Comment checkComment = CanAdd() ? cDao.CreateComment(Comment) : null;
                if (checkComment != null && checkComment.Id > 0)
                {
                    Info = "Add happens well";
                    MessengerInstance.Send(STATE.ADD_COMMENT, "SetState");
                    Console.WriteLine(Info);
                }
                else
                {
                    Info = "Add failed, contact administrator";
                    Console.WriteLine(Info);
                }
            }
            catch (Exception e)
            {
                Info = "Adding Comment failed with exception: " + e.Message + " Stacktrace: " + e.StackTrace;
                Console.WriteLine(Info);
            }
        }

        /**
         *  Add RelayCommand Validator
         * */
        Boolean CanAdd()
        {
            Console.WriteLine("Call on CanAdd");
            bool check = IsValidComment();
            Info = check ? "The actual comment is valid" : "The actual comment isn't valid: " + Info;
            return check;
        }

        /**
         * Method used in Update RelayCommand
         * With CanEdit validator
         * */
        void EditComment()
        {
            Console.WriteLine("In Add User from VM_AddUser");
            try
            {
                Comment checkComment = CanEdit() ? cDao.UpdateComment(Comment) : null;
                if (checkComment != null && checkComment.Id > 0)
                {
                    Info = "Update happens well";
                    Console.WriteLine(Info);
                    MessengerInstance.Send(STATE.UPDATE_COMMENT, "SetState");
                }
                else
                {
                    Info = "Update failed, contact administrator.";
                    Console.WriteLine(Info);
                }
            }
            catch (Exception e)
            {
                Info = "Update user failed with exception: " + e.Message;
                Console.WriteLine(Info);
            }
        }

        /**
         *  Update RelayCommand Validator
         * */
        Boolean CanEdit()
        {
            bool check = IsValidComment() && IsExistingComment();
            Info = check ? "The actual user is valid" : "The actual user isn't valid" + Info;
            return check;
        }

        /**
         * Method used in Delete RelayCommand
         * With CanDelete validator
         * */
        void DeleteComment()
        {
            Console.WriteLine("In DeleteComment from VM_Comment");
            if (!CanDelete())
                return;
            Info = "Deleting a comment";
            try
            {
                if (cDao.DeleteComment(Comment))
                {
                    Info = "DeleteComment passed";
                    MessengerInstance.Send(STATE.DELETE_COMMENT, "SetState");
                    Comment = new Comment();
                }
                else
                    Info = "DeleteComment failed";
                Console.WriteLine(Info);
            }
            catch (Exception e)
            {
                Info = "Update comment failed: " + e;
                Console.WriteLine("Exception PERSONNALISEE: " + e.Message);
            }
        }

        /**
         *  Delete RelayCommand Validator
         * */
        Boolean CanDelete()
        {
            return IsExistingComment();
        }
    }
}

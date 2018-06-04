using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ModelMovieNet;
using ModelMovieNet.Factory;
using ModelMovieNet.Interface;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using static Movienet.State_Machine;

namespace Movienet
{
    public class VM_Movies: ViewModelBase
    {
        /**
         * Access to Dao, can be inherited
         * */
        private static IServiceFacade Services { get; } = ServiceFacadeFactory.GetServiceFacade();
        private static IMovieDao mDao { get; } = Services.GetMovieDao();
        public RelayCommand Add { get; set; } = null;
        public RelayCommand Update { get; set; } = null;
        public RelayCommand Delete { get; set; } = null;
        public RelayCommand GoToUpdate { get; set; } = null;
        private Movie _movie;
        private Page _form;
        private STATE _state;
        private ICollection<Comment> _comments;
        private IDictionary<string, string> _errors { get; set; } = new Dictionary<string, string>()
        {
            { "Id", "Can't update or delete if id is 0" },
            { "Title", "This field can't be empty" },
            { "Abstract", "This field can't be empty" },
            { "Type", "This field can't be empty" },
        };

        private string _info;

        public VM_Movies()
        {
            Movie = new Movie();
            Add = new RelayCommand(AddMovie, CanAdd);
            Update = new RelayCommand(EditMovie, CanEdit);
            Delete = new RelayCommand(DeleteMovie, CanDelete);
            GoToUpdate = new RelayCommand(OpenUpdateForm);
            Info = "Informations: ";
            /**
             * State synchronisation
             * */
            MessengerInstance.Register<STATE>(this, "state_changed", HandleStateChange);
            MessengerInstance.Register<Movie>(this, "CurrentMovie", SetMovie);
            MessengerInstance.Register<STATE>(this, "CurrentState", SetState);
            // After registering, we ask for current objects state and user
            MessengerInstance.Send("VM_Movie", "Context");
        }

        public string Info
        {
            get { return _info; }
            set {
                _info = value;
                RaisePropertyChanged("Info");
            }
        }

        private User _sessionUser;

        public User SessionUser
        {
            get { return _sessionUser; }
            set { _sessionUser = value; }
        }

        void SetSessionUser(User sessionUser)
        {
            SessionUser = sessionUser;
        }

        public Movie Movie
        {
            get { return _movie; }
            set {
                _movie = value;
                RaiseCUD("Movie");
                if (_movie != null)
                {
                    Hydrate(_movie.Id, _movie.Title, _movie.Type, _movie.Abstract, _movie.Comments);
                    MessengerInstance.Send("SetMovie", Movie);
                }
            }
        }

        public int Id {
            get { return Movie.Id; }
            set
            {
                Movie.Id = value;
                RaiseCUD("Id");
            }
        }

        public string Title {
            get { return Movie.Title; }
            set
            {
                Movie.Title = value;
                RaiseCUD("Title");
            }
        }

        public string Type
        {
            get { return Movie.Type; }
            set
            {
                Movie.Type = value;
                RaiseCUD("Type");
            }
        }

        public string Abstract
        {
            get { return Movie.Abstract; }
            set
            {
                Movie.Abstract = value;
                RaiseCUD("Abstract");
            }
        }

        public ICollection<Comment> Comments {
            get
            {
                return _comments;
            }
            set
            {
                _comments = value;
                RaisePropertyChanged("Comments");
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
        private void Hydrate(int _id, string _ti, string _ty, string _ab, ICollection<Comment> _co)
        {
            Id = _id;
            Title = _ti;
            Type = _ty;
            Abstract = _ab;
            Comments = _co;
        }

        void SetMovie(Movie movie)
        {
            Console.WriteLine("VM_Movie: Call to SetMovie, messenger callback");
            Console.WriteLine("Actual Movie in VM_Movie: " + Movie.ToString());
            if (movie == null)
            {
                Console.WriteLine("Received movie is null");
            }
            else
            {
                Console.WriteLine("VM_Movie: Callback for messenger with token CurrentMovie.");
                Movie = movie;
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
                Info = "VM_Movie: State set to " + State;
                RaisePropertyChanged("State");
            }
        }
        /**
         *  The state is a trivial state machine
         *  Allow us to manage the add/update context.
         * */
        public void SetState(STATE state)
        {
            Console.WriteLine("VM_Movie: Call to SetState, messenger callback in ");
            State = state;
        }

        /**
         * When state change, we send a request of Context to
         * User_State_Machine to received the updated state
         * We pull information
         * */
        public void HandleStateChange(STATE _)
        {
            MessengerInstance.Send("VM_Movie after received state_changed", "Context");
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
        protected Boolean IsValidMovie()
        {
            Console.WriteLine("Call on IsValidMovie");
            Boolean check = false;
            Info = "";
            if (Movie != null)
            {
                Console.WriteLine("IsValidMovie: Movie != null");
                check = (
                    ((Movie.Title != null && Movie.Title.Length != 0) || TriggerFormError("Title")) &&
                    ((Movie.Type != null && Movie.Type.Length != 0) || TriggerFormError("Type")) &&
                    ((Movie.Abstract != null && Movie.Abstract.Length != 0) || TriggerFormError("Abstract"))
                );
            }
            else { Console.WriteLine("IsValidMovie:  Movie is null"); }
            return check;
        }

        /**
         * Test if there is a selected movie
         * And if this movie exist in dao
         * */
        protected Boolean IsExistingMovie()
        {
            Console.WriteLine("Call on IsExistingMovie");
            if (Movie == null || Id == 0)
            {
                Info = "Movie is null or don't have ID";
                Console.WriteLine(Info);
                return false;
            }
            try
            {
                Movie check = mDao.GetMovie(Id);
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
            Form = new UpdateMovie();
        }

        /**
         *  Add Movie method used in Add RelayCommand
         *  With CanAdd validator
         * */
        void AddMovie()
        {
            Console.WriteLine("In Add Movie from VM_Movie");
            Info = "Adding a Movie";
            try
            {
                Movie checkMovie = CanAdd() ? mDao.CreateMovie(Movie) : null;
                if (checkMovie != null && checkMovie.Id > 0)
                {
                    Info = "Add happens well";
                    MessengerInstance.Send(STATE.ADD_MOVIE, "SetState");
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
                Info = "Adding Movie failed with exception: " + e.Message;
                Console.WriteLine(Info);
            }
        }

        /**
         *  Add RelayCommand Validator
         * */
        Boolean CanAdd()
        {
            Console.WriteLine("Call on CanAdd");
            bool check = IsValidMovie();
            Info = check ? "The actual movie is valid" : "The actual movie isn't valid: " + Info;
            return check;
        }

        /**
         * Method used in Update RelayCommand
         * With CanEdit validator
         * */
        void EditMovie()
        {
            Console.WriteLine("In Add User from VM_AddUser");
            try
            {
                Movie checkMovie = CanEdit() ? mDao.UpdateMovie(Movie) : null;
                if (checkMovie != null && checkMovie.Id > 0)
                {
                    Info = "Update happens well";
                    Console.WriteLine(Info);
                    MessengerInstance.Send(STATE.UPDATE_MOVIE, "SetState");
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
            bool check = IsValidMovie() && IsExistingMovie();
            Info = check ? "The actual user is valid" : "The actual user isn't valid" + Info;
            return check;
        }

        /**
         * Method used in Delete RelayCommand
         * With CanDelete validator
         * */
        void DeleteMovie()
        {
            Console.WriteLine("In DeleteMovie from VM_Movie");
            if (!CanDelete())
                return;
            Info = "Deleting a movie";
            try
            {
                if (mDao.DeleteMovie(Movie))
                {
                    Info = "DeleteMovie passed";
                    MessengerInstance.Send(STATE.DELETE_MOVIE, "SetState");
                    Movie = new Movie();
                }
                else
                    Info = "DeleteMovie failed";
                Console.WriteLine(Info);
            }
            catch (Exception e)
            {
                Info = "Update movie failed: " + e;
                Console.WriteLine("Exception PERSONNALISEE: " + e.Message);
            }
        }

        /**
         *  Delete RelayCommand Validator
         * */
        Boolean CanDelete()
        {
            return IsExistingMovie();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ModelMovieNet;
using ModelMovieNet.Factory;
using ModelMovieNet.Interface;
using static Movienet.State_Machine;

namespace Movienet
{
    /**
     * Handle Crud operation
     * */
    public class VM_User: ViewModelBase
    {
        public RelayCommand Add { get; set; } = null;
        public RelayCommand Update { get; set; } = null;
        public RelayCommand Delete { get; set; } = null;
        public RelayCommand Log { get; set; } = null;

        /**
         * RelayCommand to open editing form
         * */
        public RelayCommand GoToUpdate { get; set; }

        /**
         * Access to Dao, can be inherited
         * */
        private static IServiceFacade Services { get; } = ServiceFacadeFactory.GetServiceFacade();
        private static IUserDao uDao { get; } = Services.GetUserDao();

        /**
         * Private scope
         * */
        private User _user;
        private string _info;
        private Page _form;
        private STATE _state;
        private IDictionary<string, string> _errors { get; set; } = new Dictionary<string, string>()
        {
            { "Id", "Can't update or delete if id is 0" },
            { "Firstname", "This field can't be empty" },
            { "Lastname", "This field can't be empty" },
            { "Login", "This field can't be empty" },
            { "Password", "Must be 4 char" },
        };

        /**
         * Constructor
         * */
        public VM_User(): base()
        {
            // Entity
            User =  new User();
            // CUD
            Add = new RelayCommand(AddUser, CanAdd);
            Update = new RelayCommand(EditUser, CanEdit);
            Delete = new RelayCommand(DeleteUser, CanDelete);
            Log = new RelayCommand(LogUser, CanLog);
            // Navigation
            GoToUpdate = new RelayCommand(() => OpenUpdateForm());
            // Information
            Info = "Informations: ";
            /**
             * State synchronisation
             * */
            MessengerInstance.Register<STATE>(this, "state_changed", HandleStateChange);
            MessengerInstance.Register<User>(this, "CurrentUser", SetUser);
            MessengerInstance.Register<STATE>(this, "CurrentState", SetState);
            // After registering, we ask for current objects state and user
            MessengerInstance.Send("VM_User", "Context");
        }

        /**
         * Shortcut to inline User Props affectation
         * */
        private void Hydrate(int _id, string _f, string _l, string _lo, string _pa)
        {
            Id = _id;
            Firstname = _f;
            Lastname = _l;
            Login = _lo;
            Password = _pa;
        }

        /**
         * User definition
         * */
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                RaiseCUD("User");
                if (_user != null)
                {
                    Hydrate(_user.Id, _user.Firstname, _user.Lastname, _user.Login, _user.Password);
                    MessengerInstance.Send(User, "SetUser");
                }
            }
        }

        /**
         * Internal state
         * */
        public STATE State
        {
            get {
                return _state;
            }
            set {
                _state = value;
                Console.WriteLine("State set to: " + _state);
                Info = "VM_User: State set to " + State;
                RaisePropertyChanged("State");
            }
        }
        /**
         *  The state is a trivial state machine
         *  Allow us to manage the add/update context.
         * */
        public void SetState(STATE state)
        {
            Console.WriteLine("VM_User: Call to SetState, messenger callback in ");
            State = state;
        }

        /**
         * When state change, we send a request of Context to
         * User_State_Machine to received the updated state
         * We pull information
         * */
        public void HandleStateChange(STATE _)
        {
            MessengerInstance.Send("VM_User after received state_changed", "Context");
        }

        /**
         * User properties
         * */
        public int Id
        {
            get { return User.Id; }
            set {
                User.Id = value;
                RaiseCUD("Id");
            }
        }

        public string Firstname
        {
            get { return User.Firstname; }
            set {
                User.Firstname = value;
                RaiseCU("Firstname");
            }
        }

        public string Lastname
        {
            get { return User.Lastname; }
            set
            {
                User.Lastname = value;
                RaiseCU("Lastname");
            }
        }

        public string Login
        {
            get { return User.Login; }
            set
            {
                User.Login = value;
                RaiseCU("Login");
            }
        }

        public string Password
        {
            get { return User.Password; }
            set
            {
                User.Password = value;
                RaiseCU("Password");
            }
        }


        /*
         * Information display variable
         * */
        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;
                RaisePropertyChanged("Info");
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
         * Observer on user
         * */
        public void SetUser(User user)
        {
            Console.WriteLine("VM_User: Call to SetUser, messenger callback");
            Console.WriteLine("Actual User in VM_User: " + User.ToString());
            if (user == null)
            {
                Console.WriteLine("Received user is null");
            }
            else
            {
                Console.WriteLine("Will be replace by: " + user.ToString());
                User = user;
            }
        }

        /**
         *  This View Model know how to validate
         *  the object he represent.
         * */
        protected Boolean IsValidUser()
        {
            Boolean check = false;
            Info = "";
            if (User != null)
            {
                check = (
                    ((User.Firstname != null && User.Firstname.Length != 0) || TriggerFormError("Firstname")) &&
                    ((User.Lastname != null && User.Lastname.Length != 0) || TriggerFormError("Lastname")) &&
                    ((User.Login != null && User.Login.Length != 0) || TriggerFormError("Login")) &&
                    ((User.Password != null && User.Password.Length != 0) || TriggerFormError("Password")) &&
                    (User.Password.Length > 4 || TriggerFormError("Password"))
                    );
            }
            return check;
        }

        /**
         * Update information message with static _errors dictionnary
         * */
        private Boolean TriggerFormError(string prop)
        {
            Console.WriteLine("Trigger " + prop + " error: " + _errors[prop]);
            Info = "["+prop+"]: " + _errors[prop];
            return false;
        }

        /**
         * Test if there is a selected user
         * And if this user exist in dao
         * */
        protected Boolean IsExistingUser()
        {
            if (Id == 0)
            {
                return false;
            }
            try
            {
                User check = uDao.GetUser(Id);
                if (check != null && check.Id == Id)
                    return true;
                TriggerFormError("Id");
                return false;
            }
            catch (Exception e)
            {
                Info = "Testing user existence failed: " + e.Message;
                Console.WriteLine(Info);
                return false;
            }
        }

        /**
         *  Used in GoToUpdate RelayCommand
         *  Set the state to "update"
         * */
        void OpenUpdateForm()
        {
            MessengerInstance.Send(User, "SetUser");
            MessengerInstance.Send(STATE.UPDATE_USER, "SetState");
            Form = new UpdateUser();
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
            if (Delete != null)
                Delete.RaiseCanExecuteChanged();
            RaisePropertyChanged(prop);
        }
        //////////////////////////////////////////////////

        void LogUser()
        {
            Console.WriteLine("In Log User from VM_User");
            String Info = "Loggin a user";
            User checkUser = new User();
            Boolean logged = false;
            if (User.Login == null || User.Password == null)
            {
                Info = "Wrong infos for Login";
                Console.WriteLine(Info);
            }
            else
            {
                try
                {
                    checkUser = uDao.LogUser(User);
                    if (null == checkUser)
                        Info = "Log Failed";
                    else
                        logged = true;
                }
                catch (Exception e)
                {
                    Info = "Logging failed with exception: " + e.Message;
                }
            }
            Console.WriteLine("Send user: " + ((checkUser.Id > 0) ? checkUser.Login + " with id: " + checkUser.Id + " as session user" : "INVALID"));
            MessengerInstance.Send(checkUser, "SetSessionUser");
            Console.WriteLine("Info : " + Info);
        }

        bool CanLog()
        {
            return true;
        }

        /**
         *  Add user method used in Add RelayCommand
         *  With CanAdd validator
         * */
        void AddUser()
        {
            Console.WriteLine("In Add User from VM_User");
            Info = "Adding a user";
            try
            {
                User checkUser = CanAdd() ? uDao.CreateUser(User) : null;
                if (checkUser != null && checkUser.Id > 0)
                {
                    Info = "Add happens well";
                    MessengerInstance.Send(STATE.ADD_USER, "SetState");
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
                Info = "Adding user failed with exception: " + e.Message;
                Console.WriteLine(Info);
            }
        }

        /**
         *  Add RelayCommand Validator
         * */
        Boolean CanAdd()
        {
            bool check = IsValidUser();
            Info = check ? "The actual user is valid" : "The actual user isn't valid: " + Info;
            return check;
        }

        /**
         * Method used in Update RelayCommand
         * With CanEdit validator
         * */
        void EditUser()
        {
            Console.WriteLine("In Add User from VM_AddUser");
            try
            {
                User checkUser = CanEdit() ? uDao.UpdateUser(User) : null;
                if (checkUser != null && checkUser.Id > 0)
                {
                    Info = "Update happens well";
                    Console.WriteLine(Info);
                    MessengerInstance.Send(STATE.UPDATE_USER, "SetState");
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
            bool check = IsValidUser() && IsExistingUser();
            Info = check ? "The actual user is valid" : "The actual user isn't valid" + Info;
            return check;
        }

        /**
         * Method used in Delete RelayCommand
         * With CanDelete validator
         * */
        void DeleteUser()
        {
            Console.WriteLine("In DeleteUser from VM_User");
            if (!CanDelete())
                return;
            Info = "Deleting a user";
            try
            {
                if (uDao.DeleteUser(User))
                {
                    Info = "DeleteUser passed";
                    MessengerInstance.Send(STATE.DELETE_USER, "SetState");
                    User = new User();
                }
                else
                    Info = "DeleteUser failed";
                Console.WriteLine(Info);
            }
            catch (Exception e)
            {
                Info = "Update user failed: " + e;
                Console.WriteLine("Exception PERSONNALISEE: " + e.Message);
            }
        }

        /**
         *  Delete RelayCommand Validator
         * */
        Boolean CanDelete()
        {
            return IsExistingUser();
        }
    }
}

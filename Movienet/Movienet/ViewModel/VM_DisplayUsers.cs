using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using ModelMovieNet;
using ModelMovieNet.Factory;
using ModelMovieNet.Interface;
using static Movienet.State_Machine;

namespace Movienet
{
    /**
     * VM_DisplayUsers handle:
     * - the display of users in a list
     * - the selection of a user
     * It interact with VM_User and RootFrame
     * using User_State_Machine channels
     * */
    public class VM_DisplayUsers: ViewModelBase
    {
        // Entity
        private ObservableCollection<User> _users;

        /**
         * Access to Dao, can be inherited
         * */
        private static IServiceFacade Services { get; } = ServiceFacadeFactory.GetServiceFacade();
        private static IUserDao uDao { get; } = Services.GetUserDao();

        // Object used to pass a selected User
        private User _selectItem;

        // Information
        private string _listInfo;

        // State
        private STATE _state;

        /**
         * Constructor
         * */
        public VM_DisplayUsers()
        {
            ListInfo = "Informations: ";
            MessengerInstance.Register<STATE>(this, "CurrentState", StateChangedAck);
            MessengerInstance.Register<STATE>(this, "state_changed", StateChangedAck);
            try
            {
                Users = new ObservableCollection<User>(uDao.getAllUsers());
            } catch (Exception e)
            {
                ListInfo = ListInfo + e.Message;
                Console.WriteLine(e.ToString());
            }
            MessengerInstance.Send("VM_Display_Users ctor", "Context");
        }

        /**
         * Entity
         * */
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set {
                _users = value;
                RaisePropertyChanged("Users");
            }
        }

        /**
         * Information
         * */
        public string ListInfo
        {
            get { return _listInfo; }
            set {
                _listInfo = value;
                RaisePropertyChanged("ListInfo");
            }
        }

        /**
         * Object used to pass a selected User
         * Trigger two messages:
         * - the selected item of list on channel 'user'
         * - the state 'select' on channel 'state' (listened by parent VM_User)
         * */
        public User SelectItem
        {
            get { return _selectItem; }
            set {
                Console.WriteLine("VM_Display_users: SelectItem setter");
                _selectItem = value;
                ListInfo = "Item selected";
                MessengerInstance.Send(_selectItem, "SetUser");
                MessengerInstance.Send(STATE.SELECT_USER, "SetState");
                RaisePropertyChanged("SelectItem");
            }
        }

        /**
         * Listening on the VM_User state allow us
         * to reload the user list when an CUD operation
         * is performed from somewhere else.
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
                ListInfo = "VM_Display_Users state set to: " + _state;
            }
        }
        
        /** 
         * 'state_changed' and 'CurrentState' callback
         * */
        protected void StateChangedAck(STATE state)
        {
            Console.WriteLine("VM_Display_Users: VM_USER_STATE CHANGED");
            State = state;
            switch (state)
            {
                case STATE.ADD_USER:
                    ListInfo = "VM_Display_Users:  Handling ADD state";
                    Console.WriteLine(ListInfo);
                    RefreshUserList();
                    break;
                case STATE.SELECT_USER:
                    ListInfo = "VM_Display_Users:  Handling SELECT state";
                    Console.WriteLine(ListInfo);
                    break;
                case STATE.UPDATE_USER:
                    ListInfo = "VM_Display_Users:  Handling UPDATE state";
                    Console.WriteLine(ListInfo);
                    RefreshUserList();
                    break;
                case STATE.DELETE_USER:
                    ListInfo = "VM_Display_Users:  Handling DELETE state";
                    Console.WriteLine(ListInfo);
                    RefreshUserList();
                    break;
                default:
                    break;
            }

        }

        /**
         * Call Dao to update the user list
         * */
        private void RefreshUserList()
        {
            try
            {
                Users = new ObservableCollection<User>(uDao.getAllUsers());
            }
            catch (Exception e)
            {
                ListInfo = "Exception updating user list: " + e.Message;
                Console.WriteLine(ListInfo);
            }
        }
    }
}

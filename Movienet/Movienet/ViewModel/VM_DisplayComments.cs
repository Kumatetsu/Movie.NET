using GalaSoft.MvvmLight;
using ModelMovieNet;
using ModelMovieNet.Factory;
using ModelMovieNet.Interface;
using System;
using System.Collections.ObjectModel;
using static Movienet.State_Machine;

namespace Movienet
{
    public class VM_DisplayComments: ViewModelBase
    {
        private static IServiceFacade Services { get; } = ServiceFacadeFactory.GetServiceFacade();
        private static ICommentDao cDao { get; } = Services.GetCommentDao();
        private ObservableCollection<Comment> _comments;
        private string _info;
        private STATE _state;
        // Object used to pass a selected Comment
        private Comment _selectItem;

        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;
                RaisePropertyChanged("Info");
            }
        }

        public ObservableCollection<Comment> Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                RaisePropertyChanged("Comments");
            }
        }

        public VM_DisplayComments()
        {
            Info = "Informations: ";
            MessengerInstance.Register<STATE>(this, "CurrentState", StateChangedAck);
            MessengerInstance.Register<STATE>(this, "state_changed", StateChangedAck);
            try
            {
                Comments = new ObservableCollection<Comment>(cDao.getAllComments());
            }
            catch (Exception e)
            {
                Info = "Failed to load comments: " + e.Message;
                Console.WriteLine(Info);
            }
        }

        /**
         * Object used to pass a selected Comment
         * Trigger two messages:
         * - the selected item of list on channel 'SetComment'
         * - the state 'SELECT_COMMENT' on channel 'SetState' (listened by parent VM_Comment)
         * */
        public Comment SelectItem
        {
            get { return _selectItem; }
            set
            {
                if (value == null)
                {
                    Console.WriteLine("New value for SelectItem is null");
                }
                else
                {
                    Console.WriteLine("VM_DisplayComments: SelectItem setter");
                    _selectItem = value;
                    Info = "Item selected";
                    Console.WriteLine("DisplayComments: Set SelectItem with " + _selectItem.ToString());
                    MessengerInstance.Send(_selectItem, "SetComment");
                    MessengerInstance.Send(STATE.SELECT_COMMENT, "SetState");
                    RaisePropertyChanged("SelectItem");
                }
            }
        }

        /**
         * Listening on the VM_Comment state allow us
         * to reload the comment list when an CUD operation
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
                Info = "VM_DisplayComments state set to: " + _state;
            }
        }

        /** 
         * 'state_changed' and 'CurrentState' callback
         * */
        protected void StateChangedAck(STATE state)
        {
            Console.WriteLine("VM_DisplayComments: VM_STATE CHANGED");
            State = state;
            switch (state)
            {
                case STATE.ADD_COMMENT:
                    Info = "VM_DisplayComments:  Handling ADD_COMMENT state";
                    Console.WriteLine(Info);
                    RefreshCommentList();
                    break;
                case STATE.SELECT_COMMENT:
                    Info = "VM_DisplayComments:  Handling SELECT_COMMENT state";
                    Console.WriteLine(Info);
                    break;
                case STATE.UPDATE_COMMENT:
                    Info = "VM_DisplayComments:  Handling UPDATE_COMMENT state";
                    Console.WriteLine(Info);
                    RefreshCommentList();
                    break;
                case STATE.DELETE_COMMENT:
                    Info = "VM_DisplayComments:  Handling DELETE_COMMENT state";
                    Console.WriteLine(Info);
                    RefreshCommentList();
                    break;
                case STATE.SELECT_MOVIE:
                    Info = "VM_DisplayComments:  Handling SELECT MOVIE";
                    Console.WriteLine(Info);
                    RefreshCommentList();
                    break;
                default:
                    break;
            }

        }

        /**
         * Call Dao to update the comment list
         * */
        private void RefreshCommentList()
        {
            Console.WriteLine("Refresh comment list");
            try
            {
                Comments = new ObservableCollection<Comment>(cDao.getAllComments());
            }
            catch (Exception e)
            {
                Info = "Exception updating comment list: " + e.Message;
                Console.WriteLine(Info);
            }
        }
    }
}

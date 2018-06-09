using GalaSoft.MvvmLight;
using ModelMovieNet;
using ModelMovieNet.Factory;
using ModelMovieNet.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Movienet.State_Machine;

namespace Movienet
{
    public class VM_DisplayMovies: ViewModelBase
    {
        private static IServiceFacade Services { get; } = ServiceFacadeFactory.GetServiceFacade();
        private static IMovieDao mDao { get; } = Services.GetMovieDao();
        private ObservableCollection<Movie> _movies;
        private string _info;
        private STATE _state;
        private String _search;
        // Object used to pass a selected Movie
        private Movie _selectItem;

        public string search
        {
            get { return _search; }
            set
            {
                _search = value;
                RaisePropertyChanged("Search");
            }
        }

        public string Info
        {
            get { return _info; }
            set {
                _info = value;
                RaisePropertyChanged("Info");
            }
        }

        public ObservableCollection<Movie> Movies
        {
            get { return _movies; }
            set
            {
                _movies = value;
                RaisePropertyChanged("Movies");
            }
        }

        public VM_DisplayMovies()
        {
            Info = "Informations: ";
            MessengerInstance.Register<STATE>(this, "CurrentState", StateChangedAck);   
            MessengerInstance.Register<STATE>(this, "state_changed", StateChangedAck);
            try
            {
                Movies = new ObservableCollection<Movie>(mDao.getAllMovies());
            } catch (Exception e)
            {
                Info = "Failed to load movies: " + e.Message;
                Console.WriteLine(Info);
            }
        }

        /**
         * Object used to pass a selected Movie
         * Trigger two messages:
         * - the selected item of list on channel 'SetMovie'
         * - the state 'SELECT_MOVIE' on channel 'SetState' (listened by parent VM_Movie)
         * */
        public Movie SelectItem
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
                    Console.WriteLine("VM_DisplayMovies: SelectItem setter");
                    _selectItem = value;
                    Info = "Item selected";
                    Console.WriteLine("DisplayMovies: Set SelectItem with " + _selectItem.ToString());
                    MessengerInstance.Send(_selectItem, "SetMovie");
                    MessengerInstance.Send(STATE.SELECT_MOVIE, "SetState");
                    RaisePropertyChanged("SelectItem");
                }
            }
        }

        /**
         * Listening on the VM_Movie state allow us
         * to reload the movie list when an CUD operation
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
                Info = "VM_DisplayMovies state set to: " + _state;
            }
        }

        /** 
         * 'state_changed' and 'CurrentState' callback
         * */
        protected void StateChangedAck(STATE state)
        {
            Console.WriteLine("VM_DisplayMovies: VM_STATE CHANGED");
            State = state;
            switch (state)
            {
                case STATE.ADD_MOVIE:
                    Info = "VM_DisplayMovies:  Handling ADD state";
                    Console.WriteLine(Info);
                    RefreshMovieList();
                    break;
                case STATE.SELECT_MOVIE:
                    Info = "VM_DisplayMovies:  Handling SELECT state";
                    Console.WriteLine(Info);
                    break;
                case STATE.UPDATE_MOVIE:
                    Info = "VM_DisplayMovies:  Handling UPDATE state";
                    Console.WriteLine(Info);
                    RefreshMovieList();
                    break;
                case STATE.DELETE_MOVIE:
                    Info = "VM_DisplayMovies:  Handling DELETE state";
                    Console.WriteLine(Info);
                    RefreshMovieList();
                    break;
                default:
                    break;
            }

        }

        /**
         * Call Dao to update the movie list
         * */
        private void RefreshMovieList()
        {
            Console.WriteLine("Refresh movie list");
            try
            {
                Movies = new ObservableCollection<Movie>(mDao.getAllMovies());
            }
            catch (Exception e)
            {
                Info = "Exception updating movie list: " + e.Message;
                Console.WriteLine(Info);
            }
        }
    }
}

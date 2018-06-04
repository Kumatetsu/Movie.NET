using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ModelMovieNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Movienet
{
    public class VM_MainWindow: State_Machine
    {
        private User sessionUser;
        private Page _rootPage;
        public Page RootPage
        {
            get
            {
                return _rootPage;
            }
            set
            {
                _rootPage = value;
                RaisePropertyChanged("RootPage");
            }
        }

        private string _info;

        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }


        public RelayCommand OpenAuthentication { get; set; }
        public RelayCommand OpenUserManagement { get; set; }
        public RelayCommand OpenMovieManagement { get; set; }
        /**
         * Constructor
         * */
        public VM_MainWindow()
        {
            Console.WriteLine("Defining DataDirectory");
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            int index = baseDir.IndexOf("Movienet");
            string dataDir = baseDir.Substring(0, index) + "Movienet\\ModelMovieNet\\";
            Console.WriteLine("dataDir: " + dataDir);
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);
            Console.WriteLine("Buidling RootPage");
            MessengerInstance.Register<User>(this, "SessionUser", SetSessionUser);
            OpenAuthentication = new RelayCommand(() => GoToAuthentication());
            OpenUserManagement = new RelayCommand(() => GoToUserManagement());
            OpenMovieManagement = new RelayCommand(() => GoToMovieManagement());
            RootPage = new Authentication();
        }

        public User SessionUser
        {
            get { return sessionUser; }
            set {
                sessionUser = value;
            }
        }


        void SetSessionUser(User user)
        {
            Console.WriteLine("VM_MainWindow: SetSession User.");
            if (user != null)
            {
                Console.WriteLine("VM_MainWindow: rootFrame");
                RootPage = new RootFrame();
            }
            else
            {
                Console.WriteLine("VM_MainWindow: authentication");
                RootPage = new Authentication();
            }
        }

        /**
         * Display an Authentication view on Detail page
         * */
        private void GoToAuthentication()
        {
            User u = new User();
            MessengerInstance.Send(u, "SetUser");
            MessengerInstance.Send(STATE.NEED_AUTHENTICATION, "SetState");
            RootPage = new Authentication();
            Info = "MainWindow set context to Authentication and user to new one";
            Console.WriteLine(Info);
        }

        /**
         * Display an Authentication view on Detail page
         * */
        private void GoToUserManagement()
        {
            RootPage = new RootFrame();
            Info = "MainWindow set rootFrame for user management";
            Console.WriteLine(Info);
        }

        /**
         * Display an Authentication view on Detail page
         * */
        private void GoToMovieManagement()
        {
            RootPage = new MovieRootFrame();
            Info = "MainWindow set movieRootFrame for movie management";
            Console.WriteLine(Info);
        }
    }
}

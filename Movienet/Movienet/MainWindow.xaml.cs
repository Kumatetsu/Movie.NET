using Movienet.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Movienet
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            root.Content = new Authentication();
        }

        // évênements onClick définis dans les buttons
        private void goToDisplayUser(object sender, RoutedEventArgs e)
        {
            // On retrouve l'élément root (la Frame dans la window)
            // on lui set le VVM User
            root.Content = new DisplayUsers();
        }

        private void goToAddUser(object sender, RoutedEventArgs e)
        {
            root.Content = new AddUser();
        }

        private void goToAuthenticate(object sender, RoutedEventArgs e)
        {
            root.Content = new Authentication();
        }

        private void goToFilms(object sender, RoutedEventArgs e)
        {

        }

        private void goToAddFilm(object sender, RoutedEventArgs e)
        {

        }
    }
}

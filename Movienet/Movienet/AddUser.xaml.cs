﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary1;
using ClassLibrary1.Factory;
using ClassLibrary1.Interface;

namespace Movienet
{
    /// <summary>
    /// Logique d'interaction pour AddUser.xaml
    /// </summary>
    public partial class AddUser : Page
    {
        public AddUser()
        {
            IServiceFacade Services = ServiceFacadeFactory.GetServiceFacade();
            IUserDao uDao = Services.GetUserDao();
            String info = "Adding a user";
            ClassLibrary1.User checkUser = new User();
            ClassLibrary1.User nUser = new User();
            nUser.firstname = "Bill";
            nUser.lastname = "Boket";
            nUser.login = "BillBoket";
            nUser.password = "TheHole";
            try
            {
                checkUser = uDao.CreateUser(nUser);
            } catch (Exception e)
            {
                info = "Add user failed: " + e;
            }
            if (checkUser.Id > 0)
            {
                info = "Everything happens well";
            }
            if (Info != null)
                Info.Text = Info.Text + " Adding result: " + info;
            InitializeComponent();
        }
    }
}

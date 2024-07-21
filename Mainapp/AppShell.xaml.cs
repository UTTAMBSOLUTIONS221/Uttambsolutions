﻿using Mainapp.Pages;
using Mainapp.Pages.Users;
namespace Mainapp
{
    public partial class AppShell : Shell
    {
        public AppShell(LoginPage loginPage)
        {
            InitializeComponent();

            Routing.RegisterRoute("DashBoardPage", typeof(DashBoardPage));

            this.CurrentItem = loginPage;
        }
    }
}

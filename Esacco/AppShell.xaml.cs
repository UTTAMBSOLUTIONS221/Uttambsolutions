﻿using Esacco.Views;

namespace Esacco
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            Routing.RegisterRoute(nameof(SaccoAdministratorPage), typeof(SaccoAdministratorPage));
            Routing.RegisterRoute(nameof(SaccoDriverPage), typeof(SaccoDriverPage));
            Routing.RegisterRoute(nameof(SaccoEquipmentPage), typeof(SaccoEquipmentPage));
        }
    }
}

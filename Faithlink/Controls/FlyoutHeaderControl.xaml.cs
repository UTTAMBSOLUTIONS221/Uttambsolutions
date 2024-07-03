namespace Faithlink.Controls;

public partial class FlyoutHeaderControl : StackLayout
{
    public FlyoutHeaderControl()
    {
        InitializeComponent();

        if (App.UserDetails != null)
        {
            lblUserName.Text = App.UserDetails.Fullname;
            lblUserEmail.Text = App.UserDetails.Emailaddress;
            lblUserRole.Text = App.UserDetails.Rolename;
        }
    }
}
namespace Mainapp.Controls;

public partial class FlyoutHeaderControl : StackLayout
{
    public FlyoutHeaderControl()
    {
        InitializeComponent();

        if (App.UserDetails != null)
        {
            lblUserName.Text = App.UserDetails.Fullname;
            //lblUserRole.Text = App.UserDetails.Rolename;
            lblMemberNumber.Text = "Member No#";
            lblMemberNo.Text = "120";
        }
    }
}
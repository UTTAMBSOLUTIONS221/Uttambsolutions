namespace Mainapp.Controls;

public partial class FlyoutHeaderControl : StackLayout
{
    public FlyoutHeaderControl()
    {
        InitializeComponent();

        if (App.UserDetails != null)
        {
            lblUserName.Text = App.UserDetails.Fullname;
            // lblUserRole.Text = App.UserDetails.Rolename; // Uncomment and use if needed
            lblMemberNumber.Text = "Member No#";
            //lblMemberNo.Text = App.UserDetails.MemberNo.ToString();
            lblMemberNo.Text = "120";
        }
    }
}
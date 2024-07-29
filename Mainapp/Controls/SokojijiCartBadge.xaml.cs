namespace Mainapp.Controls;

public partial class SokojijiCartBadge : ContentView
{
    public SokojijiCartBadge()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty CartItemCountProperty =
        BindableProperty.Create(nameof(CartItemCount), typeof(int), typeof(SokojijiCartBadge), 0);

    public int CartItemCount
    {
        get => (int)GetValue(CartItemCountProperty);
        set => SetValue(CartItemCountProperty, value);
    }
}

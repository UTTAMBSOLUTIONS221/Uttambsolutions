﻿using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Parceldrop
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set the status bar color
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var window = Platform.CurrentActivity.Window; // Get the current activity's window
                window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#ed7a17")); // Change this to your desired color
            }
        }
    }
}

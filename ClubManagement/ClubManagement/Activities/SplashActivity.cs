using Android.App;
using Android.OS;
using Android.Preferences;
using ClubManagement.Ultilities;

namespace ClubManagement.Activities
{
    [Activity(Label = "SplashActivity", MainLauncher = true, NoHistory = true, Theme = "@style/AppTheme")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var isLogged = PreferenceManager.GetDefaultSharedPreferences(Application.Context)
                .GetBoolean(AppConstantValues.LogStatusPreferenceKey, false);
            StartActivity(isLogged ? typeof(MainActivity) : typeof(LoginActivity));
            Finish();
        }
    }
}
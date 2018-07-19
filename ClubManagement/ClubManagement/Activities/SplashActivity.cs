using System.Linq;
using Android.App;
using Android.OS;
using Android.Preferences;
using ClubManagement.Controllers;
using ClubManagement.Ultilities;

namespace ClubManagement.Activities
{
    [Activity(Label = "SplashActivity", MainLauncher = true, NoHistory = true, Theme = "@style/AppTheme")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var preferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            var isLogged = preferences.GetBoolean(AppConstantValues.LogStatusPreferenceKey, false);
            if (isLogged)
            {
                StartActivity(typeof(MainActivity));
                Finish();
                var userId = preferences.GetString(AppConstantValues.UserIdPreferenceKey, string.Empty);
                var currentUser = UsersController.Instance.Values.First(u => u.Id == userId);
                UsersController.Instance.UpdateUserLocation(currentUser);
            }
            else
            {
                StartActivity(typeof(LoginActivity));
            }
        }
    }
}
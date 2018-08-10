using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Preferences;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Ultilities;
using Plugin.Connectivity;

namespace ClubManagement.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = true, NoHistory = true, Theme = "@style/AppTheme")]
    public class SplashActivity : Activity
    {
        [InjectOnClick(Resource.Id.btnRetry)]
        private void Retry(object s, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.no_internet_connection), ToastLength.Short).Show();
                return;
            }
            StartApp();
        }

        private void StartApp()
        {
            var preferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            var isLogged = preferences.GetBoolean(AppConstantValues.LogStatusPreferenceKey, false);
            if (isLogged)
            {
                StartActivity(typeof(MainActivity));
                Finish();
                var userId = preferences.GetString(AppConstantValues.UserIdPreferenceKey, string.Empty);
                var currentUser = UsersController.Instance.Values.First(u => u.Id == userId);
                AppDataController.Instance.UpdateUser();
                UsersController.Instance.UpdateUserLocation(currentUser);
            }
            else
            {
                StartActivity(typeof(LoginActivity));
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (!CrossConnectivity.Current.IsConnected)
            {
                SetContentView(Resource.Layout.activity_no_internet);
                Cheeseknife.Inject(this);
                return;
            }
            StartApp();
        }
    }
}
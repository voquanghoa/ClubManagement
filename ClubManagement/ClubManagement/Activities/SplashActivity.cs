using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Preferences;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Ultilities;
using Plugin.Connectivity;
using Plugin.CurrentActivity;

namespace ClubManagement.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/AppTheme")]
    public class SplashActivity : Activity
    {
        [InjectOnClick(Resource.Id.btnRetry)]
        private void Retry(object s, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
				this.ShowMessage(Resource.String.no_internet_connection);
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
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);
            if (!CrossConnectivity.Current.IsConnected)
            {
                SetContentView(Resource.Layout.activity_no_internet);
                Cheeseknife.Inject(this);
                return;
            }

            StartApp();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current
                .OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
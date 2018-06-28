using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;

namespace ClubManagement.Activities
{
    [Activity(Label = "LoginActivity", MainLauncher = true, Theme = "@style/AppTheme")]
    public class LoginActivity : Activity
    {
        private readonly UsersController usersController = UsersController.Instance;

        [InjectView(Resource.Id.edtEmail)] private EditText edtEmail;

        [InjectView(Resource.Id.edtPassword)] private EditText edtPassword;

        [InjectOnClick(Resource.Id.btnSignIn)]
        private void SignIn(object s, EventArgs e)
        {
            if (string.IsNullOrEmpty(edtEmail.Text) || string.IsNullOrEmpty(edtPassword.Text))
            {
                Toast.MakeText(this, "Please fill all the fields", ToastLength.Short).Show();
                return;
            }

            var users = usersController.Values;

            if (!users.Select(x => x.Email).Contains(edtEmail.Text))
            {
                Toast.MakeText(this, "Email not exist!", ToastLength.Short).Show();
                return;
            }

            if (users.First(x => x.Email == edtEmail.Text).Password == edtPassword.Text)
            {
                Toast.MakeText(this, "Login successfully!", ToastLength.Short).Show();
                PreferenceManager.GetDefaultSharedPreferences(Application.Context).Edit().PutBoolean("IsLogged", true);
                StartActivity(typeof(MainActivity));
                return;
            }

            Toast.MakeText(this, "Wrong email or password", ToastLength.Short).Show();
        }

        [InjectOnClick(Resource.Id.btnSignUp)]
        private void SignUp(object s, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);
            Cheeseknife.Inject(this);
        }
    }
}
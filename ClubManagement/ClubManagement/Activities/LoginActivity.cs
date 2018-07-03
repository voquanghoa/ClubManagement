using System;
using System.Linq;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Preferences;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;

namespace ClubManagement.Activities
{
    [Activity(Label = "LoginActivity", Theme = "@style/AppTheme")]
    public class LoginActivity : Activity
    {
        private readonly UsersController usersController = UsersController.Instance;

        [InjectView(Resource.Id.edtEmail)] private EditText edtEmail;

        [InjectView(Resource.Id.edtPassword)] private EditText edtPassword;

        [InjectOnClick(Resource.Id.btnSignIn)]
        private void SignIn(object s, EventArgs e)
        {
            AppUltilities.HideKeyboard(this);
            if (string.IsNullOrEmpty(edtEmail.Text) || string.IsNullOrEmpty(edtPassword.Text))
            {
                Toast.MakeText(this, "Please fill all the fields", ToastLength.Short).Show();
                return;
            }

            var dialog = DialogExtensions.CreateDialog("Sign in", "Please wait...", this);
            dialog.Show();
            new Thread(() =>
            {
                var users = usersController.Values;

                if (!users.Select(x => x.Email).Contains(edtEmail.Text))
                {
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, "Email not exist!", ToastLength.Short).Show();
                        dialog.Dismiss();
                    });
                    return;
                }
                if (users.Any(u => u.Email == edtEmail.Text && u.Password == edtPassword.Text))
                {
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, "Login successfully!", ToastLength.Short).Show();
                        dialog.Dismiss();
                    });
                    var user = users.First(u => u.Email == edtEmail.Text && u.Password == edtPassword.Text);
                    var preferencesEditor = PreferenceManager.GetDefaultSharedPreferences(Application.Context).Edit();
                    preferencesEditor.PutBoolean("IsLogged", true);
                    preferencesEditor.PutString("UserId", user.Id);
                    preferencesEditor.Commit();
                    StartActivity(typeof(MainActivity));
                    return;
                }

                RunOnUiThread(() =>
                {
                    dialog.Dismiss();
                    Toast.MakeText(this, "Wrong email or password", ToastLength.Short).Show();
                });
            }).Start();
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
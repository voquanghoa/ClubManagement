﻿using System;
using System.Linq;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Preferences;
using Android.Widget;
using ClubManagement.Controllers;
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
            this.HideKeyboard();

            if (string.IsNullOrEmpty(edtEmail.Text) || string.IsNullOrEmpty(edtPassword.Text))
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.fill_all_fields), ToastLength.Short).Show();
                return;
            }

            var dialog = DialogExtensions.CreateDialog(Resources.GetString(Resource.String.sign_in), Resources.GetString(Resource.String.wait), this);
            dialog.Show();
			var activity = this;

            new Thread(() =>
            {
                try{
                    var users = usersController.Values;

					var loginUser = users.FirstOrDefault(x => string.Equals(x.Email, edtEmail.Text, StringComparison.CurrentCultureIgnoreCase));

					if (loginUser == null)
                    {
                        RunOnUiThread(() =>
                        {
							Toast.MakeText(activity, Resources.GetString(Resource.String.not_exist_email), ToastLength.Short).Show();
                            dialog.Dismiss();
                        });
                        return;
                    }
					if (loginUser.Password == edtPassword.Text)
                    {
                        var preferencesEditor = PreferenceManager.GetDefaultSharedPreferences(Application.Context).Edit();
                        preferencesEditor.PutBoolean(AppConstantValues.LogStatusPreferenceKey, true);
						preferencesEditor.PutString(AppConstantValues.UserIdPreferenceKey, loginUser.Id);
                        preferencesEditor.Commit();
                        Finish();
                        StartActivity(typeof(MainActivity));
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, Resources.GetString(Resource.String.login_success), ToastLength.Short).Show();
                            dialog.Dismiss();
                        });
						usersController.UpdateUserLocation(loginUser);
                        return;
                    }
                    RunOnUiThread(() =>
                    {
                        dialog.Dismiss();
						Toast.MakeText(activity, Resources.GetString(Resource.String.wrong_email), ToastLength.Short).Show();
                    });
                }
                catch (Exception)
                {
					Toast.MakeText(activity, Resources.GetString(Resource.String.no_internet_connection), ToastLength.Short).Show();
                }
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
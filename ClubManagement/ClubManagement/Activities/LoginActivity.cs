using System;
using System.Collections.Generic;
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
            this.HideKeyboard();

            if (string.IsNullOrEmpty(edtEmail.Text) || string.IsNullOrEmpty(edtPassword.Text))
            {
				this.ShowMessage(Resource.String.fill_all_fields);
                return;
            }

            var dialog = this.CreateDialog(Resources.GetString(Resource.String.sign_in), Resources.GetString(Resource.String.wait));
            dialog.Show();
			var activity = this;

            var users = new List<UserModel>();
            this.DoRequest(() => users = usersController.Values, () =>
            {
                var loginUser = users.FirstOrDefault(x =>
                    string.Equals(x.Email, edtEmail.Text, StringComparison.CurrentCultureIgnoreCase));

                if (loginUser == null)
                {
					this.ShowMessage(Resource.String.not_exist_email);
                    dialog.Dismiss();
                    return;
                }

                if (loginUser.Password == edtPassword.Text)
                {
                    var preferencesEditor =
                        PreferenceManager.GetDefaultSharedPreferences(Application.Context).Edit();
                    preferencesEditor.PutBoolean(AppConstantValues.LogStatusPreferenceKey, true);
                    preferencesEditor.PutString(AppConstantValues.UserIdPreferenceKey, loginUser.Id);
                    preferencesEditor.PutString(AppConstantValues.UserAvatarUrl, loginUser.Avatar);
                    preferencesEditor.Commit();
                    Finish();

                    AppDataController.Instance.UpdateUser();

                    StartActivity(typeof(MainActivity));
					this.ShowMessage(Resource.String.login_success);
                    dialog.Dismiss();
                    this.DoRequest(() => usersController.UpdateUserLocation(loginUser));
                    return;
                }

				this.ShowMessage(Resource.String.wrong_email);
                dialog.Dismiss();
            }, () => dialog.Dismiss());
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;

namespace ClubManagement.Activities
{
    [Activity(Label = "RegisterActivity", Theme = "@style/AppTheme")]
    public class RegisterActivity : Activity
    {
        private readonly UsersController usersController = UsersController.Instance;

        [InjectView(Resource.Id.edtEmail)] private EditText edtEmail;

        [InjectView(Resource.Id.edtName)] private EditText edtName;

        [InjectView(Resource.Id.edtPassword)] private EditText edtPassword;

        [InjectView(Resource.Id.edtConfirmPassword)]
        private EditText edtConfirmPassword;

        [InjectOnClick(Resource.Id.btnSignIn)]
        private void SignIn(object s, EventArgs e)
        {
            Finish();
        }

        [InjectOnClick(Resource.Id.btnSignUp)]
        private void SignUp(object s, EventArgs e)
        {
            this.HideKeyboard();

            edtEmail.Text = edtEmail.Text.ToLower().Trim();
            edtName.Text = new Regex("[ ]{2,}]").Replace(edtName.Text, " ").Trim();

            if (string.IsNullOrEmpty(edtEmail.Text) || string.IsNullOrEmpty(edtName.Text) ||
                string.IsNullOrEmpty(edtPassword.Text) || string.IsNullOrEmpty(edtConfirmPassword.Text))
            {
				this.ShowMessage(Resource.String.fill_all_fields);
                return;
            }

            if (edtPassword.Text.Length < 6)
            {
				this.ShowMessage(Resource.String.password_too_short);
                return;
            }

            if (!edtEmail.Text.IsValidEmailFormat())
            {
				this.ShowMessage(Resource.String.invalid_email_format);
                return;
            }

            if (!edtName.Text.IsValidNameFormat())
            {
				this.ShowMessage(Resource.String.invalid_name_format);
                return;
            }

            if (edtConfirmPassword.Text != edtPassword.Text)
            {
				this.ShowMessage(Resource.String.not_match_pass);
                return;
            }

            var dialog = this.CreateDialog(Resources.GetString(Resource.String.sign_up), Resources.GetString(Resource.String.wait));
            dialog.Show();

            var userEmails = new List<string>();
            this.DoRequest(() => userEmails = usersController.Values.Select(x => x.Email).ToList(), () =>
            {
                if (userEmails.Contains(edtEmail.Text))
                {
					this.ShowMessage(Resource.String.exist_email);
                    dialog.Dismiss();
                    return;
                }

                this.DoRequest(() =>
                {
                    usersController.Add(new UserModel
                    {
                        Email = edtEmail.Text,
                        Name = edtName.Text,
                        Password = edtPassword.Text,
                        CreatedTime = DateTime.Now
                    });
                }, () =>
                {
					this.ShowMessage(Resource.String.signup_success);
                    dialog.Dismiss();
                    Finish();
                });
            }, ()=> dialog.Dismiss());
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_register);
            Cheeseknife.Inject(this);
        }
    }
}
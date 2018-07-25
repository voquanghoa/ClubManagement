﻿using System;
using System.Linq;
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
            if (string.IsNullOrEmpty(edtEmail.Text) || string.IsNullOrEmpty(edtName.Text) ||
                string.IsNullOrEmpty(edtPassword.Text) || string.IsNullOrEmpty(edtConfirmPassword.Text))
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.fill_all_fields), ToastLength.Short).Show();
                return;
            }
            if (edtConfirmPassword.Text != edtPassword.Text)
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.not_match_pass), ToastLength.Short).Show();
                return;
            }

            var dialog = DialogExtensions.CreateDialog(Resources.GetString(Resource.String.sign_up), Resources.GetString(Resource.String.wait), this);
            dialog.Show();

            new Thread(() =>
            {
                try
                {
                    var userEmails = usersController.Values.Select(x => x.Email).ToList();

                    if (userEmails.Contains(edtEmail.Text))
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, Resources.GetString(Resource.String.exist_email), ToastLength.Short)
                                .Show();
                            dialog.Dismiss();
                        });
                        return;
                    }

                    usersController.Add(new UserModel
                    {
                        Email = edtEmail.Text,
                        Name = edtName.Text,
                        Password = edtPassword.Text
                    });
                }
                catch (Exception)
                {
                    Toast.MakeText(this, Resources.GetString(Resource.String.no_internet_connection), ToastLength.Short)
                        .Show();
                    return;
                }

                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, Resources.GetString(Resource.String.signup_success), ToastLength.Short).Show();
                    dialog.Dismiss();
                });
                Finish();
            }).Start();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_register);
            Cheeseknife.Inject(this);
        }
    }
}
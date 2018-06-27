using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace ClubManagement.Activities
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
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
            //sign up here
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_register);
            Cheeseknife.Inject(this);
        }
    }
}
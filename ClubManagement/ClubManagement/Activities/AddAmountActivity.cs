using System;
using Android.App;
using Android.OS;
using ClubManagement.Ultilities;

namespace ClubManagement.Activities
{
    [Activity(Label = "AddAmountActivity")]
    public class AddAmountActivity : Activity
    {
        [InjectOnClick(Resource.Id.btnBack)]
        private void Back(object s, EventArgs e)
        {
            Finish();
        }

        [InjectOnClick(Resource.Id.btnDelete)]
        private void Delete(object s, EventArgs e)
        {
            this.ShowMessage("sadasdas");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_add_amount);
            Cheeseknife.Inject(this);
        }
    }
}
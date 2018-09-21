
using Android.App;
using Android.OS;
using ClubManagement.Activities.Base;
using ClubManagement.Ultilities;
using System;

namespace ClubManagement.Activities
{
    [Activity(Label = "CreateFeeActivity")]
    public class CreateFeeActivity : CreateOrEditFeeActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            edtDeadline.Text = DateTime.Now.ToDateString();
        }
    }
}
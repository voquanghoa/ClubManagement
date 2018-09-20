using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Gms.Location.Places;
using ClubManagement.Ultilities;
using Android.Gms.Location.Places.UI;
using ClubManagement.Fragments;
using ClubManagement.Controllers;
using ClubManagement.Models;
using Square.Picasso;
using Android.Text;
using Newtonsoft.Json;

namespace ClubManagement.Activities.Base
{
    public class CreateOrEditFeeActivity : Activity
    {
        [InjectOnClick(Resource.Id.btnCancel)]
        protected void Cancel(object sender, EventArgs e)
        {
            this.ShowConfirmDialog(
                Resource.String.create_fee_cancel_title,
                Resource.String.cross_create_event_message,
                Finish,
                () => { }).Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityCreateFee);
            Cheeseknife.Inject(this);
        }
    }
}
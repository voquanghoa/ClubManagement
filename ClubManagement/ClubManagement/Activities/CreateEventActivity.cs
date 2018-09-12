using System;
using Android.App;
using Android.Content;
using Android.OS;
using ClubManagement.Activities.Base;
using Android.Widget;
using Android.Views;

namespace ClubManagement.Activities
{
    [Activity(Label = "CreateEventActivity")]
    public class CreateEventActivity : CreateOrEditEventActivity
    {
        [InjectView(Resource.Id.btnCross)]
        private ImageButton btnCross;

        public CreateEventActivity()
        {
            startTime = DateTime.Now;
            endTime = DateTime.Now.AddHours(1);
        }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            btnCross.Visibility = ViewStates.Visible;

            UpdateView();
        }
    }
}
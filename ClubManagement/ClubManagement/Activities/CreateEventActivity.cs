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
    public class CreateEventActivity : Activity
    {
        [InjectView(Resource.Id.btnCross)]
        private ImageButton btnCross;

        private CreateOrEditEvent createOrEditEvent = new CreateOrEditEvent();

        public CreateEventActivity()
        {
            createOrEditEvent.StartTime = DateTime.Now;
            createOrEditEvent.EndTime = DateTime.Now.AddHours(1);
        }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var view = LayoutInflater.Inflate(Resource.Layout.activity_add_event, null);

            SetContentView(view);

            Cheeseknife.Inject(this);

            btnCross.Visibility = ViewStates.Visible;

            createOrEditEvent.OnCreate(savedInstanceState, this, view);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            createOrEditEvent.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
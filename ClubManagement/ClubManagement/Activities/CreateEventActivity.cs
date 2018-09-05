using System;
using Android.App;
using Android.OS;
using ClubManagement.Ultilities;

namespace ClubManagement.Activities
{
    [Activity(Label = "CreateEventActivity")]
    public class CreateEventActivity : Activity
    {
        [InjectOnClick(Resource.Id.btnDone)]
        private void Done(object sender, EventArgs e)
        {
            // add event
            SetResult(Result.Ok);
            Finish();
        }

        [InjectOnClick(Resource.Id.btnCross)]
        private void Cross(object sender, EventArgs e)
        {
            this.ShowConfirmDialog(
                Resource.String.cross_create_event_title, 
                Resource.String.cross_create_event_message,
                Finish,
                () => { }).Show();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_add_event);
            Cheeseknife.Inject(this);
        }
    }
}
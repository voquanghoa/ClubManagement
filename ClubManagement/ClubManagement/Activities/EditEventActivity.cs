
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using ClubManagement.Activities.Base;
using ClubManagement.Models;
using Newtonsoft.Json;

namespace ClubManagement.Activities
{
    [Activity(Label = "EditEventActivity")]
    public class EditEventActivity : Activity
    {
        [InjectView(Resource.Id.tvTitle)]
        private TextView tvTitle;

        [InjectView(Resource.Id.btnCancel)]
        private Button btnCancel;

        [InjectView(Resource.Id.btnDone)]
        private Button btnDone;

        private CreateOrEditEvent createOrEditEvent = new CreateOrEditEvent();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var view = LayoutInflater.Inflate(Resource.Layout.activity_add_event, null);

            SetContentView(view);

            Cheeseknife.Inject(this);

            btnCancel.Visibility = ViewStates.Visible;
            tvTitle.Gravity = GravityFlags.Center;
            tvTitle.Text = GetString(Resource.String.edit_event);
            btnDone.Text = GetString(Resource.String.button_ok);

            createOrEditEvent.OnCreate(savedInstanceState, this, view);

            var content = Intent.GetStringExtra("EventDetail");
            var eventDetail = JsonConvert.DeserializeObject<UserLoginEventModel>(content);
            createOrEditEvent.Event = eventDetail;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            createOrEditEvent.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
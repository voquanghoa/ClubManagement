
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
    public class EditEventActivity : CreateOrEditEventActivity
    {
        [InjectView(Resource.Id.tvTitle)]
        private TextView tvTitle;

        [InjectView(Resource.Id.btnCancel)]
        private Button btnCancel;

        [InjectView(Resource.Id.btnDone)]
        private Button btnDone;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            btnCancel.Visibility = ViewStates.Visible;
            tvTitle.Gravity = GravityFlags.Center;
            tvTitle.Text = GetString(Resource.String.edit_event);
            btnDone.Text = GetString(Resource.String.button_ok).ToUpper();

            var content = Intent.GetStringExtra("EventDetail");
            var eventDetail = JsonConvert.DeserializeObject<UserLoginEventModel>(content);
            Event = eventDetail;
        }
    }
}
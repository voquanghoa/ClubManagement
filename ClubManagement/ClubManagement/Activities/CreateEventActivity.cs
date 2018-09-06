using System;
using Android.App;
using Android.Content;
using Android.Gms.Location.Places;
using Android.Gms.Location.Places.UI;
using Android.OS;
using Android.Widget;
using ClubManagement.Ultilities;

namespace ClubManagement.Activities
{
    [Activity(Label = "CreateEventActivity")]
    public class CreateEventActivity : Activity
    {
        private IPlace place;

        [InjectView(Resource.Id.edtEventTitle)] private EditText edtEventTitle;

        [InjectView(Resource.Id.tvStartDate)] private TextView tvStartDate;

        [InjectView(Resource.Id.tvStartTime)] private TextView tvStartTime;

        [InjectView(Resource.Id.tvEndDate)] private TextView tvEndDate;

        [InjectView(Resource.Id.tvEndTime)] private TextView tvEndTime;

        [InjectView(Resource.Id.edtChooseLocation)]
        private EditText edtChooseLocation;

        [InjectView(Resource.Id.edtEventLocation)]
        private EditText edtEventLocation;

        [InjectView(Resource.Id.edtEventDescription)]
        private EditText edtEventDescription;

        private const int PlacePickerRequset = 1;

        [InjectOnClick(Resource.Id.btnChangeEventImage)]
        private void ChangeEventImage(object sender, EventArgs e)
        {
            // show pop up menu
        }

        [InjectOnClick(Resource.Id.pickStartTime)]
        private void PickStartTime(object sender, EventArgs e)
        {
            // show date time picker dialog 
            Toast.MakeText(this, "sss", ToastLength.Short).Show();

        }

        [InjectOnClick(Resource.Id.pickEndTime)]
        private void PickEndTime(object sender, EventArgs e)
        {
            // show date time picker dialog
            Toast.MakeText(this, "sss", ToastLength.Short).Show();
        }

        [InjectOnClick(Resource.Id.btnResetEndTime)]
        private void DeleteEndTime(object sender, EventArgs e)
        {
            tvEndDate.Text = string.Empty;
            tvEndTime.Text = string.Empty;
        }

        [InjectOnClick(Resource.Id.edtChooseLocation)]
        private void ChooseLocation(object sender, EventArgs e)
        {
            var builder = new PlacePicker.IntentBuilder();
            StartActivityForResult(builder.Build(this), PlacePickerRequset);
        }

        [InjectOnClick(Resource.Id.btnDone)]
        private void Done(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(edtChooseLocation.Text) ||
                string.IsNullOrEmpty(edtEventTitle.Text) ||
                string.IsNullOrEmpty(tvStartDate.Text) ||
                string.IsNullOrEmpty(edtEventLocation.Text) ||
                string.IsNullOrEmpty(tvStartTime.Text))
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.fill_all_fields), ToastLength.Short).Show();
                return;
            }
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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode != PlacePickerRequset || resultCode != Result.Ok) return;
            place = PlacePicker.GetPlace(this, data);
            edtChooseLocation.Text = place.AddressFormatted.ToString();
        }
    }
}
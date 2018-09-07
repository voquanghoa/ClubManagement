using System;
using Android.App;
using Android.Content;
using Android.Gms.Location.Places;
using Android.Gms.Location.Places.UI;
using Android.OS;
using Android.Widget;
using ClubManagement.Ultilities;
using Android.Views;
using ClubManagement.Controllers;
using ClubManagement.Fragments;
using ClubManagement.Models;

namespace ClubManagement.Activities
{
    [Activity(Label = "CreateEventActivity")]
    public class CreateEventActivity : Activity
    {
        private IPlace place;

        private DateTime startTime;

        private DateTime endTime;

        public CreateEventActivity()
        {
            startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                DateTime.Now.Hour > 9 ? DateTime.Now.Hour + 1 : 9, 0, 0);
            endTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour + 1, startTime.Minute, startTime.Second);
        }

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

        [InjectView(Resource.Id.imgEvent)]
        private ImageView imgEvent;

        private const int PlacePickerRequset = 1;

        private const int RequestPickAvatar = 2;

        [InjectOnClick(Resource.Id.btnChangeEventImage)]
        private void ChangeEventImage(object sender, EventArgs e)
        {
            if (sender is View view)
            {
                var popupMenu = view.CreatepopupMenu(Resource.Menu.AddPhoto);
                popupMenu.MenuItemClick += PopupMenu_MenuItemClick;
                popupMenu.Show();
            }
        }

        private void PopupMenu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            if (sender is PopupMenu popupMenu)
            {
                switch (e.Item.ItemId)
                {
                    case Resource.Id.addPhoto:
                        var intent = new Intent(Intent.ActionGetContent);
                        intent.SetType("image/*");
                        StartActivityForResult(intent, RequestPickAvatar);

                        break;
                    case Resource.Id.cancel:
                        popupMenu.Dismiss();
                        break;
                }
            }
        }

        [InjectOnClick(Resource.Id.tvStartDate)]
        private void PickStartDate(object sender, EventArgs e)
        {
            PickDate(DateTime.Now, true);
        }

        [InjectOnClick(Resource.Id.tvStartTime)]
        private void PickStartTime(object sender, EventArgs e)
        {
            PickTime(startTime, true);
        }

        [InjectOnClick(Resource.Id.tvEndDate)]
        private void PickEndDate(object sender, EventArgs e)
        {
            PickDate(startTime, false);
        }

        [InjectOnClick(Resource.Id.tvEndTime)]
        private void PickEndTime(object sender, EventArgs e)
        {
            PickTime(endTime, false);
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

        private EventModel eventModel = new EventModel();


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_add_event);
            Cheeseknife.Inject(this);
            UpdateView();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == RequestPickAvatar && resultCode == Result.Ok)
            {
                this.DoRequest(async ()=> 
                {
                    eventModel.ImageUrl = await CloudinaryController.UploadImage(this, data.Data, $"Images/{Guid.NewGuid()}", 256);
                });
                    
                imgEvent.SetImageURI(data.Data);
            }

            if (requestCode != PlacePickerRequset || resultCode != Result.Ok) return;
            place = PlacePicker.GetPlace(this, data);
            edtChooseLocation.Text = place.AddressFormatted.ToString();
        }

        private void PickDate(DateTime minDateTime, bool isPickingStartDate)
        {
            var datePickerDialog = new CustomDatePickerDialog(minDateTime);
            datePickerDialog.PickDate += (s, se) =>
            {
                if (s is DateTime dateTime)
                {
                    if (isPickingStartDate)
                    {
                        startTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, startTime.Hour,
                            startTime.Minute, startTime.Second);
                        UpdateTime();
                    }
                    else
                    {
                        endTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, endTime.Hour,
                            endTime.Minute, endTime.Second);
                    }
                    UpdateView();
                }
            };
            datePickerDialog.Show(FragmentManager, "");
        }

        private void PickTime(DateTime mintime, bool isPickingStartTime)
        {
            var timePickerDialog = new CustomTimePickerDialog(mintime);
            timePickerDialog.PickTime += (s, e) =>
            {
                if (s is DateTime dateTime)
                {
                    if (isPickingStartTime)
                    {
                        if (dateTime <= DateTime.Now)
                        {
                            Toast.MakeText(this, Resources.GetString(Resource.String.pick_time_in_future), ToastLength.Short).Show();
                            return;
                        }
                        startTime = dateTime;
                        UpdateTime();
                    }
                    else
                    {
                        if (dateTime <= startTime)
                        {
                            Toast.MakeText(this, Resources.GetString(Resource.String.pick_time_before_start_time), ToastLength.Short).Show();
                            return;
                        }

                        endTime = dateTime;
                    }
                    UpdateView();
                }
            };
            timePickerDialog.Show(FragmentManager, "");
        }

        private void UpdateTime()
        {
            if (endTime > startTime) return;
            endTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour + 1, startTime.Minute, startTime.Second);
        }

        private void UpdateView()
        {
            tvStartDate.Text = startTime.ToString("yyyy MMM dd");
            tvStartTime.Text = $"{startTime.Hour} : {startTime.Minute}";
            tvEndDate.Text = endTime.ToString("yyyy MMM dd");
            tvEndTime.Text = $"{endTime.Hour} : {endTime.Minute}";
        }
    }
}
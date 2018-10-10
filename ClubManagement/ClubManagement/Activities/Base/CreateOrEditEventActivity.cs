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
    public class CreateOrEditEventActivity : Activity
    {
        protected IPlace place;

        protected DateTime startTime;

        protected DateTime endTime;

        protected string imageUrl = "";

        [InjectView(Resource.Id.edtEventTitle)]
        protected EditText edtEventTitle;

        [InjectView(Resource.Id.tvStartDate)]
        protected TextView tvStartDate;

        [InjectView(Resource.Id.tvStartTime)]
        protected TextView tvStartTime;

        [InjectView(Resource.Id.tvEndDate)]
        protected TextView tvEndDate;

        [InjectView(Resource.Id.tvEndTime)]
        protected TextView tvEndTime;

        [InjectView(Resource.Id.edtChooseLocation)]
        protected EditText edtChooseLocation;

        [InjectView(Resource.Id.edtEventLocation)]
        protected EditText edtEventLocation;

        [InjectView(Resource.Id.edtEventDescription)]
        protected EditText edtEventDescription;

        [InjectView(Resource.Id.imgEvent)]
        protected ImageView imgEvent;

        private const int PlacePickerRequset = 1;

        private const int RequestPickAvatar = 2;

        [InjectOnClick(Resource.Id.btnChangeEventImage)]
        protected void ChangeEventImage(object sender, EventArgs e)
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
        protected void PickStartDate(object sender, EventArgs e)
        {
            PickDate(DateTime.Now, true);
        }

        [InjectOnClick(Resource.Id.tvStartTime)]
        protected void PickStartTime(object sender, EventArgs e)
        {
            PickTime(startTime, true);
        }

        [InjectOnClick(Resource.Id.tvEndDate)]
        protected void PickEndDate(object sender, EventArgs e)
        {
            PickDate(startTime, false);
        }

        [InjectOnClick(Resource.Id.tvEndTime)]
        protected void PickEndTime(object sender, EventArgs e)
        {
            PickTime(endTime, false);
        }

        [InjectOnClick(Resource.Id.edtChooseLocation)]
        protected void ChooseLocation(object sender, EventArgs e)
        {
            var builder = new PlacePicker.IntentBuilder();
            StartActivityForResult(builder.Build(this), PlacePickerRequset);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_add_event);
            Cheeseknife.Inject(this);
            edtEventTitle.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == RequestPickAvatar && resultCode == Result.Ok)
            {
                var progressDialog = this.CreateDialog(GetString(Resource.String.upload_a_photo),
                GetString(Resource.String.wait));
                progressDialog.Show();

                this.DoRequest(CloudinaryController.UploadImage(this, data.Data, $"Images/{Guid.NewGuid()}", 256),
                    () => progressDialog.Dismiss());

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
                if (!(s is DateTime dateTime)) return;
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
            };
            datePickerDialog.Show(FragmentManager, "");
        }

        private void PickTime(DateTime mintime, bool isPickingStartTime)
        {
            var timePickerDialog = new CustomTimePickerDialog(mintime);
            timePickerDialog.PickTime += (s, e) =>
            {
                if (!(s is DateTime dateTime)) return;
                if (isPickingStartTime)
                {
                    if (dateTime <= DateTime.Now)
                    {
						this.ShowMessage(Resource.String.pick_time_in_future);
                        return;
                    }
                    startTime = dateTime;
                    UpdateTime();
                }
                else
                {
                    if (dateTime <= startTime)
                    {
						this.ShowMessage(Resource.String.pick_time_before_start_time);
                        return;
                    }

                    endTime = dateTime;
                }
                UpdateView();
            };
            timePickerDialog.Show(FragmentManager, "");
        }

        private void UpdateTime()
        {
            if (endTime > startTime) return;
            endTime = new DateTime(startTime.Ticks).AddHours(1);
        }

        protected void UpdateView()
        {
            tvStartDate.Text = startTime.ToDateString();
            tvStartTime.Text = startTime.ToTimeString();
            tvEndDate.Text = endTime.ToDateString();
            tvEndTime.Text = endTime.ToTimeString();
        }
    }
}
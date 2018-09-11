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

namespace ClubManagement.Activities.Base
{
    public class CreateOrEditEvent
    {
        private IPlace place;

        private DateTime startTime;

        private DateTime endTime;

        private string imageUrl = "";

        private Activity activity;

        public DateTime StartTime
        {
            set
            {
                startTime = value;
            }
        }

        public DateTime EndTime
        {
            set
            {
                endTime = value;
            }
        }

        public UserLoginEventModel Event
        {
            set
            {
                startTime = value.TimeStart;
                endTime = value.TimeEnd;
                edtEventTitle.Text = value.Title;
                edtEventDescription.Text = value.Description;
                edtEventLocation.Text = value.Place;
                edtChooseLocation.Text = value.Place;

                UpdateView();
            }
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
                        activity.StartActivityForResult(intent, RequestPickAvatar);

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
            activity.StartActivityForResult(builder.Build(activity), PlacePickerRequset);
        }

        [InjectOnClick(Resource.Id.btnDone)]
        private void Done(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(edtChooseLocation.Text) ||
                string.IsNullOrEmpty(edtEventTitle.Text) ||
                string.IsNullOrEmpty(edtEventDescription.Text) ||
                string.IsNullOrEmpty(tvStartDate.Text) ||
                string.IsNullOrEmpty(edtEventLocation.Text) ||
                string.IsNullOrEmpty(tvStartTime.Text))
            {
                Toast.MakeText(activity, activity.GetString(Resource.String.fill_all_fields), ToastLength.Short).Show();
                return;
            }

            var eventModel = new EventModel
            {
                Description = edtEventDescription.Text,
                Place = edtEventLocation.Text,
                Title = edtEventTitle.Text,
                CreatedBy = AppDataController.Instance.UserName,
                CreatedTime = DateTime.Now,
                TimeStart = startTime,
                TimeEnd = endTime,
                Longitude = place.LatLng.Longitude,
                Latitude = place.LatLng.Latitude,
                ImageUrl = imageUrl
            };

            var progressDialog = activity.CreateDialog(activity.GetString(Resource.String.adding_event),
                activity.GetString(Resource.String.wait));
            progressDialog.Show();

            activity.DoRequest(async () =>
            {
                await EventsController.Instance.Add(eventModel);
            }, () =>
            {
                progressDialog.Dismiss();
            });

            activity.SetResult(Result.Ok);
            activity.Finish();
        }

        [InjectOnClick(Resource.Id.btnCross)]
        private void Cross(object sender, EventArgs e)
        {
            Cancel();
        }

        [InjectOnClick(Resource.Id.btnCancel)]
        private void Cancel(object sender, EventArgs e)
        {
            Cancel();
        }

        private void Cancel()
        {
            activity.ShowConfirmDialog(
                Resource.String.cross_create_event_title,
                Resource.String.cross_create_event_message,
                activity.Finish,
                () => { }).Show();
        }

        public void OnCreate(Bundle savedInstanceState, Activity activity, View view)
        {
            Cheeseknife.Inject(this, view);

            this.activity = activity;

            UpdateView();
        }
        
        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == RequestPickAvatar && resultCode == Result.Ok)
            {
                activity.DoRequest(async () =>
                {
                    imageUrl = await CloudinaryController.UploadImage(activity, data.Data, $"Images/{Guid.NewGuid()}", 256);
                });

                imgEvent.SetImageURI(data.Data);
            }

            if (requestCode != PlacePickerRequset || resultCode != Result.Ok) return;
            place = PlacePicker.GetPlace(activity, data);
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
            datePickerDialog.Show(activity.FragmentManager, "");
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
                        Toast.MakeText(activity, activity.GetString(Resource.String.pick_time_in_future), ToastLength.Short).Show();
                        return;
                    }
                    startTime = dateTime;
                    UpdateTime();
                }
                else
                {
                    if (dateTime <= startTime)
                    {
                        Toast.MakeText(activity, activity.GetString(Resource.String.pick_time_before_start_time), ToastLength.Short).Show();
                        return;
                    }

                    endTime = dateTime;
                }
                UpdateView();
            };
            timePickerDialog.Show(activity.FragmentManager, "");
        }

        private void UpdateTime()
        {
            if (endTime > startTime) return;
            endTime = new DateTime(startTime.Ticks).AddHours(1);
        }

        public void UpdateView()
        {
            tvStartDate.Text = startTime.ToString("yyyy MMM dd");
            tvStartTime.Text = $"{startTime.Hour} : {startTime.Minute}";
            tvEndDate.Text = endTime.ToString("yyyy MMM dd");
            tvEndTime.Text = $"{endTime.Hour} : {endTime.Minute}";
        }
    }
}
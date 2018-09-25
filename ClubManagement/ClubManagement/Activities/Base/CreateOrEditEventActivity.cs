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
        private IPlace place;

        protected DateTime startTime;

        protected DateTime endTime;

        private string imageUrl = "";

        private EventModel eventModel;

        private bool isEdit;

        protected UserLoginEventModel Event
        {
            set
            {
                eventModel = value as EventModel;
                isEdit = true;

                startTime = value.TimeStart;
                endTime = value.TimeEnd;
                edtEventTitle.Text = value.Title;
                edtEventDescription.Text = value.Description;
                edtEventLocation.Text = value.Place;
                edtChooseLocation.Text = value.Address;
                imageUrl = value.ImageUrl;

                if (!string.IsNullOrEmpty(value.ImageUrl))
                {
                    Picasso.With(this).Load(value.ImageUrl).Into(imgEvent);
                }

                UpdateView();
            }
        }

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

        [InjectOnClick(Resource.Id.btnDone)]
        protected void Done(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(edtEventTitle.Text) ||
                string.IsNullOrEmpty(edtEventDescription.Text) ||
                string.IsNullOrEmpty(tvStartDate.Text) ||
                string.IsNullOrEmpty(edtChooseLocation.Text) ||
                string.IsNullOrEmpty(tvStartTime.Text))
            {
				this.ShowMessage(Resource.String.fill_all_fields);
                return;
            }

            if (isEdit)
            {
                eventModel.Description = edtEventDescription.Text;
                eventModel.Place = edtEventLocation.Text;
                eventModel.Title = edtEventTitle.Text;
                eventModel.TimeStart = startTime;
                eventModel.TimeEnd = endTime;
                eventModel.ImageUrl = imageUrl;

                if (!eventModel.Address.Equals(edtChooseLocation.Text))
                {
                    eventModel.Address = edtChooseLocation.Text;
                    eventModel.Longitude = place.LatLng.Longitude;
                    eventModel.Latitude = place.LatLng.Latitude;
                }
            }
            else
            {
                eventModel = new EventModel
                {
                    Description = edtEventDescription.Text,
                    Place = edtEventLocation.Text,
                    Title = edtEventTitle.Text,
                    CreatedBy = AppDataController.Instance.UserName,
                    CreatedTime = DateTime.Now,
                    TimeStart = startTime,
                    TimeEnd = endTime,
                    Address = edtChooseLocation.Text,
                    Longitude = place.LatLng.Longitude,
                    Latitude = place.LatLng.Latitude,
                    ImageUrl = imageUrl
                };
            }

            var progressDialog = this.CreateDialog(GetString(isEdit
                    ? Resource.String.editing_event
                    : Resource.String.adding_event),
                GetString(Resource.String.wait));

            progressDialog.Show();

            this.DoRequest(async () =>
            {
                if (isEdit)
                {
                    await EventsController.Instance.Edit(eventModel);
                }
                else
                {
                    await EventsController.Instance.Add(eventModel);
                }
            }, () =>
            {
                progressDialog.Dismiss();

                if (!isEdit)
                {
                    Toast.MakeText(this, Resource.String.create_event_success, ToastLength.Short).Show();
                    SetResult(Result.Ok);
                }
                else
                {
                    var eventDetail = JsonConvert.SerializeObject(eventModel);
                    SetResult(Result.Ok, new Intent().PutExtra("EventDetail", eventDetail));
                }

                Finish();
            });
        }

        [InjectOnClick(Resource.Id.btnCross)]
        protected void Cross(object sender, EventArgs e)
        {
            Cancel();
        }

        [InjectOnClick(Resource.Id.btnCancel)]
        protected void Cancel(object sender, EventArgs e)
        {
            Cancel();
        }

        private void Cancel()
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

                this.DoRequest(async () =>
                {
                    imageUrl = await CloudinaryController.UploadImage(this, data.Data, $"Images/{Guid.NewGuid()}", 256);
                }, () => progressDialog.Dismiss());

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
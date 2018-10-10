using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using ClubManagement.Activities.Base;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using Newtonsoft.Json;
using Square.Picasso;
using System;

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

        [InjectOnClick(Resource.Id.btnDone)]
        private void Done(object sender, EventArgs e)
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

            var nameChanged = !eventModel.Title.Equals(edtEventTitle.Text);
            var timeChanged = eventModel.TimeStart != startTime || eventModel.TimeEnd != endTime;
            var locationChanged = !eventModel.Address.Equals(edtChooseLocation.Text);

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

            var message = "";

            if (nameChanged && timeChanged && locationChanged)
            {
                message = "Admin changed the name, time and location of event";
            }
            else if (nameChanged && timeChanged || timeChanged && locationChanged || locationChanged && nameChanged)
            {
                if (nameChanged && timeChanged)
                {
                    message = "Admin changed the name and time of event";
                }

                if (timeChanged && locationChanged)
                {
                    message = "Admin changed the time and location of event";
                }

                if (locationChanged && nameChanged)
                {
                    message = "Admin changed the name and location of event";
                }
            }
            else
            {
                if (nameChanged)
                {
                    message = "Admin changed the name of event";
                }

                if (timeChanged)
                {
                    message = "Admin changed the time of event";
                }

                if (locationChanged)
                {
                    message = "Admin changed the location of event";
                }
            }

            var progressDialog = this.CreateDialog(GetString(Resource.String.editing_event),
                GetString(Resource.String.wait));

            progressDialog.Show();

            this.DoRequest(EventsController.Instance.Edit(eventModel), () =>
            {
                progressDialog.Dismiss();

                var eventDetail = JsonConvert.SerializeObject(eventModel);
                this.ShowMessage(Resource.String.edit_event_success);
                SetResult(Result.Ok, new Intent().PutExtra("EventDetail", eventDetail));

                if (nameChanged || timeChanged || locationChanged)
                {
                    var notificationsController = NotificationsController.Instance;

                    notificationsController.UpdateNotificationAsync(new NotificationModel()
                    {
                        Message = $"{message} {eventModel.Title}",
                        Type = AppConstantValues.NotificationEditEvent,
                        TypeId = eventModel.Id,
                        IsNew = true,
                        LastUpdate = DateTime.Now
                    });

                    notificationsController
                        .PushNotifyAsync(AppConstantValues.NotificationEditEvent, eventModel.Title);
                }

                Finish();
            });
        }

        [InjectOnClick(Resource.Id.btnCancel)]
        private void Cancel(object sender, EventArgs e)
        {
            this.ShowConfirmDialog(
                Resource.String.confirm,
                Resource.String.edit_event_cancel_message,
                Finish,
                () => { }).Show();
        }

        private EventModel eventModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Init();
        }

        private void Init()
        {
            btnCancel.Visibility = ViewStates.Visible;
            tvTitle.Gravity = GravityFlags.Center;
            tvTitle.Text = GetString(Resource.String.edit_event);
            btnDone.Text = GetString(Resource.String.button_ok).ToUpper();

            var content = Intent.GetStringExtra("EventDetail");
            eventModel = JsonConvert.DeserializeObject<UserLoginEventModel>(content);

            startTime = eventModel.TimeStart;
            endTime = eventModel.TimeEnd;
            edtEventTitle.Text = eventModel.Title;
            edtEventDescription.Text = eventModel.Description;
            edtEventLocation.Text = eventModel.Place;
            edtChooseLocation.Text = eventModel.Address;
            imageUrl = eventModel.ImageUrl;

            if (!string.IsNullOrEmpty(eventModel.ImageUrl))
            {
                Picasso.With(this).Load(eventModel.ImageUrl).Into(imgEvent);
            }

            UpdateView();
        }
    }
}
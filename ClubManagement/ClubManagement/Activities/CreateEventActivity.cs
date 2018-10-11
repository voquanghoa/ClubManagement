using System;
using Android.App;
using Android.Content;
using Android.OS;
using ClubManagement.Activities.Base;
using Android.Widget;
using Android.Views;
using ClubManagement.Ultilities;
using ClubManagement.Models;
using ClubManagement.Controllers;

namespace ClubManagement.Activities
{
    [Activity(Label = "CreateEventActivity")]
    public class CreateEventActivity : CreateOrEditEventActivity
    {
        [InjectView(Resource.Id.btnCross)]
        private ImageButton btnCross;

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

            var eventModel = new EventModel
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

            var progressDialog = this.CreateDialog(GetString(Resource.String.adding_event),
                GetString(Resource.String.wait));

            progressDialog.Show();

            this.DoRequest(EventsController.Instance.Add(eventModel), () =>
            {
                progressDialog.Dismiss();

                this.ShowMessage(Resource.String.create_event_success);
                SetResult(Result.Ok);

                Finish();
            });
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

        public CreateEventActivity()
        {
            startTime = DateTime.Now;
            endTime = DateTime.Now.AddHours(1);
        }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            btnCross.Visibility = ViewStates.Visible;

            UpdateView();
        }
    }
}
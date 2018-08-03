using System;
using Android.App;
using Android.Gms.Maps;
using Android.OS;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using Newtonsoft.Json;
using System.Linq;
using Android.Content;
using ClubManagement.Activities.Base;

namespace ClubManagement.Activities
{
    [Activity(Label = "EventDetailActivity", Theme = "@style/AppTheme")]
    public class EventDetailActivity : MapAbstractActivity
    {
        [InjectView(Resource.Id.tvTitle)]
        private TextView tvTitle;

        [InjectView(Resource.Id.tvDescription)]
        private TextView tvDescription;

        [InjectView(Resource.Id.tvTime)]
        private TextView tvTime;

        [InjectView(Resource.Id.tvPlace)]
        private TextView tvPlace;

        [InjectView(Resource.Id.btnJoin)]
        private Button btnJoin;

        [InjectOnClick(Resource.Id.btnBack)]
        private void Back(object s, EventArgs e)
        {
            Finish();
        }

        [InjectView(Resource.Id.tvCreatedBy)]
        private TextView tvCreatedBy;

        private UserEventsController userEventsController = UserEventsController.Instance;

        private string userId = AppDataController.Instance.UserId;

        private UserLoginEventModel eventDetail;

        private bool currentIsJoined;

        private string content;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EventDetail);

            Cheeseknife.Inject(this);

            content = Intent.GetStringExtra("EventDetail");

            eventDetail = JsonConvert.DeserializeObject<UserLoginEventModel>(content);
            
            tvCreatedBy.Text = $"Created by {eventDetail.CreatedBy} in {eventDetail.CreatedTime}";
            tvTitle.Text = eventDetail.Title;
            tvDescription.Text = eventDetail.Description;
            tvTime.Text = eventDetail.Time.ToShortDateString();
            tvPlace.Text = eventDetail.Place;

            currentIsJoined = eventDetail.IsJoined;

            btnJoin.ChangeStatusButtonJoin(currentIsJoined);

            btnJoin.Click += (s, e) =>
            {
                this.DoWithAdmin(() =>
                {
                    UpdateUserEvents(!currentIsJoined);
                }, () =>
                {
                    if (currentIsJoined == false)
                    {
                        UpdateUserEvents(!currentIsJoined);
                    }
                });
            };

            FragmentManager.FindFragmentById<MapFragment>(Resource.Id.mapFragment).GetMapAsync(this);

			MapReady += EventDetailActivity_MapReady;
        }

        private void UpdateUserEvents(bool isJoined)
        {
            currentIsJoined = isJoined;
            btnJoin.ChangeStatusButtonJoin(isJoined);

            this.DoRequest(() =>
            {
                if (isJoined)
                {
                    userEventsController.Add(new UserEventModel()
                    {
                        EventId = eventDetail.Id,
                        UserId = userId
                    });
                }
                else
                {
                    var userEvent = userEventsController.Values
                        .First(x => x.EventId == eventDetail.Id && x.UserId == userId);

                    userEventsController.Delete(userEvent);
                }
            }, () => { });
        }

		private void EventDetailActivity_MapReady(object sender, EventArgs e)
		{
			googleMap.MapClick += GoogleMap_MapClick;

            AddMapMarker(eventDetail.Latitude, eventDetail.Longitude, eventDetail.Title, Resource.Drawable.icon_event);

            MoveMapCamera(eventDetail.Latitude, eventDetail.Longitude);
		}

		private void GoogleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
		{
			var intent = new Intent(this, typeof(MemberLocationActivity));

            intent.PutExtra("EventDetail", content);

            StartActivity(intent);
		}

    }
}
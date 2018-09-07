using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Gms.Maps;
using ClubManagement.Controllers;
using ClubManagement.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ClubManagement.Activities.Base;
using System.Collections.Generic;
using Android.Gms.Maps.Model;
using ClubManagement.Ultilities;
using Android.Widget;
using Square.Picasso;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace ClubManagement.Activities
{
    [Activity(Label = "MapActivity")]
    public class MemberLocationActivity : MapAbstractActivity
    {
        [InjectOnClick(Resource.Id.btnBack)]
        private void Back(object s, EventArgs e)
        {
            Finish();
        }

        [InjectView(Resource.Id.tvNameAddress)]
        private TextView tvNameAddress;

        [InjectView(Resource.Id.tvAddress)]
        private TextView tvAddress;

        [InjectView(Resource.Id.tvNumberPeople)]
        private TextView tvNumberPeople;

        private UserEventsController userEventsController = UserEventsController.Instance;

        private UsersController usersController = UsersController.Instance;

        private UserLoginEventModel eventDetail;

        private List<Marker> markers = new List<Marker>();

        private void MemberLocationActivity_MapReady(object sender, EventArgs e)
        {
            var users = new List<PersonGoTimeModel>();

            this.DoRequest(() =>
            {
                users = userEventsController.Values.Where(x => x.EventId == eventDetail.Id)
                    .Join(usersController.Values,
                        x => x.UserId,
                        y => y.Id,
                        (x, y) => y)
                    .Select(x =>
                    {
                        var personGoTimeModel = new PersonGoTimeModel()
                        {
                            Name = x.Name,
                            DistanceAndTime = MapsController.Instance.GetGoTime(x.Latitude,
                                x.Longitude,
                                eventDetail.Latitude,
                                eventDetail.Longitude),
                            Avatar = x.Avatar,
                            Selected = false,
                            Latitude = x.Latitude,
                            Longitude = x.Longitude,
                            LastLogin = x.LastLogin
                        };

                        return personGoTimeModel;
                    }).ToList();
            }, () =>
            {
                users.ForEach(x =>
                {
                    var title = $"{x.DistanceAndTime.Duration?.Text ?? "0 mims"}\n{x.DistanceAndTime.Distance?.Text ?? "0 m"} ago";

                    var userMarker = AddMapMarker(x.Latitude, x.Longitude, x.Name);
                    userMarker.Snippet = title;

                    markers.Add(userMarker);

                    var alpha = eventDetail.Time.Subtract(x.LastLogin) < new TimeSpan(0, 30, 0)
                        && eventDetail.Time.CompareTo(x.LastLogin) == 1
                        ? 1 : 0.6f;

                    if (!string.IsNullOrEmpty(x.Avatar))
                    {
                        Picasso.With(this).Load(x.Avatar).Into(new Target()
                        {
                            Marker = userMarker,
                            Alpha = alpha
                        });
                    }
                    else
                    {
                        userMarker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.icon_user));
                        userMarker.Alpha = alpha;
                    }
                });
            });

            var marker = AddMapMarker(eventDetail.Latitude, eventDetail.Longitude, eventDetail.Title);

            marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.icon_event));

            MoveMapCamera(eventDetail.Latitude, eventDetail.Longitude);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ActivityMemberLocation);

            Cheeseknife.Inject(this);

            var content = Intent.GetStringExtra("EventDetail");
            eventDetail = JsonConvert.DeserializeObject<UserLoginEventModel>(content);

            tvNameAddress.Text = eventDetail.Place;
            tvAddress.Text = eventDetail.Place;
            tvNumberPeople.Text = $"{Intent.GetStringExtra("NumberPeople")} Going";

            FragmentManager.FindFragmentById<MapFragment>(Resource.Id.mapFragment).GetMapAsync(this);

            MapReady += MemberLocationActivity_MapReady;
        }
    }
}
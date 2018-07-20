using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Fragments;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ClubManagement.Activities
{
    [Activity(Label = "MapActivity")]
    public class MemberLocationActivity : Activity, IOnMapReadyCallback
    {
        [InjectOnClick(Resource.Id.btnBack)]
        private void Back(object s, EventArgs e)
        {
            Finish();
        }

        private UserEventsController userEventsController = UserEventsController.Instance;

        private UsersController usersController = UsersController.Instance;

        private UserLoginEventModel eventDetail;

        private GoogleMap googleMap;

        private PersonGoTimesFragment personGoTimesFragment = new PersonGoTimesFragment();

        public void OnMapReady(GoogleMap googleMap)
        {
            this.googleMap = googleMap;

            Task.Run(() =>
            {
                var user = userEventsController.Values.Where(x => x.EventId == eventDetail.Id)
               .Join(usersController.Values,
                   x => x.UserId,
                   y => y.Id,
                   (x, y) => y)
               .ToList();

                RunOnUiThread(() =>
                {
                    user.ForEach(x => AddMarkerMap(x.Latitude, x.Longitude, x.Name, Resource.Drawable.icon_person));
                });
            });

            AddMarkerMap(eventDetail.Latitude, eventDetail.Longitude, eventDetail.Title, Resource.Drawable.icon_event);

            MoveCameraMap(eventDetail.Latitude, eventDetail.Longitude);
        }

        private void AddMarkerMap(double lat, double lng, string title, int iconResourceId)
        {
            googleMap.AddMarker(new MarkerOptions()
                .SetPosition(new LatLng(lat, lng))
                .SetTitle(title)
                .SetIcon(BitmapDescriptorFactory.FromResource(iconResourceId)));
        }

        private void MoveCameraMap(double lat, double lng)
        {
            var builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(lat, lng));
            builder.Zoom(12);

            var cameraPosition = builder.Build();
            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            googleMap.MoveCamera(cameraUpdate);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ActivityMemberLocation);

            Cheeseknife.Inject(this);

            var fragemtListPerson = FragmentManager.FindFragmentById<Fragment>(Resource.Id.fragemtListPerson);

            var mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.fragemtMap);

            mapFragment.GetMapAsync(this);

            var content = Intent.GetStringExtra("EventDetail");

            eventDetail = JsonConvert.DeserializeObject<UserLoginEventModel>(content);

            personGoTimesFragment.EventDetail = eventDetail;

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.fragemtListPerson, personGoTimesFragment)
                .Commit();

            personGoTimesFragment.PersonGoTimeClick += (s, e) =>
            {
                MoveCameraMap(e.Latitude, e.Longitude);
            };

            personGoTimesFragment.DisplayPersonsClick += (s, e) =>
            {
                FragmentManager.BeginTransaction()
                .Detach(personGoTimesFragment)
                .Attach(personGoTimesFragment)
                .Commit();
            };
        }
    }
}
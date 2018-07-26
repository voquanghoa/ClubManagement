using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Gms.Maps;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Fragments;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Android.Widget;
using ClubManagement.Activities.Base;
using System.Collections.Generic;
using Android.Gms.Maps.Model;

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

        private UserEventsController userEventsController = UserEventsController.Instance;

        private UsersController usersController = UsersController.Instance;

        private UserLoginEventModel eventDetail;

        private PersonGoTimesFragment personGoTimesFragment = new PersonGoTimesFragment();

        private List<Marker> markers = new List<Marker>();

        private void MemberLocationActivity_MapReady(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    var user = userEventsController.Values.Where(x => x.EventId == eventDetail.Id)
                        .Join(usersController.Values,
                            x => x.UserId,
                            y => y.Id,
                            (x, y) => y)
                        .ToList();

                    RunOnUiThread(() =>
                    {
                        user.ForEach(x => AddMapMarker(x.Latitude, x.Longitude, x.Name, Resource.Drawable.icon_person));
                    });
                }
                catch (Exception)
                {
                    Toast.MakeText(this, Resources.GetString(Resource.String.no_internet_connection), ToastLength.Short).Show();
                }
            });

            AddMapMarker(eventDetail.Latitude, eventDetail.Longitude, eventDetail.Title, Resource.Drawable.icon_event);

            MoveMapCamera(eventDetail.Latitude, eventDetail.Longitude);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ActivityMemberLocation);

            Cheeseknife.Inject(this);

            FragmentManager.BeginTransaction()
                           .Replace(Resource.Id.memberFrament, personGoTimesFragment)
                .Commit();

            var content = Intent.GetStringExtra("EventDetail");

            eventDetail = JsonConvert.DeserializeObject<UserLoginEventModel>(content);

            personGoTimesFragment.EventDetail = eventDetail;

            personGoTimesFragment.PersonGoTimeClick += (s, e) =>
            {
                MoveMapCamera(e.Latitude, e.Longitude);
            };

            personGoTimesFragment.DisplayPersonsClick += (s, e) =>
            {
                FragmentManager.BeginTransaction()
                    .Detach(personGoTimesFragment)
                    .Attach(personGoTimesFragment)
                    .Commit();
            };

            FragmentManager.FindFragmentById<MapFragment>(Resource.Id.mapFragment).GetMapAsync(this);

            MapReady += MemberLocationActivity_MapReady;

            var previousPosition = 0;

            personGoTimesFragment.ItemClick += (s, e) =>
            {
                markers[previousPosition].SetIcon(BitmapDescriptorFactory
                    .FromResource(Resource.Drawable.icon_person));

                markers[e.Position].SetIcon(BitmapDescriptorFactory
                    .FromResource(Resource.Drawable.icon_person_selected));
                markers[e.Position].ShowInfoWindow();

                previousPosition = e.Position;
            };
        }
    }
}
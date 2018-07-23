using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Fragments;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ClubManagement.Activities.Base;

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

        protected override void HandleWhenMapReady(GoogleMap googleMap)
        {
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ActivityMemberLocation);

            Cheeseknife.Inject(this);

            var fragemtListPerson = FragmentManager.FindFragmentById<Fragment>(Resource.Id.fragemtListPerson);

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.fragemtListPerson, personGoTimesFragment)
                .Commit();

            var content = Intent.GetStringExtra("EventDetail");

            eventDetail = JsonConvert.DeserializeObject<UserLoginEventModel>(content);

            personGoTimesFragment.EventDetail = eventDetail;

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

            FragmentManager.FindFragmentById<MapFragment>(Resource.Id.fragemtMap).GetMapAsync(this);
        }
    }
}
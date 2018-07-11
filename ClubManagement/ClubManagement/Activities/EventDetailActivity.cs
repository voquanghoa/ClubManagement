using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Preferences;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Fragments;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using Newtonsoft.Json;
using System.Linq;

namespace ClubManagement.Activities
{
    [Activity(Label = "EventDetailActivity", Theme = "@style/AppTheme")]
    public class EventDetailActivity : Activity, IOnMapReadyCallback
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

        [InjectView(Resource.Id.tvCreatedBy)]
        private TextView tvCreatedBy;

        private UserLoginEventModel eventDetail;

        private bool currentIsJoined;

        private UserEventsController userEventsController = UserEventsController.Instance;

        private UsersController usersController = UsersController.Instance;

        private GoogleMap googleMap;

        public void OnMapReady(GoogleMap googleMap)
        {
            this.googleMap = googleMap;

            userEventsController.Values.Where(x => x.EventId == eventDetail.Id)
                .Join(usersController.Values,
                    x => x.UserId,
                    y => y.Id,
                    (x, y) => y)
                .ToList()
                .ForEach(x => AddMarkerMap(x.Latitude, x.Longitude, x.Name, Resource.Drawable.icon_person));

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
            builder.Zoom(15);

            var cameraPosition = builder.Build();
            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            googleMap.MoveCamera(cameraUpdate);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EventDetail);

            var fragemtListPerson = FragmentManager.FindFragmentById<Fragment>(Resource.Id.fragemtListPerson);

            var mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.fragemtMap);

            mapFragment.GetMapAsync(this);

            Cheeseknife.Inject(this);

            var content = Intent.GetStringExtra("EventDetail");

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
                currentIsJoined = !currentIsJoined;
                btnJoin.ChangeStatusButtonJoin(currentIsJoined);
                UpdateUserEvents(currentIsJoined);
            };

            var personGoTimesFragment = new PersonGoTimesFragment(eventDetail);

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

        private void UpdateUserEvents(bool isJoined)
        {
            var preferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            var userId = preferences.GetString("UserId", string.Empty);

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
        }
    }
}
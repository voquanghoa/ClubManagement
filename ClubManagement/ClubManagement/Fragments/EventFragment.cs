using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Adapters;
using ClubManagement.Models;
using System.Linq;
using ClubManagement.Controllers;
using Android.Preferences;
using Android.Content;
using ClubManagement.Activities;
using Newtonsoft.Json;
using Android.Runtime;
using System.Collections.Generic;

namespace ClubManagement.Fragments
{
    public class EventFragment : Fragment
    {
        private View view;

        private const string AllTab = "All";

        private const string UpcomingTab = "Upcoming";

        private const string JoinedTab = "Joined";

        private UserEventsController userEventsController = UserEventsController.Instance;

        private EventsController eventsController = EventsController.Instance;

        private EventsAdapter adapter;

        private List<UserLoginEventModel> events;

        private string userId;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.FragmentEvent, container, false);

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            recyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

            var preferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            userId = preferences.GetString("UserId", string.Empty);

            events = eventsController.Values.Select(x =>
            {
                var userLoginEventModel = new UserLoginEventModel(x)
                {
                    Place = MapsController.Instance.GetAddress(x.Latitude, x.Longitude),
                    IsJoined = userEventsController.Values.Any(y => y.EventId == x.Id && y.UserId == userId)
                };

                return userLoginEventModel;
            }).ToList();

            adapter = new EventsAdapter(events);
            recyclerView.SetAdapter(adapter);

            adapter.ItemClick += (s, e) =>
            {
                var intent = new Intent(view.Context, typeof(EventDetailActivity));

                var eventDetail = JsonConvert.SerializeObject(events[e.Position]);

                intent.PutExtra("EventDetail", eventDetail);

                StartActivityForResult(intent, 0);
            };

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabView1);

            tabLayout.AddTab(tabLayout.NewTab().SetText(AllTab));
            tabLayout.AddTab(tabLayout.NewTab().SetText(UpcomingTab));
            tabLayout.AddTab(tabLayout.NewTab().SetText(JoinedTab));

            tabLayout.TabSelected += (s, e) =>
            {
                switch (e.Tab.Text)
                {
                    case AllTab:
                        adapter.Events = events;
                        break;
                    case UpcomingTab:
                        adapter.Events = events.Where(x => x.IsJoined).ToList();
                        break;
                    case JoinedTab:
                        adapter.Events = events.Where(x => !x.IsJoined).ToList();
                        break;
                }
            };

            return view;
        }

        public override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 0)
            {
                UpdateRecyclerView();
            }
        }

        private void UpdateRecyclerView()
        {
            events = eventsController.Values.Select(x =>
            {
                var userLoginEventModel = new UserLoginEventModel(x)
                {
                    Place = MapsController.Instance.GetAddress(x.Latitude, x.Longitude),
                    IsJoined = userEventsController.Values.Any(y => y.EventId == x.Id && y.UserId == userId)
                };

                return userLoginEventModel;
            }).ToList();

            adapter.Events = events;
        }
    }
}
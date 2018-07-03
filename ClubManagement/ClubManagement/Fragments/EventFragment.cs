using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Adapters;
using ClubManagement.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using ClubManagement.Controllers;
using Android.Preferences;

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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.FragmentEvent, container, false);

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            recyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

            var events = eventsController.Values;

            var adapter = new EventsAdapter(events);
            recyclerView.SetAdapter(adapter);

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabView1);

            tabLayout.AddTab(tabLayout.NewTab().SetText(AllTab));
            tabLayout.AddTab(tabLayout.NewTab().SetText(UpcomingTab));
            tabLayout.AddTab(tabLayout.NewTab().SetText(JoinedTab));

            var preferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            var userId = preferences.GetString("UserId", string.Empty);

            tabLayout.TabSelected += (s, e) =>
            {
                switch (e.Tab.Text)
                {
                    case AllTab:
                        adapter.Events = events;   
                        break;
                    case UpcomingTab:
                        adapter.Events = events.Where(x => userEventsController.Values.Where(y => y.EventId == x.Id).All(y => y.UserId != userId)).ToList();
                        break;
                    case JoinedTab:
                        adapter.Events = events.Where(x => userEventsController.Values.Any(y => y.EventId == x.Id && y.UserId == userId)).ToList();
                        break;
                }
            };

            return view;
        }
    }
}
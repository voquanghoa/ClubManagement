using Android.OS;
using Android.App;
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
using System.Collections.Generic;
using Fragment = Android.Support.V4.App.Fragment;
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using System;
using ClubManagement.Fragments.Bases;

namespace ClubManagement.Fragments
{
	public class EventFragment : SwipeToRefreshDataFragment<List<UserLoginEventModel>>
    {
        private View view;

        private const string AllTab = "All";

        private const string UpcomingTab = "Upcoming";

        private const string JoinedTab = "Joined";

        private UserEventsController userEventsController = UserEventsController.Instance;

        private EventsController eventsController = EventsController.Instance;

        private EventsAdapter adapter;

        private string userId;

		protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.FragmentEvent, container, false);

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            recyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

            var preferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            userId = preferences.GetString("UserId", string.Empty);

            adapter = new EventsAdapter();
            recyclerView.SetAdapter(adapter);

            adapter.ItemClick += (s, e) =>
            {
                var intent = new Intent(view.Context, typeof(EventDetailActivity));

				var eventDetail = JsonConvert.SerializeObject(data[e.Position]);

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
						adapter.Events = data;
                        break;
                    case UpcomingTab:
						adapter.Events = data.Where(x => x.IsJoined).ToList();
                        break;
                    case JoinedTab:
						adapter.Events = data.Where(x => !x.IsJoined).ToList();
                        break;
                }
            };

            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 0)
            {
				UpdateViewData();
			}
        }

		protected override List<UserLoginEventModel> QueryData()
		{
			return eventsController.Values.Select(x =>
            {
                var userLoginEventModel = new UserLoginEventModel(x)
                {
                    Place = MapsController.Instance.GetAddress(x.Latitude, x.Longitude),
                    IsJoined = userEventsController.Values.Any(y => y.EventId == x.Id && y.UserId == userId)
                };

                return userLoginEventModel;
            }).ToList();
		}

		protected override void DisplayData(List<UserLoginEventModel> data)
		{
			adapter.Events = data;
		}
	}
}
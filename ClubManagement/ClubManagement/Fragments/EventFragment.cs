using System;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Adapters;
using ClubManagement.Models;
using System.Linq;
using ClubManagement.Controllers;
using Android.Content;
using ClubManagement.Activities;
using Newtonsoft.Json;
using System.Collections.Generic;
using Android.Support.V4.Widget;
using ClubManagement.Fragments.Bases;
using Android.Support.V7.Widget.Helper;

namespace ClubManagement.Fragments
{
    public class EventFragment : SwipeToRefreshDataFragment<List<UserLoginEventModel>>
    {
        private TabLayout tabLayout;

        private readonly UserEventsController userEventsController = UserEventsController.Instance;

        private readonly EventsController eventsController = EventsController.Instance;

        private readonly EventsAdapter adapter = new EventsAdapter();

        private readonly string userId = AppDataController.Instance.UserId;

        public int SelectedTabIndex { set; get; }

        private readonly ItemTouchHelper itemTouchHelper;

        protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

        public EventFragment()
        {
            data = new List<UserLoginEventModel>();

            adapter.ItemClick += (s, e) =>
            {
                var intent = new Intent(Context, typeof(EventDetailActivity));

                var eventDetail = JsonConvert.SerializeObject(adapter.IsPastTab
                    ? adapter.Events[e.Position]
                    : ((DescriptionItem) adapter.EventItems[e.Position]).EventModel);

                intent.PutExtra("EventDetail", eventDetail);

                StartActivity(intent);
            };
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragmentEvent, container, false);

            InitView(view);

            return view;
        }

        private void InitView(View view)
        {
            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            recyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));
            recyclerView.SetAdapter(adapter);

            tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabView1);
            tabLayout.TabSelected += (s, e) => DisplayData(data);
            tabLayout.GetTabAt(SelectedTabIndex).Select();
        }

        public override void OnResume()
        {
            base.OnResume();
            UpdateViewData();
        }

        protected override List<UserLoginEventModel> QueryData()
        {
            try
            {
                var userEvents = userEventsController.Values;

                return eventsController.Values.Select(x =>
                {
                    var userLoginEventModel = new UserLoginEventModel(x)
                    {
                        IsJoined = userEvents
                            .Any(y => y.EventId == x.Id && y.UserId == userId),
                        NumberOfJoinedUsers = userEvents.Count(e => e.EventId == x.Id)
                    };

                    return userLoginEventModel;
                }).ToList();
            }
            catch (Exception)
            {
                return data;
            }
        }

        protected override void DisplayData(List<UserLoginEventModel> data)
        {
            if (data != null)
            {
                switch (tabLayout.SelectedTabPosition)
                {
                    case 0:
                        adapter.IsPastTab = false;
                        adapter.Events = data.Where(x => x.TimeEnd > DateTime.Now && !x.IsJoined).OrderBy(x => x.TimeStart).ToList();
                        break;
                    case 1:
                        adapter.IsPastTab = false;
                        adapter.Events = data.Where(x => x.TimeEnd > DateTime.Now && x.IsJoined).OrderBy(x => x.TimeStart).ToList();
                        break;
                    case 2:
                        adapter.IsPastTab = true;
                        adapter.Events = data.Where(x => x.TimeEnd <= DateTime.Now).OrderByDescending(x => x.TimeStart).ToList();
                        break;
                }
            }
        }
    }
}
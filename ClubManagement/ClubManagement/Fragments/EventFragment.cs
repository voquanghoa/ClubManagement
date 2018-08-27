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
using ClubManagement.Ultilities;
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

        private readonly EventDialogFragment eventDialogFragment = new EventDialogFragment();

        private FloatingActionButton fabAdd;

        private readonly ItemTouchHelper itemTouchHelper;

        protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

        public EventFragment()
        {
            data = new List<UserLoginEventModel>();

            var swipeToDeleteCallback = new SwipeLeftToDeleteCallback(ItemTouchHelper.ActionStateIdle, ItemTouchHelper.Left);
            swipeToDeleteCallback.SwipeLeft += SwipeToDeleteCallback_SwipeLeft;

            itemTouchHelper = new ItemTouchHelper(swipeToDeleteCallback);

            eventDialogFragment.SaveClick += (s, e) =>
            {
                if (s is EventModel eventModel)
                {
                    var userLoginEventModel = new UserLoginEventModel(eventModel)
                    {
                        IsJoined = false
                    };

                    data.Add(userLoginEventModel);

                    adapter.Events = data;
                }
            };

            adapter.ItemClick += (s, e) =>
            {
                var intent = new Intent(Context, typeof(EventDetailActivity));

                var eventDetail = JsonConvert.SerializeObject(adapter.Events[e.Position]);

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
            fabAdd = view.FindViewById<FloatingActionButton>(Resource.Id.fabAdd);
			fabAdd.ShowIfAdmin();

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            recyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));
            recyclerView.SetAdapter(adapter);

            tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabView1);
            tabLayout.TabSelected += (s, e) => DisplayData(data);

            Context.DoWithAdmin(() =>
            {
                fabAdd.Click += AddEvent_Click;
                itemTouchHelper.AttachToRecyclerView(recyclerView);
            });
        }

        private void SwipeToDeleteCallback_SwipeLeft(object sender, ClickEventArgs e)
        {
            if (sender is ItemEventViewHolder eventViewHolder)
            {
                var id = eventViewHolder.Id;

                Context.ShowConfirmDialog(Resource.String.delete_event, Resource.String.confirm_delete,
                    () =>
                    {
                        data.RemoveAll(x => x.Id.Equals(id));
                        eventsController.Delete(new EventModel() { Id = id });
                        DisplayData(data);
                    }, () =>
                    {
                        DisplayData(data);
                    }).Show();
            }
        }

        private void AddEvent_Click(object sender, System.EventArgs e)
        {
            eventDialogFragment.Show(FragmentManager, null);
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
                        adapter.Events = data.Where(x => x.Time >= DateTime.Now && !x.IsJoined).ToList();
                        fabAdd.ShowIfAdmin();
                        break;
                    case 1:
                        fabAdd.Visibility = ViewStates.Gone;
                        adapter.Events = data.Where(x => x.Time >= DateTime.Now && x.IsJoined).ToList();
                        break;
                    case 2:
                        fabAdd.Visibility = ViewStates.Gone;
                        adapter.Events = data.Where(x => x.Time < DateTime.Now).ToList();
                        break;
                }
            }
        }
    }
}
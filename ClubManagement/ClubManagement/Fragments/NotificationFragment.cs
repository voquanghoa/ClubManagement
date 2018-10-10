using Android.Content;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Activities;
using ClubManagement.Adapters;
using ClubManagement.Controllers;
using ClubManagement.Fragments.Bases;
using ClubManagement.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Fragments
{
    public class NotificationFragment : SwipeToRefreshDataFragment<List<NotificationModel>>
    {
        private readonly NotificationsAdapter adapter = new NotificationsAdapter();

        protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

        public NotificationFragment()
        {
            adapter.ItemClick += (s, e) =>
            {
                if (s is NotificationModel notification)
                {
                    Task.Run(() =>
                    {
                        notification.IsNew = false;
                        NotificationsController.Instance.Edit(notification);

                        var intent = new Intent(Context, typeof(EventDetailActivity));

                        var eventDetail = JsonConvert.SerializeObject(EventsController.Instance
                            .Values.Find(x => x.Id == notification.TypeId));

                        intent.PutExtra("EventDetail", eventDetail);

                        StartActivity(intent);
                    });
                }
            };
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragmentNotification, container, false);

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            recyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));
            recyclerView.SetAdapter(adapter);

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            UpdateViewData();
        }

        protected override void DisplayData(List<NotificationModel> data)
        {
            adapter.Notifications = data;
        }

        protected override List<NotificationModel> QueryData()
        {
            return NotificationsController.Instance.Values.ToList();
        }
    }
}
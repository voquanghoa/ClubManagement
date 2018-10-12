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
using ClubManagement.Ultilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Fragments
{
    public class NotificationFragment : SwipeToRefreshDataFragment<List<NotificationModel>>
    {
        private readonly NotificationsAdapter adapter = new NotificationsAdapter();

        protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

        public event EventHandler ItemClick;

        public NotificationFragment()
        {
            adapter.ItemClick += (s, e) =>
            {
                if (s is NotificationModel notification)
                {
                    notification.IsNew = false;
                    NotificationsController.Instance.Edit(notification);

                    switch (notification.Type)
                    {
                        case AppConstantValues.NotificationEditEvent:
                            var intent = new Intent(Context, typeof(EventDetailActivity));

                            var eventDetail = JsonConvert.SerializeObject(EventsController.Instance
                                .ValuesJustUpdate.Find(x => x.Id == notification.TypeId));

                            intent.PutExtra("EventDetail", eventDetail);

                            StartActivity(intent);

                            break;
                        case AppConstantValues.NotificationEditFee:
                            var isPaid = AppDataController.Instance.GetListMoneyState()
                                .Find(x => x.MoneyModel.Id.Equals(notification.TypeId))
                                .IsPaid;

                            ItemClick?.Invoke(isPaid, null);

                            break;
                    }
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
            EventsController.Instance.UpdateValues();
            MoneysController.Instance.UpdateValues();

            return NotificationsController.Instance.Values.OrderByDescending(x=>x.LastUpdate).ToList();
        }
    }
}
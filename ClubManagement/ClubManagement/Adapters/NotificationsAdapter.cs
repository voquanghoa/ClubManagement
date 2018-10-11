using System;
using Android.Views;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using ClubManagement.Models;

namespace ClubManagement.Adapters
{
    class NotificationsAdapter : RecyclerView.Adapter
    {
        public event EventHandler ItemClick;

        private List<NotificationModel> notifications = new List<NotificationModel>();

        public List<NotificationModel> Notifications
        {
            set
            {
                notifications = value;
                NotifyDataSetChanged();
            }
            get => notifications;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var id = Resource.Layout.ItemRecycleViewNotification;
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(id, parent, false);

            var vh = new NotificationsAdapterViewHolder(itemView);
            vh.ClickHander += ItemClick;

            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            if (viewHolder is NotificationsAdapterViewHolder vh)
            {
                vh.NotificationModel = notifications[position];
            }
        }

        public override int ItemCount => notifications.Count;
    }
}
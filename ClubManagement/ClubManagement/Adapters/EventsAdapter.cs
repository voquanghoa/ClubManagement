using System;
using Android.Views;
using Android.Support.V7.Widget;
using ClubManagement.Models;
using System.Collections.Generic;

namespace ClubManagement.Adapters
{
    public class EventsAdapter : RecyclerView.Adapter
    {
		private List<UserLoginEventModel> events = new List<UserLoginEventModel>();

        public event EventHandler<ClickEventArgs> ItemClick;

        public List<UserLoginEventModel> Events
        {
            set
            {
                events = value;
                NotifyDataSetChanged();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var id = Resource.Layout.ItemEvent;
            var itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var viewHolder = new EventViewHolder(itemView);

            viewHolder.ClickHander += ItemClick;

            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            ((EventViewHolder)viewHolder).EventModel = events[position];
        }

        public override int ItemCount => events.Count;
    }
}
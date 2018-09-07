using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using ClubManagement.Models;
using System.Collections.Generic;

namespace ClubManagement.Adapters
{
    class GuestsAdapter : RecyclerView.Adapter
    {
        private List<GuestModel> guests;

        public List<GuestModel> Guests
        {
            set
            {
                guests = value;
                NotifyDataSetChanged();
            }
        }

        public event EventHandler<ClickEventArgs> ItemClick;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var id = Resource.Layout.ItemGuest;
            var itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            return new GuestAdapterViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            if (viewHolder is GuestAdapterViewHolder holder)
            {
                holder.UserModel = guests[position];
            }
        }

        public override int ItemCount => guests.Count;
    }
}
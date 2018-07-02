using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace ClubManagement.Adapters
{
    class EventAdapter : RecyclerView.Adapter
    {
        public event EventHandler<EventAdapterClickEventArgs> ItemClick;
        public event EventHandler<EventAdapterClickEventArgs> ItemLongClick;
        string[] items;

        public EventAdapter(string[] data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            //var id = Resource.Layout.__YOUR_ITEM_HERE;
            //itemView = LayoutInflater.From(parent.Context).
            //       Inflate(id, parent, false);

            var vh = new EventAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as EventViewHolder;
            //holder.TextView.Text = items[position];
        }

        public override int ItemCount => items.Length;

        void OnClick(EventAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(EventAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class EventAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
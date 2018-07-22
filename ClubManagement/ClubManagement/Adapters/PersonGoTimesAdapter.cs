using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using ClubManagement.Models;
using System.Collections.Generic;

namespace ClubManagement.Adapters
{
    class PersonGoTimesAdapter : RecyclerView.Adapter
    {
        private List<PersonGoTimeModel> personGoTimes;

        public List<PersonGoTimeModel> PersonGoTimes
        {
            set
            {
                personGoTimes = value;
                NotifyDataSetChanged();
            }
        }

        public event EventHandler<ClickEventArgs> ItemClick;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var id = Resource.Layout.ItemPersonGoTime;
            var itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var viewHoler = new PersonsGoTimeAdapterViewHolder(itemView);

            viewHoler.ClickHander += ItemClick;

            return viewHoler;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            ((PersonsGoTimeAdapterViewHolder)viewHolder).PersonGoTimeModel = personGoTimes[position];
        }

        public override int ItemCount => personGoTimes.Count;
    }
}
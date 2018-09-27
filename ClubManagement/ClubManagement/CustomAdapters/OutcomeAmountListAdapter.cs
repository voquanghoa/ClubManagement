using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Models;

namespace ClubManagement.CustomAdapters
{
    public class OutcomeAmountListAdapter : RecyclerView.Adapter
    {
        public List<OutcomeAmountItem> OutcomeAmountItems { get; set; }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is OutcomeAmountItemViewHolder viewHolder)
            {
                viewHolder.OutcomeAmountItem = OutcomeAmountItems[position];
                viewHolder.Pos = position;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.outcome_fee_item, parent, false);
            return new OutcomeAmountItemViewHolder(itemView);
        }

        public override int ItemCount => OutcomeAmountItems.Count;
    }
}
using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using ClubManagement.Models;

namespace ClubManagement.Adapters
{
    public class FeeGroupAdapter : RecyclerView.Adapter
    {
        private List<FeeGroupModel> feeGroups;

        public event EventHandler<ClickEventArgs> ItemClick;

        public FeeGroupAdapter(List<FeeGroupModel> feeGroups)
        {
            this.feeGroups = feeGroups;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var id = Resource.Layout.ItemFeeGroup;
            var itemView = LayoutInflater.From(parent.Context).
                Inflate(id, parent, false);

            var vh = new FeeGroupAdapterViewHolder(itemView);
            vh.ClickHander += ItemClick;

            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            if (viewHolder is FeeGroupAdapterViewHolder vh)
            {
                vh.FeeGroup = feeGroups[position];
            }
        }

        public override int ItemCount => feeGroups.Count;
    }
}
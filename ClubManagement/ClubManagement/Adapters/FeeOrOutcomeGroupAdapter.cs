using System;

using Android.Views;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using ClubManagement.Models;

namespace ClubManagement.Adapters
{
    public class FeeOrOutcomeGroupAdapter : RecyclerView.Adapter
    {
        private List<FeeOrOutcomeGroupModel> feeOrOutcomeGroups;

        public event EventHandler<ClickEventArgs> ItemClick;

        public FeeOrOutcomeGroupAdapter(List<FeeOrOutcomeGroupModel> feeOrOutcomeGroups)
        {
            this.feeOrOutcomeGroups = feeOrOutcomeGroups;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var id = Resource.Layout.ItemFeeGroup;
            var itemView = LayoutInflater.From(parent.Context).
                Inflate(id, parent, false);

            var vh = new FeeOrOutcomeGroupAdapterViewHolder(itemView);
            vh.ClickHander += ItemClick;

            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            if (viewHolder is FeeOrOutcomeGroupAdapterViewHolder vh)
            {
                vh.FeeGroup = feeOrOutcomeGroups[position];
            }
        }

        public override int ItemCount => feeOrOutcomeGroups.Count;
    }
}
using Android.Views;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using ClubManagement.Models;

namespace ClubManagement.Adapters
{
    public class BalancesAdapter : RecyclerView.Adapter
    {
        private List<BalanceModel> balances;

        public BalancesAdapter(List<BalanceModel> balances)
        {
            this.balances = balances;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var id = Resource.Layout.ItemBalance;
            var itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            return new BalanceAdapterViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            ((BalanceAdapterViewHolder)viewHolder).BalanceModel = balances[position];
        }

        public override int ItemCount => balances.Count;
    }
}
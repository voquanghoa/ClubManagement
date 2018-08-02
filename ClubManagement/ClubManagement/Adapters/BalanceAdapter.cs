using Android.Views;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using ClubManagement.Models;
using ClubManagement.Fragments;

namespace ClubManagement.Adapters
{
    public class BalancesAdapter : RecyclerView.Adapter
    {
        private List<OutcomeModel> balances;

        private BalancesFragment.Type type;

        public List<OutcomeModel> Balances
        {
            set
            {
                balances = value;
                NotifyDataSetChanged();
            }
        }

        public BalancesAdapter(BalancesFragment.Type type)
        {
            this.type = type;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var id = Resource.Layout.ItemBalance;
            var itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            return new BalanceAdapterViewHolder(itemView, type);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            if (viewHolder is BalanceAdapterViewHolder holder) holder.BalanceModel = balances[position];
        }

        public override int ItemCount => balances.Count;
    }
}
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Models;

namespace ClubManagement.CustomAdapters
{
    class MoneyListAdapter : RecyclerView.Adapter
    {
        private readonly List<MoneyState> moneyStates;

        public MoneyListAdapter(List<MoneyState> moneyStates)
        {
            this.moneyStates = moneyStates;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is MoneyViewHolder viewHolder) viewHolder.MoneyState = moneyStates[position];
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.recyclerview_money_list_item, parent, false);
            return new MoneyViewHolder(itemView);
        }

        public override int ItemCount => moneyStates.Count;
    }
}
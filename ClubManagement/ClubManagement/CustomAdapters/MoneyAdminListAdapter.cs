using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;

namespace ClubManagement.CustomAdapters
{
    public class MoneyAdminListAdapter : RecyclerView.Adapter
    {
        private readonly List<MoneyAdminState> moneyAdminStates;

        private readonly TextView tvState;

        private readonly string moneyId;

        public MoneyAdminListAdapter(List<MoneyAdminState> moneyAdminStates, TextView tvState, string moneyId)
        {
            this.moneyAdminStates = moneyAdminStates;
            this.tvState = tvState;
            this.moneyId = moneyId;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is MoneyAdminViewHolder viewHolder)
            {
                viewHolder.MoneyAdminState = moneyAdminStates[position];
                viewHolder.TvState = tvState;
                viewHolder.MoneyId = moneyId;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.recyclerview_money_admin_item, parent, false);
            return new MoneyAdminViewHolder(itemView);
        }

        public override int ItemCount => moneyAdminStates.Count;
    }
}
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Activities;
using ClubManagement.Interfaces;
using ClubManagement.Models;

namespace ClubManagement.CustomAdapters
{
    public class MoneyListAdapter : RecyclerView.Adapter, IItemClickListener
    {
        private List<MoneyState> moneyStates = new List<MoneyState>();

        public List<MoneyState> MoneyStates
        {
            get => moneyStates;
            set
            {
				moneyStates.Clear();
				moneyStates.AddRange(value);
                NotifyDataSetChanged();
			}
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (!(holder is MoneyViewHolder viewHolder)) return;
            viewHolder.MoneyState = moneyStates[position];
            viewHolder.ItemClickListener = this;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.recyclerview_money_list_item, parent, false);
            return new MoneyViewHolder(itemView);
        }

        public override int ItemCount => moneyStates.Count;

        public void OnClick(View view, int position)
        {
            var intent = new Intent(view.Context, typeof(MoneyDetailActivity));
            var moneyState = moneyStates[position].MoneyModel;
            intent.PutExtra("Budget", moneyState.Amount);
            intent.PutExtra("Description", moneyState.Description);
            intent.PutExtra("MoneyId", moneyState.Id);
            intent.PutExtra("Time", moneyState.Time.ToShortDateString());
            view.Context.StartActivity(intent);
        }
    }
}
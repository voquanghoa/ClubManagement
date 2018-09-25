using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Models;

namespace ClubManagement.CustomAdapters
{
    public class IncomeListAdapter : RecyclerView.Adapter
    {
        private List<IncomeModel> incomeModels = new List<IncomeModel>();

        public List<IncomeModel> IncomeModels
        {
            get => incomeModels;
            set
            {
                incomeModels = value;
                NotifyDataSetChanged();
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is IncomeViewHolder viewHolder)
            {
                viewHolder.IncomeModel = IncomeModels[position];
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.income_item, parent, false);
            return new IncomeViewHolder(itemView);
        }

        public override int ItemCount => IncomeModels.Count;
    }
}
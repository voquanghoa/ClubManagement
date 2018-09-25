using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Models;

namespace ClubManagement.CustomAdapters
{
    public class OutcomeListAdapter : RecyclerView.Adapter
    {
        private List<OutcomeModel> outcomeModels = new List<OutcomeModel>();

        public List<OutcomeModel> OutcomeModels
        {
            get => outcomeModels;
            set
            {
                outcomeModels = value;
                NotifyDataSetChanged();
            }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is OutcomeViewHolder viewHolder)
            {
                viewHolder.OutcomeModel = OutcomeModels[position];
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.outcome_item, parent, false);
            return new OutcomeViewHolder(itemView);
        }

        public override int ItemCount => OutcomeModels.Count;
    }
}
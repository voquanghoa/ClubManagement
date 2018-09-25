using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Models;

namespace ClubManagement.CustomAdapters
{
    public class OutcomeListAdapter : RecyclerView.Adapter
    {
        private List<OutcomeModel> outcomeModels = new List<OutcomeModel>();

        private List<OutcomeItem> outcomeItems = new List<OutcomeItem>();

        public event EventHandler DeleteClick;

        public List<OutcomeModel> OutcomeModels
        {
            get => outcomeModels;
            set
            {
                outcomeModels = value;
                outcomeItems = GetListOutcomeItems(value);
                NotifyDataSetChanged();
            }
        }

        public override int GetItemViewType(int position)
        {
            return outcomeItems[position].GetType();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is OutcomeViewHolder viewHolder)
            {
                viewHolder.OutcomeModel = ((OutcomeDetailItem) outcomeItems[position]).OutcomeModel;
                viewHolder.DeleteClick += DeleteClick;
            }
            else if (holder is OutcomeTimeViewHolder outcomeTimeViewHolder)
            {
                outcomeTimeViewHolder.Time = ((OutcomeTimeItem) outcomeItems[position]).Time;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == OutcomeItem.DetailType)
            {
                var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.outcome_item, parent, false);
                return new OutcomeViewHolder(itemView);
            }

            if (viewType == OutcomeItem.TimeType)
            {
                var itemView = LayoutInflater.From(parent.Context)
                    .Inflate(Resource.Layout.outcome_time_item, parent, false);
                return new OutcomeTimeViewHolder(itemView);
            }

            return null;
        }

        public override int ItemCount => outcomeItems.Count;

        private List<OutcomeItem> GetListOutcomeItems(List<OutcomeModel> outcomes)
        {
            if (!outcomes.Any())
            {
                return new List<OutcomeItem>();
            }

            var currentTime = outcomes[0].Date.Date;

            var result = new List<OutcomeItem>
            {
                new OutcomeTimeItem
                {
                    Time = currentTime
                }
            };

            foreach (var outcome in outcomes)
            {
                if (!outcome.Date.Date.Equals(currentTime.Date)) 
                {
                    currentTime = outcome.Date.Date;
                    result.Add(new OutcomeTimeItem
                    {
                        Time = currentTime
                    });
                }
                result.Add(new OutcomeDetailItem
                {
                    OutcomeModel = outcome
                });
            }

            return result;
        }
    }
}
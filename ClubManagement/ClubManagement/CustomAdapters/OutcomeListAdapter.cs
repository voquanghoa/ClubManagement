using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Activities;
using ClubManagement.Interfaces;
using ClubManagement.Models;
using Newtonsoft.Json;

namespace ClubManagement.CustomAdapters
{
    public class OutcomeListAdapter : RecyclerView.Adapter, IItemClickListener
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
                viewHolder.ItemClick = this;
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

        public void OnClick(View view, int position)
        {
            var outcome = ((OutcomeDetailItem) outcomeItems[position]).OutcomeModel;
            var intent = new Intent(view.Context, typeof(OutcomeDetailActivity));
            intent.PutExtra("outcome", JsonConvert.SerializeObject(outcome));
            view.Context.StartActivity(intent);
        }
    }
}
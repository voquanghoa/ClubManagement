using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Activities;
using ClubManagement.Interfaces;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using Newtonsoft.Json;
using Plugin.Connectivity;

namespace ClubManagement.CustomAdapters
{
    public class MoneyListAdapter : RecyclerView.Adapter, IItemClickListener
    {
        private readonly List<MoneyState> moneyStates = new List<MoneyState>();

        public event EventHandler ItemDeleteClick;

        public List<MoneyState> MoneyStates
        {
            get => moneyStates;
            set
            {
                moneyStates.Clear();
                moneyStates.AddRange(value);
                feeItems = GetListFeeItem(value);
                NotifyDataSetChanged();
            }
        }

        private List<FeeItem> feeItems = new List<FeeItem>();

        public override int GetItemViewType(int position)
        {
            return feeItems[position].GetType();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is MoneyViewHolder moneyViewHolder)
            {
                moneyViewHolder.MoneyState = ((FeeDetailItem) feeItems[position]).MoneyState;
                moneyViewHolder.DeleteClick += ItemDeleteClick;
                moneyViewHolder.ItemClickListener = this;
            }
            else if (holder is MoneyDeadlineTimeViewHolder moneyDeadlineTimeViewHolder)
            {
                moneyDeadlineTimeViewHolder.DeadlineTime = ((FeeDeadlineTimeItem) feeItems[position]).DeadlineTime;
            }
            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == FeeItem.TypeDetail)
            {
                var itemView = LayoutInflater.From(parent.Context)
                    .Inflate(Resource.Layout.recyclerview_money_list_item, parent, false);
                return new MoneyViewHolder(itemView);
            }
            else if (viewType == FeeItem.TypeTimeHeader)
            {
                var itemView = LayoutInflater.From(parent.Context)
                    .Inflate(Resource.Layout.recyclerview_money_list_deadline_time_item, parent, false);
                return new MoneyDeadlineTimeViewHolder(itemView);
            }

            return null;
        }

        public override int ItemCount => feeItems.Count;

        public void OnClick(View view, int position)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Toast.MakeText(view.Context, view.Context.Resources.GetString(Resource.String.no_internet_connection),
                    ToastLength.Short).Show();
                return;
            }

            view.Context.DoWithAdmin(() =>
            {
                var intent = new Intent(view.Context, typeof(MoneyDetailActivity));
                var moneyModel = ((FeeDetailItem) feeItems[position]).MoneyState.MoneyModel;
                intent.PutExtra("MoneyModel", JsonConvert.SerializeObject(moneyModel));
                view.Context.StartActivity(intent);
            });
        }

        private List<FeeItem> GetListFeeItem(List<MoneyState> moneyStates)
        {
            if (!moneyStates.Any()) return new List<FeeItem>();
            var currentTime = moneyStates[0].MoneyModel.Time.Date;

            var result = new List<FeeItem>
            {
                new FeeDeadlineTimeItem
                {
                    DeadlineTime = currentTime
                }
            };
            foreach (var moneyState in moneyStates)
            {
                if (moneyState.MoneyModel.Time.Date.Equals(currentTime))
                {
                    result.Add(new FeeDetailItem
                    {
                        MoneyState = moneyState
                    });
                }
                else
                {
                    currentTime = moneyState.MoneyModel.Time.Date;
                    result.Add(new FeeDeadlineTimeItem
                    {
                        DeadlineTime = currentTime
                    });
                    result.Add(new FeeDetailItem
                    {
                        MoneyState = moneyState
                    });
                }
            }
            return result;
        }
    }
}
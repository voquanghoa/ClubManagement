using System;
using Android.Views;
using Android.Support.V7.Widget;
using ClubManagement.Models;
using System.Collections.Generic;
using System.Linq;
using ClubManagement.Ultilities;

namespace ClubManagement.Adapters
{
    public class EventsAdapter : RecyclerView.Adapter
    {
        public List<EventItem> EventItems = new List<EventItem>();

        private List<UserLoginEventModel> events = new List<UserLoginEventModel>();

        public event EventHandler<ClickEventArgs> ItemClick;

        public bool IsPastTab { get; set; }

        public List<UserLoginEventModel> Events
        {
            set
            {
                events = value;
                EventItems = GetEventItems(value);
                NotifyDataSetChanged();
            }
            get => events;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == EventItem.TypeHeader)
            {
                var itemView = LayoutInflater.From(parent.Context).Inflate(
                    Resource.Layout.item_header_recyclerview_events, parent,
                    false);
                return new ItemEventHeaderViewHolder(itemView);
            }
            if (viewType == EventItem.TypeDescription || IsPastTab)
            {
                var itemView = LayoutInflater.From(parent.Context)
                    .Inflate(Resource.Layout.item_recyclerview_events, parent, false);
                var viewHolder = new ItemEventViewHolder(itemView);
                viewHolder.ClickHander += ItemClick;
                return viewHolder;
            }

            return null;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            if (IsPastTab)
            {
                ((ItemEventViewHolder) viewHolder).EventModel = events[position];
                return;
            }
            if (GetItemViewType(position) == EventItem.TypeHeader)
            {
                ((ItemEventHeaderViewHolder) viewHolder).Header = ((EventHeaderItem) EventItems[position]).Header;
            }
            else if (GetItemViewType(position) == EventItem.TypeDescription)
            {
                ((ItemEventViewHolder) viewHolder).EventModel = ((DescriptionItem) EventItems[position]).EventModel;
            }
        }

        public override int GetItemViewType(int position)
        {
            return IsPastTab ? base.GetItemViewType(position) : EventItems[position].GetType();
        }

        public override int ItemCount => IsPastTab ? events.Count : EventItems.Count;

        private List<EventItem> GetEventItems(List<UserLoginEventModel> eventModels)
        {
            var eventsWithTimeHeader = new Dictionary<string, List<UserLoginEventModel>>
            {
                { AppConstantValues.EventListHeaderToday, new List<UserLoginEventModel>()},
                { AppConstantValues.EventListHeaderTomorrow, new List<UserLoginEventModel>()},
                { AppConstantValues.EventListHeaderThisWeek, new List<UserLoginEventModel>()},
                { AppConstantValues.EventListHeaderNextWeek, new List<UserLoginEventModel>()},
                { AppConstantValues.EventListHeaderOther, new List<UserLoginEventModel>()}
            };

            foreach (var eventModel in eventModels)
            {
                if (eventModel.Time.IsToday())
                {
                    eventsWithTimeHeader[AppConstantValues.EventListHeaderToday].Add(eventModel);
                }
                else if (eventModel.Time.IsTomorrow())
                {
                    eventsWithTimeHeader[AppConstantValues.EventListHeaderTomorrow].Add(eventModel);
                }
                else if (eventModel.Time.IsInThisWeek())
                {
                    eventsWithTimeHeader[AppConstantValues.EventListHeaderThisWeek].Add(eventModel);
                }
                else if (eventModel.Time.IsInNextWeek())
                {
                    eventsWithTimeHeader[AppConstantValues.EventListHeaderNextWeek].Add(eventModel);
                }
                else
                {
                    eventsWithTimeHeader[AppConstantValues.EventListHeaderOther].Add(eventModel);
                }
            }

            var eventItems = new List<EventItem>();

			foreach (var item in eventsWithTimeHeader.Where(x => x.Value.Any()))
            {
                eventItems.Add(new EventHeaderItem
                {
                    Header = item.Key
                });
                item.Value.ForEach(x => eventItems.Add(new DescriptionItem
                {
                    EventModel = x
                }));
            }

            return eventItems;
        }
    }
}
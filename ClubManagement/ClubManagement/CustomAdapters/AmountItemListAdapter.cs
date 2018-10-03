using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Models;

namespace ClubManagement.CustomAdapters
{
    public class AmountItemListAdapter : RecyclerView.Adapter
    {
        public List<AmountItem> Items { get; set; }

        private bool isDeleting = false;

        public bool IsDeleting
        {
            get => isDeleting;

            set
            {
                isDeleting = value;
                Items.ForEach(x => x.IsDeleting = value);
                NotifyDataSetChanged();
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is AmountItemViewHolder viewHolder)
            {
                viewHolder.Item = Items[position];
                viewHolder.IsDeleting = IsDeleting;
                viewHolder.Click += (s, e) =>
                {
                    if (s is bool isChoose)
                    {
                        Items[position].IsChooseToDelete = isChoose;
                        viewHolder.Item = Items[position];
                    }
                };
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_amount, parent, false);
            return new AmountItemViewHolder(view);
        }

        public override int ItemCount => Items.Count;

        public void AddItem(AmountItem item)
        {
            Items.Add(item);
            NotifyDataSetChanged();
        }

        public void DeleteChoosedItem()
        {
            foreach (var item in Items.ToList())
            {
                if (item.IsChooseToDelete)
                {
                    Items.Remove(item);
                }
            }
            NotifyDataSetChanged();
        }
    }
}
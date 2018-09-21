using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using ClubManagement.Models;

namespace ClubManagement.Adapters
{
    public class FeeGroupAdapterViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.imgView)]
        private ImageView imageView;

        [InjectView(Resource.Id.tvTitle)]
        private TextView tvTitle;

        private int id;

        public event EventHandler<ClickEventArgs> ClickHander;

        public FeeGroupModel FeeGroup
        {
            set
            {
                imageView.SetImageResource(value.ImageId);
                tvTitle.Text = ItemView.Context.GetString(value.TitleId);
                id = value.Id;
            }
        }

        public FeeGroupAdapterViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);

            itemView.Click += (s, e) =>
            {
                ClickHander?.Invoke(id, null);
            };
        }
    }
}
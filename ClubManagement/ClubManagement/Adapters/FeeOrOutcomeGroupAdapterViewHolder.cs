using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using ClubManagement.Models;

namespace ClubManagement.Adapters
{
    public class FeeOrOutcomeGroupAdapterViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.imgView)]
        private ImageView imageView;

        [InjectView(Resource.Id.tvTitle)]
        private TextView tvTitle;

        private string id;

        public event EventHandler<ClickEventArgs> ClickHander;

        public FeeOrOutcomeGroupModel FeeGroup
        {
            set
            {
                imageView.SetImageResource(value.ImageId);
                tvTitle.Text = ItemView.Context.GetString(value.TitleId);
                id = value.Id;
            }
        }

        public FeeOrOutcomeGroupAdapterViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);

            itemView.Click += (s, e) =>
            {
                ClickHander?.Invoke(id, null);
            };
        }
    }
}
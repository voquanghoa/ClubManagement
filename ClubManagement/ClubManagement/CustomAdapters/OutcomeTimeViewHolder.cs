using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ClubManagement.CustomAdapters
{
    public class OutcomeTimeViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.tvOutcomeTime)]
        private TextView tvOutcomeTime;

        public DateTime Time
        {
            set => tvOutcomeTime.Text = value.ToString("MMM dd, yyyy");
        }

        public OutcomeTimeViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }
    }
}
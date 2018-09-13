using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ClubManagement.CustomAdapters
{
    public class MoneyDeadlineTimeViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.tvDeadlineTime)]
        private TextView tvDeadlineTime;

        public DateTime DeadlineTime
        {
            set => tvDeadlineTime.Text = $"Deadline: {value:MMM dd, yyyy}";
        }
        public MoneyDeadlineTimeViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }
    }
}
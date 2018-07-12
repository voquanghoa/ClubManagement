using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;

namespace ClubManagement.Adapters
{
    public class BalanceAdapterViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.tvTitle)]
        private TextView tvTitle;

        [InjectView(Resource.Id.tvDescription)]
        private TextView tvDescription;

        [InjectView(Resource.Id.tvMoney)]
        private TextView tvMoney;

        [InjectView(Resource.Id.tvDate)]
        private TextView tvDate;

        public BalanceModel BalanceModel
        {
            set
            {
                tvTitle.Text = value.Title;
                tvDescription.Text = value.Description;
                tvMoney.Text = value.Money.ToString();
                tvDate.Text = value.Date.ToShortDateString();
            }
        }

        public BalanceAdapterViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }
    }
}
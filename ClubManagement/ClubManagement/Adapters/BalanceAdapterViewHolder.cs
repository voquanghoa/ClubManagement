using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Fragments;
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

        private BalancesFragment.Type type;

        public OutcomeModel BalanceModel
        {
            set
            {
                var numberSign = BalancesFragment.Type.Income == type ? "+" : "-";

                tvTitle.Text = value.Title;
                tvDescription.Text = value.Description;
                var money = value.Amount * 1000;
                tvMoney.Text = $"{numberSign}{money}đ";
                tvDate.Text = value.Date.ToShortDateString();
            }
        }

        public BalanceAdapterViewHolder(View itemView, BalancesFragment.Type type) : base(itemView)
        {
            this.type = type;
            Cheeseknife.Inject(this, itemView);
        }
    }
}
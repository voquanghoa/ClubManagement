using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;
using ClubManagement.Ultilities;

namespace ClubManagement.CustomAdapters
{
    public class OutcomeAmountItemViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.tvItemName)] private TextView tvItemName;

        [InjectView(Resource.Id.tvAmount)] private TextView tvAmount;

        [InjectView(Resource.Id.itemView)] private LinearLayout itemView;

        public OutcomeAmountItem OutcomeAmountItem
        {
            set
            {
                tvItemName.Text = value.Name;
                tvAmount.Text = value.Amount.ToCurrency();
            }
        }

        public int Pos
        {
            set => InitTheme(value);
        }

        public OutcomeAmountItemViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }

        private void InitTheme(int pos)
        {
            var isActivited = pos % 2 == 0;
            itemView.Activated = isActivited;
            tvAmount.Activated = isActivited;
            tvItemName.Activated = isActivited;
        }
    }
}
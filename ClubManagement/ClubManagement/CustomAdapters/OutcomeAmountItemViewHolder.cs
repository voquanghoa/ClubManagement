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

        [InjectView(Resource.Id.itemView)] private View itemView;

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
            if (pos % 2 == 1)
            {
                itemView.SetBackgroundColor(GetColorResource(Resource.Color.odd_line_color));
                tvAmount.SetTextColor(GetColorResource(Resource.Color.text_color_black));
                tvItemName.SetTextColor(GetColorResource(Resource.Color.text_color_black));
            }
            else
            {
                itemView.SetBackgroundColor(GetColorResource(Resource.Color.even_line_color));
                tvAmount.SetTextColor(GetColorResource(Resource.Color.text_color_white));
                tvItemName.SetTextColor(GetColorResource(Resource.Color.text_color_white));
            }
        }

        private Color GetColorResource(int colorId)
        {
            return ItemView.Context.Resources.GetColor(colorId, null);
        }
    }
}
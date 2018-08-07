using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Interfaces;
using ClubManagement.Models;

namespace ClubManagement.CustomAdapters
{
    public class MoneyViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        [InjectView(Resource.Id.tvTitle)]
        private TextView tvTitle;

        [InjectView(Resource.Id.tvDescription)]
        private TextView tvDescription;

        [InjectView(Resource.Id.imgState)]
        private ImageView imgState;

        public IItemClickListener ItemClickListener { get; set; }

        public MoneyState MoneyState
        {
            set
            {
                tvTitle.Text = $"{value.MoneyModel.Time.ToShortDateString()} - Budget : {value.MoneyModel.Amount * 1000} đ";
                tvDescription.Text = value.MoneyModel.Description;
                imgState.SetImageResource(value.IsPaid ? Resource.Drawable.icon_paid : Resource.Drawable.icon_unpaid);
            }
        }

        public MoneyViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
            itemView.SetOnClickListener(this);
        }

        public void OnClick(View v)
        {
            ItemClickListener.OnClick(v, AdapterPosition);
        }
    }
}
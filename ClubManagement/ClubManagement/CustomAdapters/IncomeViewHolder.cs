using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;
using ClubManagement.Ultilities;

namespace ClubManagement.CustomAdapters
{
    public class IncomeViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.imgGroup)] private ImageView imgGroup;

        [InjectView(Resource.Id.tvTitle)] private TextView tvTitle;

        [InjectView(Resource.Id.tvAmount)] private TextView tvAmount;

        [InjectView(Resource.Id.tvTime)] private TextView tvTime;

        [InjectView(Resource.Id.pbAmount)] private ProgressBar pbAmount;

        public IncomeModel IncomeModel
        {
            set
            {
                tvTitle.Text = value.Description;
                tvTime.Text = value.Time.ToString("MMM dd, yyyy");
                var totalAmount = value.NumberOfUsers * value.Amount;
                var paidAmount = value.NumberOfPaidUsers * value.Amount;
                var progress = (int) (paidAmount * 100 / totalAmount);
                tvAmount.Text = $"{paidAmount:N0}/{totalAmount.ToCurrency()}";
                pbAmount.Progress = progress;
                pbAmount.ProgressTintList = ItemView.Context.Resources.GetColorStateList(paidAmount < totalAmount ? Resource.Color.color_red : Resource.Color.color_blue, null);
            }
        }

        public IncomeViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }
    }
}
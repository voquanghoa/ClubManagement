using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;

namespace ClubManagement.CustomAdapters
{
    public class MoneyAdminViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.tvUser)] private TextView tvUser;

        [InjectView(Resource.Id.imgState)] private ImageView imgState;

        public MoneyAdminState MoneyAdminState
        {
            set
            {
                tvUser.Text = value.User.Name;
                imgState.SetImageResource(value.IsPaid ? Resource.Drawable.icon_paid : Resource.Drawable.icon_unpaid);
            }
        }

        public MoneyAdminViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }
    }
}
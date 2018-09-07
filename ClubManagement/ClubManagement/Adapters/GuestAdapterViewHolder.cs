using Android.Views;
using Android.Support.V7.Widget;
using Android.Widget;
using ClubManagement.Models;
using System.Linq;
using Square.Picasso;

namespace ClubManagement.Adapters
{
    public class GuestAdapterViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.tvName)]
        private TextView tvName;

        [InjectView(Resource.Id.tvHeadLetter)]
        private TextView tvHeadLetter;

        [InjectView(Resource.Id.civAvatar)]
        private ImageView civAvatar;

        public GuestModel UserModel
        {
            set
            {
                tvName.Text = value.Name;

                tvHeadLetter.Text = value.IsHeadLetter ? value.Name.FirstOrDefault().ToString().ToUpper() : "";

                if (!string.IsNullOrEmpty(value.Avatar))
                {
                    Picasso.With(ItemView.Context).Load(value.Avatar).Fit().Into(civAvatar);
                }
                else
                {
                    civAvatar.SetImageResource(Resource.Drawable.icon_user);
                }
            }
        }

        public GuestAdapterViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }
    }
}
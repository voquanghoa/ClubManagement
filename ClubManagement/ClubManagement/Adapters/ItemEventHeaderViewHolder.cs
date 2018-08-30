using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ClubManagement.Adapters
{
    public class ItemEventHeaderViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.tvHeader)] private TextView tvHeader;

        public string Header
        {
            set => tvHeader.Text = value;
        }

        public ItemEventHeaderViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }
    }
}
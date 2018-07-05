using Android.Support.V7.Widget;
using Android.Views;

namespace ClubManagement.CustomAdapters
{
    class MoneyViewHolder : RecyclerView.ViewHolder
    {
        public MoneyViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }
    }
}
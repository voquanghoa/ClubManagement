using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;
using ClubManagement.Ultilities;

namespace ClubManagement.CustomAdapters
{
    public class OutcomeViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.tvDescription)]
        private TextView tvDescription;

        [InjectView(Resource.Id.imgGroup)] private ImageView imgGroup;

        [InjectView(Resource.Id.btnAdmin)] private ImageButton btnAdmin;

        [InjectView(Resource.Id.tvAmount)] private TextView tvAmount;

        [InjectOnClick(Resource.Id.btnAdmin)]
        private void ShowMenu(object sender, EventArgs e)
        {
            //show admin's menu
        }

        public OutcomeModel OutcomeModel
        {
            set
            {
                //set imgGroup
                tvDescription.Text = value.Title;
                tvAmount.Text = $"-{value.Amount.ToCurrency()}";
            }
        }
        public OutcomeViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }
    }
}
using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;
using Com.Bumptech.Glide;
using Refractored.Controls;

namespace ClubManagement.CustomAdapters
{
    public class MoneyAdminViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.civAvatar)] private CircleImageView civAvatar;

        [InjectView(Resource.Id.tvUserName)] private TextView tvUserName;

        [InjectView(Resource.Id.btnPay)] private Button btnPay;

        [InjectView(Resource.Id.tvPaidTime)] private TextView tvPaidTime;

        [InjectOnClick(Resource.Id.btnPay)]
        private void Pay(object s, EventArgs e)
        {

        }

        public string MoneyId { get; set; }

        private MoneyAdminState moneyAdminState;

        public MoneyAdminState MoneyAdminState
        {
            get => moneyAdminState;
            set
            {
                if (!string.IsNullOrEmpty(value.User.Avatar))
                {
                    Glide.With(ItemView.Context).Load(value.User.Avatar).Into(civAvatar);
                }

                tvUserName.Text = value.User.Name;
                if (value.IsPaid)
                {
                    tvPaidTime.Visibility = ViewStates.Visible;
                    tvPaidTime.Text = "";
                    btnPay.SetTextColor(ItemView.Resources.GetColor(Resource.Color.text_color_black, null));
                    btnPay.SetBackgroundResource(Resource.Drawable.button_repay_background);
                    btnPay.Text = "Repay";
                }
                else
                {
                    tvPaidTime.Visibility = ViewStates.Gone;
                    btnPay.SetTextColor(ItemView.Resources.GetColor(Resource.Color.text_color_white, null));
                    btnPay.SetBackgroundResource(Resource.Drawable.button_pay_background);
                    btnPay.Text = "Pay";
                }
            }
        }

        public MoneyAdminViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }
    }
}
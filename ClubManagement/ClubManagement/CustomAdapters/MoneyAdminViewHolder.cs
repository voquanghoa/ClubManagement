using System;
using System.Linq;
using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using Com.Bumptech.Glide;
using Refractored.Controls;

namespace ClubManagement.CustomAdapters
{
    public class MoneyAdminViewHolder : RecyclerView.ViewHolder
    {
        public string MoneyId { get; set; }

        [InjectView(Resource.Id.civAvatar)] private CircleImageView civAvatar;

        [InjectView(Resource.Id.tvUserName)] private TextView tvUserName;

        [InjectView(Resource.Id.btnPay)] private Button btnPay;

        [InjectView(Resource.Id.tvPaidTime)] private TextView tvPaidTime;

        [InjectOnClick(Resource.Id.btnPay)]
        private void Pay(object s, EventArgs e)
        {
            var userMoney = new UserMoneyModel
            {
                MoneyId = MoneyId,
                UserId = moneyAdminState.User.Id,
                PaidTime = DateTime.Now
            };
            if (!moneyAdminState.IsPaid)
            {
                var dialog = ItemView.Context.CreateDialog(Resource.String.paying_fee, Resource.String.wait);
                dialog.Show();
                ((Activity) ItemView.Context).DoRequest(
                    async () =>
                    {
                        await UserMoneysController.Instance.Add(userMoney);
                        dialog.Dismiss();
                        moneyAdminState.PaidTime = DateTime.Now;
                        moneyAdminState.IsPaid = true;
                        PayClick?.Invoke(moneyAdminState, e);
                    });
            }
            else
            {
                ItemView.Context.ShowConfirmDialog(ItemView.Resources.GetString(Resource.String.confirm_repay),
                    $"Do you want to repay for {moneyAdminState.User.Name}?",
                    () =>
                    {
                        var dialog = ItemView.Context.CreateDialog(Resource.String.repaying_fee, Resource.String.wait);
                        dialog.Show();
                        ((Activity) ItemView.Context).DoRequest(async () =>
                        {
                            await UserMoneysController.Instance.Delete(UserMoneysController.Instance.Values.First(x =>
                                x.UserId == moneyAdminState.User.Id && x.MoneyId == MoneyId));
                            dialog.Dismiss();
                            moneyAdminState.IsPaid = false;
                            PayClick?.Invoke(moneyAdminState, e);
                        });
                    }, () => { }).Show();
            }
        }

        public event EventHandler PayClick;

        private MoneyAdminState moneyAdminState;

        public MoneyAdminState MoneyAdminState
        {
            get => moneyAdminState;
            set
            {
                moneyAdminState = value;
                if (!string.IsNullOrEmpty(value.User.Avatar))
                {
                    Glide.With(ItemView.Context).Load(value.User.Avatar).Into(civAvatar);
                }

                tvUserName.Text = value.User.Name;
                if (value.IsPaid)
                {
                    tvPaidTime.Visibility = ViewStates.Visible;
                    tvPaidTime.Text = $"Paid on {value.PaidTime:MMM dd, yyyy}";
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
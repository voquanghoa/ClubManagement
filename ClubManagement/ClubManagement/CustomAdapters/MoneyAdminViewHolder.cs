using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content.Res;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;

namespace ClubManagement.CustomAdapters
{
    public class MoneyAdminViewHolder : RecyclerView.ViewHolder
    {
        private readonly bool isAdmin = true; // Fake admin 

        [InjectView(Resource.Id.tvUser)] private TextView tvUser;

        [InjectView(Resource.Id.imgState)] private ImageView imgState;
        
        public TextView TvState { get; set; }

        public string MoneyId { get; set; }

        private MoneyAdminState moneyAdminState;

        public MoneyAdminState MoneyAdminState
        {
            get => moneyAdminState;
            set
            {
                tvUser.Text = value.User.Name;
                imgState.SetImageResource(value.IsPaid ? Resource.Drawable.icon_paid : Resource.Drawable.icon_unpaid);
                moneyAdminState = value;
            }
        }

        [InjectOnClick(Resource.Id.imgState)]
        private void Pay(object sender, EventArgs e)
        {
            if (!isAdmin) return;
            var moneyUserController = UserMoneysController.Instance;
            var builder = new AlertDialog.Builder(ItemView.Context)
                .SetCancelable(false)
                .SetNegativeButton("No", (s, dce) => { });
            if (!MoneyAdminState.IsPaid)
            {
                builder
                    .SetTitle("Do you want to pay this budget?")
                    .SetPositiveButton("Yes", (s, dce) =>
                    {
                        try
                        {
                            moneyUserController.Add(new UserMoneyModel
                            {
                                UserId = MoneyAdminState.User.Id,
                                MoneyId = MoneyId
                            });
                            MoneyAdminState.IsPaid = !MoneyAdminState.IsPaid;
                            TvState.Text = "You have just paid this budget!";
                            imgState.SetImageResource(Resource.Drawable.icon_paid);
                            Toast.MakeText(ItemView.Context, "Pay successfully!", ToastLength.Short).Show();
                        }
                        catch (Exception)
                        {
                            Toast.MakeText(ItemView.Context, ItemView.Context.Resources.GetString(Resource.String.no_internet_connection), ToastLength.Short).Show();
                        }
                    })
                    .Show();
            }
            else
            {
                builder.SetTitle("Do you want to unpay this budget?")
                    .SetPositiveButton("Yes", (s, dce) =>
                    {
                        try
                        {
                            var moneyUserList = moneyUserController.Values ?? new List<UserMoneyModel>();
                            var moneyUser = moneyUserList.First(x =>
                                x.MoneyId == MoneyId && x.UserId == MoneyAdminState.User.Id);
                            if (moneyUser == null) return;
                            moneyUserController.Delete(moneyUser);
                            MoneyAdminState.IsPaid = !MoneyAdminState.IsPaid;
                            Toast.MakeText(ItemView.Context, "Unpay successfully!", ToastLength.Short).Show();
                            TvState.Text = "You have just unpaid this budget!";
                            imgState.SetImageResource(Resource.Drawable.icon_unpaid);
                        }
                        catch (Exception)
                        {
                            Toast.MakeText(ItemView.Context,
                                ItemView.Context.Resources.GetString(Resource.String.no_internet_connection),
                                ToastLength.Short).Show();
                        }
                    })
                    .Show();
            }
        }

        public MoneyAdminViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }
    }
}
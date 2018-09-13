using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using ClubManagement.Activities;
using ClubManagement.Interfaces;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using Newtonsoft.Json;
using PopupMenu = Android.Widget.PopupMenu;

namespace ClubManagement.CustomAdapters
{
    public class MoneyViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        [InjectView(Resource.Id.tvDescription)]
        private TextView tvDescription;

        [InjectView(Resource.Id.imgGroup)] private ImageView imgGroup;

        [InjectView(Resource.Id.tvIsPaid)] private TextView tvIsPaid;

        [InjectView(Resource.Id.tvAmount)] private TextView tvAmount;

        [InjectView(Resource.Id.btnAdmin)] private ImageButton btnAdmin;

        public IItemClickListener ItemClickListener { get; set; }

        public MoneyState MoneyState
        {
            set
            {
                tvDescription.Text = value.MoneyModel.Description;
                if (value.IsPaid)
                {
                    tvIsPaid.SetTextColor(ContextCompat.GetColorStateList(ItemView.Context, Resource.Color.state_going_color));
                    tvIsPaid.Text = "PAID";
                }
                else
                {
                    tvIsPaid.SetTextColor(ContextCompat.GetColorStateList(ItemView.Context, Resource.Color.color_red));
                    tvIsPaid.Text = "UNPAID";
                }

                tvAmount.Text = $"{value.MoneyModel.Amount.ToCurrency()}";

                ItemView.Context.DoWithAdmin(() =>
                {
                    btnAdmin.Visibility = ViewStates.Visible;
                    btnAdmin.Click += (s, e) =>
                    {
                        var popupMenu = ((View) s).CreatepopupMenu(Resource.Menu.admin_menu);
                        popupMenu.MenuItemClick += PopupMenu_MenuItemClick;
                        var menu = popupMenu.Menu;
                        var item = menu.GetItem(1);
                        var spanString = new SpannableString(item.TitleFormatted.ToString());
                        spanString.SetSpan(new ForegroundColorSpan(ItemView.Resources.GetColor(Resource.Color.color_red, null)), 0, spanString.Length(), 0); //fix the color to white
                        item.SetTitle(spanString);
                        popupMenu.Show();
                    };
                });
            }
        }

        private void PopupMenu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.edit:
                    // Edit fee
                    break;
                case Resource.Id.delete:
                    ((Activity) ItemView.Context).RunOnUiThread(() =>
                    {
                        ItemView.Context.ShowConfirmDialog(Resource.String.delete_fee, Resource.String.confirm_delete,
                            () =>
                            {
                            },
                            () => { }).Show();
                    });
                    break;
            }
            ((PopupMenu) sender).Dismiss();
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
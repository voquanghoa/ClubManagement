using System;
using System.Linq;
using Android.App;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Interfaces;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using PopupMenu = Android.Widget.PopupMenu;
using Android.Content;
using ClubManagement.Activities;
using Newtonsoft.Json;

namespace ClubManagement.CustomAdapters
{
    public class OutcomeViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        [InjectView(Resource.Id.tvDescription)]
        private TextView tvDescription;

        [InjectView(Resource.Id.imgGroup)] private ImageView imgGroup;

        [InjectView(Resource.Id.btnAdmin)] private ImageButton btnAdmin;

        [InjectView(Resource.Id.tvAmount)] private TextView tvAmount;

        private PopupMenu popupMenu;

        public IItemClickListener ItemClick { get; set; }

        public event EventHandler DeleteClick;

        [InjectOnClick(Resource.Id.btnAdmin)]
        private void ShowMenu(object sender, EventArgs e)
        {
            popupMenu = ((View) sender).CreatepopupMenu(Resource.Menu.admin_menu);
            popupMenu.MenuItemClick += PopupMenu_MenuItemClick;
            var menu = popupMenu.Menu;
            var item = menu.GetItem(1);
            var spanString = new SpannableString(item.TitleFormatted.ToString());
            spanString.SetSpan(new ForegroundColorSpan(ItemView.Resources.GetColor(Resource.Color.color_red, null)), 0, spanString.Length(), 0); 
            item.SetTitle(spanString);
            popupMenu.Show();
        }

        private void PopupMenu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            popupMenu.Dismiss();
            switch (e.Item.ItemId)
            {
                case Resource.Id.edit:
                    var intent = new Intent(ItemView.Context, typeof(EditOutcomeActivity));
                    var content = JsonConvert.SerializeObject(outcomeModel);
                    intent.PutExtra("OutcomeDetail", content);

                    ItemView.Context.StartActivity(intent);
                    break;
                case Resource.Id.delete:
                    ((Activity)ItemView.Context).RunOnUiThread(() =>
                    {
                        ItemView.Context.ShowConfirmDialog(Resource.String.confirm , Resource.String.delete_outcome,
                            () =>
                            {
                                var dialog = ItemView.Context.CreateDialog(Resource.String.deleting_outcome, Resource.String.wait);
                                dialog.Show();
                                ((Activity)ItemView.Context).DoRequest(OutComesController.Instance.Delete(OutcomeModel),
                                    () => 
                                    {
                                        dialog.Dismiss();
                                        DeleteClick?.Invoke(OutcomeModel, e);
                                        ItemView.Context.ShowMessage(Resource.String.delete_outcome_success);
                                    });
                            },
                            () => { }).Show();
                    });
                    break;
            }
        }

        private OutcomeModel outcomeModel;

        public OutcomeModel OutcomeModel
        {
            get => outcomeModel;
            set
            {
                if (value.Group != null) imgGroup.SetImageResource(AppConstantValues.FeeGrooups.Find(x => x.Id.Equals(value.Group)).ImageId);
                outcomeModel = value;
                btnAdmin.Visibility = ViewStates.Gone;
                tvDescription.Text = value.Title;
                tvAmount.Text = $"-{value.Amount.ToCurrency()}";
                ItemView.Context.DoWithAdmin(()=> btnAdmin.Visibility = ViewStates.Visible);
            }
        }
        public OutcomeViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
            itemView.SetOnClickListener(this);
        }

        public void OnClick(View v)
        {
            ItemClick.OnClick(v, AdapterPosition);
        }
    }
}
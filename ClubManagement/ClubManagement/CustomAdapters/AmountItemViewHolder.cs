using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;

namespace ClubManagement.CustomAdapters
{
    public class AmountItemViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.tvItemName)] private TextView tvItemName;

        [InjectView(Resource.Id.tvAmount)] private TextView tvAmount;

        [InjectView(Resource.Id.btnEdit)] private ImageButton btnEdit;

        [InjectOnClick(Resource.Id.btnEdit)]
        private void Edit(object s, EventArgs e)
        {
            if (isDeleting)
            {
                item.IsChooseToDelete = !item.IsChooseToDelete;
                btnEdit.SetImageResource(item.IsChooseToDelete
                    ? Resource.Drawable.icon_delete_amount_check
                    : Resource.Drawable.icon_delete_amount_uncheck);
                Click?.Invoke(item.IsChooseToDelete, e);
            }
        }

        private bool isDeleting = false;

        public bool IsDeleting
        {
            set
            {
                isDeleting = value;
                btnEdit.SetImageResource(value
                    ? Resource.Drawable.icon_delete_amount_uncheck
                    : Resource.Drawable.icon_edit_amount);
            }
        }

        private AmountItem item = new AmountItem();

        public AmountItem Item
        {
            set
            {
                item = value;
                tvItemName.Text = value.Item.Name;
                tvAmount.Text = value.Item.Amount.ToString();
            }
        }

        public event EventHandler Click;

        public AmountItemViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
        }
    }
}
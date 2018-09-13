using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Interfaces;
using ClubManagement.Models;
using ClubManagement.Ultilities;

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
                        // Show admin menu
                    };
                });
            }
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
using System;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Widget;
using ClubManagement.Models;
using Android.Preferences;
using Android.App;
using ClubManagement.Ultilities;
using Android.Graphics;

namespace ClubManagement.Adapters
{
    public class PersonsGoTimeAdapterViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.textViewName)]
        private TextView tvName;

        [InjectView(Resource.Id.textViewGoTime)]
        private TextView tvGoTime;

        public event EventHandler<ClickEventArgs> ClickHander;

        private Color selectedColor = Color.Rgb(110, 219, 81);

        private Color unSelectColor = Color.Rgb(195, 207, 219);

        public PersonGoTimeModel PersonGoTimeModel
        {
            set
            {
                tvName.Text = value.Name;
                tvGoTime.Text = value.GoTime;

                ItemView.SetBackgroundColor(value.Selected ? selectedColor : unSelectColor);
            }
        }

        public PersonsGoTimeAdapterViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);

            itemView.Click += (s, e) =>
            {
                ClickHander?.Invoke(s, new ClickEventArgs() { Position = AdapterPosition });
            };
        }
    }
}
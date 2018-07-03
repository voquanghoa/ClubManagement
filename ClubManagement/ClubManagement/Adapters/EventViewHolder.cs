using System;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Widget;
using ClubManagement.Models;
using ClubManagement.Controllers;
using System.Linq;
using Android.Preferences;
using Android.App;
using Android.Graphics;

namespace ClubManagement.Adapters
{
    public class EventViewHolder : RecyclerView.ViewHolder
    {
        [InjectView(Resource.Id.tvTitle)]
        private TextView tvTitle;

        [InjectView(Resource.Id.tvDescription)]
        private TextView tvDescription;

        [InjectView(Resource.Id.tvTime)]
        private TextView tvTime;

        [InjectView(Resource.Id.tvPlace)]
        private TextView tvPlace;

        [InjectView(Resource.Id.btnJoin)]
        private Button btnJoin;

        public event EventHandler ClickHander;

        public EventModel EventModel
        {
            set
            {
                tvTitle.Text = value.Title;
                tvDescription.Text = value.Description;
                tvTime.Text = value.Time.ToShortDateString();
                tvPlace.Text = MapsController.Instance.GetAddress(value.Latitude, value.Longitude);
                var preferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                var userId = preferences.GetString("UserId", string.Empty);

                if (UserEventsController.Instance.Values.Where(x => x.EventId == value.Id).Any(x => x.UserId == userId)) 
                {
                    btnJoin.Text = "Joined";
                    btnJoin.SetTextColor(Color.Green);
                    btnJoin.SetBackgroundColor(Color.Gray);
                }
                else
                {
                    btnJoin.Text = "Join";
                    btnJoin.SetTextColor(Color.White);
                    btnJoin.SetBackgroundColor(Color.Green);
                }
            }
        }

        public EventViewHolder(View itemView) :base(itemView)
        {
            Cheeseknife.Inject(this, itemView);

            itemView.Click += (s, e) =>
            {
                ClickHander?.Invoke(s, e);
            };
        }
    }
}
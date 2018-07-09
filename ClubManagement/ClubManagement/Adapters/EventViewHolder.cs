using System;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Widget;
using ClubManagement.Models;
using Android.Preferences;
using Android.App;
using ClubManagement.Ultilities;

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

        public event EventHandler<ClickEventArgs> ClickHander;

        public UserLoginEventModel EventModel
        {
            set
            {
                tvTitle.Text = value.Title;
                tvDescription.Text = value.Description;
                tvTime.Text = value.Time.ToShortDateString();
                tvPlace.Text = value.Place;
                var preferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                var userId = preferences.GetString("UserId", string.Empty);

                btnJoin.ChangeStatusButtonJoin(value.IsJoined);

                btnJoin.Clickable = false;
            }
        }

        public EventViewHolder(View itemView) :base(itemView)
        {
            Cheeseknife.Inject(this, itemView);

            itemView.Click += (s, e) =>
            {
                ClickHander?.Invoke(s, new ClickEventArgs() { Position = AdapterPosition });
            };
        }
    }
}
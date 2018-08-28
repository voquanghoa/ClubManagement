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

        public string Id;

        public UserLoginEventModel EventModel
        {
            set
            {
                Id = value.Id;
                tvTitle.Text = value.Title;
                tvDescription.Text = value.Description;
                tvTime.Text = value.Time.ToShortDateString();
                tvPlace.Text = value.Place;
                var preferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                preferences.GetString("UserId", string.Empty);

                

                btnJoin.Clickable = false;

                btnJoin.Visibility = value.Time < DateTime.Now && !value.IsJoined
                    ? ViewStates.Gone
                    : ViewStates.Visible;
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
using System;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;
using Com.Bumptech.Glide;

namespace ClubManagement.Adapters
{
    public class ItemEventViewHolder : RecyclerView.ViewHolder
    {
        public event EventHandler<ClickEventArgs> ClickHander;

        [InjectView(Resource.Id.tvEventTitle)] private TextView tvEventTitle;

        [InjectView(Resource.Id.tvTime)] private TextView tvTime;

        [InjectView(Resource.Id.imgEvent)] private ImageView imgEvent;

        [InjectView(Resource.Id.tvPlace)] private TextView tvPlace;

        [InjectView(Resource.Id.imgJoinedUsers)]
        private ImageView imgJoinedUsers;

        [InjectView(Resource.Id.tvGoingStatus)]
        private TextView tvGoingStatus;

        public string Id;

        public UserLoginEventModel EventModel
        {
            set
            {
                Id = value.Id;
                tvEventTitle.Text = value.Title;
                tvTime.Text = value.Time.DayOfWeek + " " + value.Time.ToShortTimeString();
                tvPlace.Text = value.Place;
                Glide.With(ItemView.Context).Load(value.ImageUrl).Into(imgEvent);
                if (value.IsJoined)
                {
                    imgJoinedUsers.SetImageResource(Resource.Drawable.icon_paid);
                    tvGoingStatus.Text = "You and " + (value.NumberOfJoinedUsers > 1
                                             ? (value.NumberOfJoinedUsers - 1)
                                             : value.NumberOfJoinedUsers) + " going";
                    tvGoingStatus.SetTextColor(ContextCompat.GetColorStateList(ItemView.Context,
                        Resource.Color.state_going_color));
                }
                else
                {
                    imgJoinedUsers.SetImageResource(Resource.Drawable.icon_going);
                    tvGoingStatus.Text = $"{value.NumberOfJoinedUsers} going";
                    tvGoingStatus.SetTextColor(ContextCompat.GetColorStateList(ItemView.Context, Resource.Color.state_not_going_color));
                }
            }
        }

        public ItemEventViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);
            itemView.Click += (s, e) =>
            {
                ClickHander?.Invoke(s, new ClickEventArgs() { Position = AdapterPosition });
            };
        }
    }
}
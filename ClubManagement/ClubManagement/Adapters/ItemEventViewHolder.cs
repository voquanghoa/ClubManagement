using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.Content.Res;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;

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

        public UserLoginEventModel EventModel
        {
            set
            {
                tvEventTitle.Text = value.Title;
                tvTime.Text = value.Time.ToShortTimeString();
                tvPlace.Text = value.Place;
                imgJoinedUsers.SetImageResource(value.IsJoined
                    ? Resource.Drawable.icon_paid
                    : Resource.Drawable.icon_going);
                tvGoingStatus.Text = value.IsJoined ? "Going" : $"You and {value.NumberOfJoinedUsers - 1} going";
                tvGoingStatus.SetTextColor(ContextCompat.GetColorStateList(ItemView.Context, Resource.Color.state_going_color));
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
using System;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;
using Android.Support.V7.Widget;
using ClubManagement.Ultilities;
using ClubManagement.Controllers;
using Square.Picasso;
using Android.Text;
using Android.Text.Style;
using Android.Graphics;
using Android.App;
using System.Threading.Tasks;

namespace ClubManagement.Adapters
{
    public class NotificationsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public event EventHandler ClickHander;

        [InjectView(Resource.Id.tvNotificationTitle)]
        private TextView tvNotificationTitle;

        [InjectView(Resource.Id.tvNotificationTime)]
        private TextView tvNotificationTime;

        [InjectView(Resource.Id.imgNotification)]
        private ImageView imgNotification;

        [InjectView(Resource.Id.imgNotificationTime)]
        private ImageView imgNotificationTime;

        private NotificationModel notificationModel;

        public NotificationModel NotificationModel
        {
            set
            {
                notificationModel = value;

                imgNotificationTime.Visibility = ViewStates.Visible;
                imgNotification.SetImageResource(Resource.Drawable.icon_notification);

                var time = (DateTime.Now - value.LastUpdate);
                var timeString = "";
                
                if (time.TotalMinutes < 1)
                {
                    timeString = "Just now";
                }
                else if (time.TotalHours < 1)
                {
                    if (time.Minutes == 1) timeString = $"{time.Minutes} minute ago";
                    else timeString = $"{time.Minutes} minutes ago";
                }
                else if (time.TotalDays <= 1)
                {
                    if (time.Hours == 1) timeString = $"{time.Hours} hour ago";
                    timeString = $"{time.Hours} hours ago";
                }
                else
                {
                    timeString = $"{value.LastUpdate.ToDateString()} at {value.LastUpdate.ToTimeString()}";
                }

                tvNotificationTime.Text = timeString;

                var startBold = AppConstantValues.NotificationStartBold[value.Type];
                var endBold = AppConstantValues.NotificationEndBold.GetValueOrDefault(value.Type, "");

                var spannableString = new SpannableString(value.Message);
                spannableString.SetSpan(new StyleSpan(TypefaceStyle.Bold)
                    , value.Message.ToLower().IndexOf(startBold) + startBold.Length
                    , string.IsNullOrEmpty(endBold)
                        ? value.Message.Length
                        : value.Message.LastIndexOf(endBold)
                    , 0);
                tvNotificationTitle.SetText(spannableString, TextView.BufferType.Normal);

                if (ItemView.Context is Activity activity)
                {
                    var backgroundId = 0;

                    activity.DoRequest(Task.Run(() =>
                    {
                        backgroundId = !value.UserIdsSeen.Contains(AppDataController.Instance.UserId)
                            ? Resource.Color.notification_background_new
                            : Resource.Color.notification_background_seen;
                    }), () =>
                    {
                        ItemView.SetBackgroundResource(backgroundId);
                    });

                    switch (value.Type)
                    {
                        case AppConstantValues.NotificationEditEvent:
                            imgNotificationTime.SetImageResource(Resource.Drawable.icon_calendar);

                            var imageUrl = "";

                            activity.DoRequest(Task.Run(() =>
                            {
                                imageUrl = EventsController.Instance.ValuesJustUpdate
                                    .Find(x => x.Id == value.TypeId).ImageUrl;
                            }), () =>
                            {
                                if (!string.IsNullOrEmpty(imageUrl))
                                {
                                    Picasso.With(ItemView.Context).Load(imageUrl).Fit().Into(imgNotification);
                                }
                            });

                            break;
                        case AppConstantValues.NotificationEditFee:
                            imgNotificationTime.SetImageResource(Resource.Drawable.icon_credit_card_blue);

                            var imageId = 0;

                            activity.DoRequest(Task.Run(() =>
                            {
                                var group = MoneysController.Instance.ValuesJustUpdate
                                    .Find(x => x.Id == value.TypeId).Group;

                                imageId = AppConstantValues.FeeGrooups
                                .Find(x => x.Id.Equals(group)).ImageId;
                            }), () =>
                            {
                                imgNotification.SetImageResource(imageId);
                            });

                            break;
                        case AppConstantValues.NotificationDeleteEvent:
                            imgNotificationTime.Visibility = ViewStates.Gone;

                            break;
                        case AppConstantValues.NotificationDeleteFee:
                            imgNotificationTime.Visibility = ViewStates.Gone;

                            break;
                    }
                }
            }
        }

        public NotificationsAdapterViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);

            itemView.Click += (s, e) =>
            {
                itemView.SetBackgroundResource(Resource.Color.notification_background_seen);
                ClickHander?.Invoke(notificationModel, e);
            };
        }
    }
}
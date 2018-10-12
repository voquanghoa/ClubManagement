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

                ItemView.SetBackgroundResource(value.IsNew
                    ? Resource.Color.notification_background_new
                    : Resource.Color.notification_background_seen);
                imgNotification.SetImageResource(Resource.Drawable.icon_notification);

                tvNotificationTime.Text = value.LastUpdate.ToString();

                var type = AppConstantValues.NotificationTypes[value.Type];
                var spannableString = new SpannableString(value.Message);
                spannableString.SetSpan(new StyleSpan(TypefaceStyle.Bold)
                    , value.Message.IndexOf(type) + type.Length
                    , value.Message.Length, 0);
                tvNotificationTitle.SetText(spannableString, TextView.BufferType.Normal);

                if (ItemView.Context is Activity activity)
                {
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
                    }
                }
            }
        }

        public NotificationsAdapterViewHolder(View itemView) : base(itemView)
        {
            Cheeseknife.Inject(this, itemView);

            itemView.Click += (s, e) =>
            {
                ClickHander?.Invoke(notificationModel, e);
            };
        }
    }
}
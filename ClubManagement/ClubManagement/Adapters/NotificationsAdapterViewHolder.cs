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
using System.Linq;
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

                var spannableString = new SpannableString(value.Message);
                spannableString.SetSpan(new StyleSpan(TypefaceStyle.Bold), value.Message.IndexOf("event") + "event".Length, value.Message.Length, 0);

                tvNotificationTitle.SetText(spannableString, TextView.BufferType.Normal);
                tvNotificationTime.Text = value.LastUpdate.ToString();

                switch (value.Type)
                {
                    case AppConstantValues.NotificationEditEvent:
                        imgNotificationTime.SetImageResource(Resource.Drawable.icon_calendar);

                        if (ItemView.Context is Activity activity)
                        {
                            var imageUrl = "";

                            activity.DoRequest(Task.Run(() =>
                            {
                                imageUrl = EventsController.Instance.Values
                                    .Find(x => x.Id == value.TypeId).ImageUrl;
                            }), () =>
                            {
                                if (!string.IsNullOrEmpty(imageUrl))
                                {
                                    Picasso.With(ItemView.Context).Load(imageUrl).Fit().Into(imgNotification);
                                }
                            });
                        }

                        break;
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
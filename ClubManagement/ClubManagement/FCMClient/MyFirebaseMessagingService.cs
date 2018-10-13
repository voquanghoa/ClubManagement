using Android.Content;
using Firebase.Messaging;
using Android.Media;
using ClubManagement.Activities;
using Android.App;
using Android.Support.V4.App;

namespace ClubManagement
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    [IntentFilter(new[] { global::Android.Content.Intent.ActionBootCompleted })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            Android.Util.Log.Debug(TAG, "From: " + message.From);
            Android.Util.Log.Debug(TAG, "Notification Message Body: " + message.GetNotification());
            SendNotification(message.GetNotification().Title, message.GetNotification().Body);
        }

        private void SendNotification(string title, string body)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("Id", Resource.Id.notificationTab);

            var stackBuilder = Android.App.TaskStackBuilder.Create(this);
            stackBuilder.AddNextIntentWithParentStack(intent);
            var pendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.icon_tab_money)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetSound(defaultSoundUri)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}
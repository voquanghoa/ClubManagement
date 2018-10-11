using Android.App;
using Android.Content;
using Firebase.Iid;
using Android.Util;
using Android.Preferences;
using ClubManagement.Ultilities;

namespace ClubManagement
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            base.OnTokenRefresh();
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
        }
    }
}
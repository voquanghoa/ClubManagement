using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Util;

namespace ClubManagement.Ultilities
{
    public static class AppUltilities
    {
        public static void HideKeyboard(this Activity activity)
        {
            if (activity.CurrentFocus == null) return;
            var inputMethodManager = (InputMethodManager) activity.GetSystemService(Context.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(activity.CurrentFocus.WindowToken, 0);
        }

        public static void ChangeStatusButtonJoin(this Button btnJoin, bool isJoined)
        {
            if (isJoined)
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

        public static void SetTextFont(this TextView textView, TypefaceStyle style)
        {
            var assetManager = Application.Context.ApplicationContext.Assets;
            var typeface = Typeface.CreateFromAsset(assetManager, Java.Lang.String.Format(Locale.Us, "fonts/%s", "IckyTicketMono.ttf"));
            textView.SetTypeface(typeface, style);
        }
    }
}
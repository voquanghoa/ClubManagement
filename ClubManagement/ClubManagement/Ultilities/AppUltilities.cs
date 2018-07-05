using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views.InputMethods;
using Android.Widget;

namespace ClubManagement.Ultilities
{
    public static class AppUltilities
    {
        public static void HideKeyboard(this Activity activity)
        {
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
    }
}
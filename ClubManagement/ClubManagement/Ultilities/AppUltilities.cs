using Android.App;
using Android.Content;
using Android.Views.InputMethods;

namespace ClubManagement.Ultilities
{
    public static class AppUltilities
    {
        public static void HideKeyboard(Activity activity)
        {
            if (activity.CurrentFocus == null) return;
            var inputMethodManager = (InputMethodManager) activity.GetSystemService(Context.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(activity.CurrentFocus.WindowToken, 0);
        }
    }
}
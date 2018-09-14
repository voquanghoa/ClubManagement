using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
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

        public static void ChangeTextViewStatus(this TextView textViewStatus, bool isJoined)
        {
            if (!isJoined)
            {
                textViewStatus.Text = Application.Context.Resources
                    .GetString(Resource.String.going);
                textViewStatus.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.icon_like,
                    0,
                    0,
                    0);
            }
            else
            {
                textViewStatus.Text = Application.Context.Resources
                    .GetString(Resource.String.you_are_going);
                textViewStatus.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.icon_paid,
                    0,
                    0,
                    0);
            }
        }

        public static void SetTextFont(this TextView textView, TypefaceStyle style)
        {
            var assetManager = Application.Context.ApplicationContext.Assets;
            var typeface = Typeface.CreateFromAsset(assetManager, Java.Lang.String.Format(Locale.Us, "fonts/%s", "IckyTicketMono.ttf"));
            textView.SetTypeface(typeface, style);
        }

        public static PopupMenu CreatepopupMenu(this View view, int menuRes)
        {
            var popupMenu = new PopupMenu(view.Context, view);

            var field = popupMenu.Class.GetDeclaredField("mPopup");
            field.Accessible = true;
            var menuPopupHelper = field.Get(popupMenu);
            var setForceIcons = menuPopupHelper.Class.GetDeclaredMethod("setForceShowIcon", Java.Lang.Boolean.Type);
            setForceIcons.Invoke(menuPopupHelper, true);

            popupMenu.Inflate(menuRes);

            return popupMenu;
        }
    }
}
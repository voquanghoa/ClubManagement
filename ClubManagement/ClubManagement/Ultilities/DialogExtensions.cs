using Android.App;
using Android.Content;

namespace ClubManagement.Ultilities
{
    public static class DialogExtensions
    {
        public static AlertDialog CreateDialog(string title, string message, Context context)
        {
            return new AlertDialog.Builder(context)
                .SetCancelable(false)
                .SetTitle(title)
                .SetMessage(message)
                .Create();
        }
    }
}
using Android.App;
using Android.Content;
#pragma warning disable 618

namespace ClubManagement.Ultilities
{
    public static class DialogExtensions
    {
        public static ProgressDialog CreateDialog(string title, string message, Context context)
        {
            var progressDialog = new ProgressDialog(context);
            progressDialog.SetTitle(title);
            progressDialog.SetMessage(message);
            progressDialog.SetCancelable(false);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            return progressDialog;
        }
    }
}
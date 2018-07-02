using Android.App;
using Android.Content;

namespace ClubManagement.Ultilities
{
    public static class DialogExtensions
    {
#pragma warning disable 618
        public static ProgressDialog CreateDialog(string title, string message, Context context)
#pragma warning restore 618
        {
#pragma warning disable 618
            var progressDialog = new ProgressDialog(context);
#pragma warning restore 618
            progressDialog.SetTitle(title);
            progressDialog.SetMessage(message);
            progressDialog.SetCancelable(false);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            return progressDialog;
        }
    }
}
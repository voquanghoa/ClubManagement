using Android.App;
using Android.Content;
using Android.Preferences;
using ClubManagement.Activities;

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

        public static void ShowLogoutDialog(Context context)
        {
            var preferencesEditor = PreferenceManager.GetDefaultSharedPreferences(Application.Context).Edit();
            new AlertDialog.Builder(context)
                .SetCancelable(false)
                .SetTitle("Are you sure to logout?")
                .SetPositiveButton("Yes", (dce, e) =>
                {
                    preferencesEditor.PutString("UserId", string.Empty);
                    preferencesEditor.PutBoolean("IsLogged", false);
                    preferencesEditor.Commit();
                    context.StartActivity(typeof(LoginActivity));
                })
                .SetNegativeButton("No", (dce, e) => { }).Show();
        }
    }
}
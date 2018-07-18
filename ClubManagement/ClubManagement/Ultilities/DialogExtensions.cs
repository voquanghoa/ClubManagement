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
                .SetTitle(context.Resources.GetString(Resource.String.confirm_logout))
                .SetPositiveButton(context.Resources.GetString(Resource.String.dialog_positive_button), (dce, e) =>
                {
                    preferencesEditor.PutString(AppConstantValues.UserIdPreferenceKey, string.Empty);
                    preferencesEditor.PutBoolean(AppConstantValues.LogStatusPreferenceKey, false);
                    preferencesEditor.Commit();
                    ((Activity)context).Finish();
                    context.StartActivity(typeof(LoginActivity));
                })
                .SetNegativeButton(context.Resources.GetString(Resource.String.dialog_negative_button), (dce, e) => { }).Show();
        }
    }
}
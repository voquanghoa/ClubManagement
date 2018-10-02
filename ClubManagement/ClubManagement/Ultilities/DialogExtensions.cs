using Android.App;
using Android.Content;
using Android.Preferences;
using ClubManagement.Activities;
using System;

#pragma warning disable 618

namespace ClubManagement.Ultilities
{
    public static class DialogExtensions
    {
        public static ProgressDialog CreateDialog(this Context context, string title, string message)
        {
            var progressDialog = new ProgressDialog(context);
            progressDialog.SetTitle(title);
            progressDialog.SetMessage(message);
            progressDialog.SetCancelable(false);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            return progressDialog;
        }

        public static ProgressDialog CreateDialog(this Context context, int titleId, int messageId)
        {
			Func<int, string> getString = context.Resources.GetString;
            
			return context.CreateDialog(getString(titleId), getString(messageId));
        }
        
        public static void ShowLogoutDialog(this Context context)
        {
            var preferencesEditor = PreferenceManager.GetDefaultSharedPreferences(Application.Context).Edit();
			context.ShowConfirmDialog(Resource.String.confirm, Resource.String.confirm_logout, () =>
			{
				preferencesEditor.PutString(AppConstantValues.UserIdPreferenceKey, string.Empty);
                preferencesEditor.PutBoolean(AppConstantValues.LogStatusPreferenceKey, false);
                preferencesEditor.Commit();
                ((Activity)context).Finish();
                context.StartActivity(typeof(LoginActivity));
			}).Show();
        }

        
        public static AlertDialog ShowConfirmDialog(this Context context, string title,
            string message, Action actionAllow, Action actionDeny = null)
        {
            return new AlertDialog.Builder(context)
                .SetTitle(title)
                .SetMessage(message)
                .SetCancelable(false)
                .SetPositiveButton(Resource.String.dialog_positive_button,
                    (s, e) =>
                    {
				        actionAllow();
				        (s as Dialog)?.Dismiss();
                    })
                .SetNegativeButton(Resource.String.dialog_negative_button,
                    (s, e) =>
                    {
				        actionDeny?.Invoke();
				        (s as Dialog)?.Dismiss();
                    })
                .Create();
        }

		public static AlertDialog ShowConfirmDialog(this Context context, int title,
            int message, Action actionAllow, Action actionDeny = null)
        {
            Func<int, string> getString = context.Resources.GetString;

            return context.ShowConfirmDialog(getString(title), getString(message), actionAllow, actionDeny);
        }
    }
}
using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using Android.Support.V4.App;
using Activity = Android.App.Activity;
using System.Threading.Tasks;

namespace ClubManagement.Ultilities
{
    public static class ActivityExtensions
    {
		public static void ShowMessage(this Fragment fragment, String messageFormat, params String[] args)
        {
			fragment.Context.ShowMessage(messageFormat, args);
        }

		public static void ShowMessage(this Fragment fragment, String message)
        {
			fragment.Context.ShowMessage(message);
        }

		public static void ShowMessage(this Fragment fragment, int textId)
        {
			fragment.Context.ShowMessage(textId);
        }

		public static void ShowMessage(this Context context, String messageFormat, params String[] args)
        {
			context.ShowMessage(String.Format(messageFormat, args));
        }
		
		public static void ShowMessage(this Context context, String message)
		{
 			((Activity)context).RunOnUiThread(() => Toast.MakeText(context, message, ToastLength.Long).Show());	
		}

		public static void ShowMessage(this Context context, int textId)
        {
			context.ShowMessage(context.GetString(textId));
        }

        public static async void DoRequest(this Activity activity, Task action, Action postAction = null, Action exceptionAction = null)
        {
            try
            {
                await action;
                activity.RunOnUiThread(postAction);
            }
            catch (Exception ex)
            {
                activity.RunOnUiThread(exceptionAction);
                activity.ShowMessage(ex.Message);
            }
        }

        public static void DoWithAdmin(this Context context, Action action)
        {
            if (AppDataController.Instance.IsAdmin) action();
        }

        public static void DoWithAdmin(this Context context, Action action, Action notAdminAction)
        {
            if (AppDataController.Instance.IsAdmin) action();
            else notAdminAction();
        }

		public static void ShowIfAdmin(this View view)
		{
			if (AppDataController.Instance.IsAdmin) 
			{
				view.Visibility = ViewStates.Visible;	
			}
            else
			{
				view.Visibility = ViewStates.Gone;   
			}
		}
    }
}
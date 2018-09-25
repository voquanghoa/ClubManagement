using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using Android.Support.V4.App;
using Activity = Android.App.Activity;
             
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
		
        public static void DoRequest(this Activity activity, Action action, Action postAction = null, Action exceptionAction = null)
        {
            new Thread(() =>
            {
                try
                {
                    action.Invoke();
                    activity.RunOnUiThread(postAction);
                }
                catch (Exception ex)
                {
                    activity.RunOnUiThread(exceptionAction);

					activity.ShowMessage(ex.Message);
                }

            }).Start();
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
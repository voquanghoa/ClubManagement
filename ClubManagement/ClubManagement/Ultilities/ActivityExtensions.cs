﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ClubManagement.Ultilities
{
    public static class ActivityExtensions
    {
        public static void DoRequest(this Activity activity, Action action, Action postAction = null)
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
                    activity.RunOnUiThread(() => Toast.MakeText(activity,
                            ex.Message, ToastLength.Short)
                        .Show());
                }

            }).Start();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class FirebaseController
    {
        protected static FirebaseClient firebaseClient = new FirebaseClient("https://clubmanagement-98743.firebaseio.com/");
    }
}
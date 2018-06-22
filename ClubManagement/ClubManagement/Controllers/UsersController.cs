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
using ClubManagement.Models;
using Firebase.Xamarin.Database.Query;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class UsersController : FirebaseController
    {
        //private  = (new FirebaseClient("https://clubmanagement-98743.firebaseio.com/"))..Child("users");

        public static UsersController Instance = new UsersController();

        private UsersController()
        {
        }

        public void AddUser<T>(T t)
        {
            firebaseClient.Child(t.GetType().ToString()).PostAsync(t);
        }

        public void EditUser<T>(T t)
        {
            var items = firebaseClient.Child(t.GetType().ToString()).OnceAsync<T>();

            foreach (var item in items)
            {
                item.Object.Name = "c";

                users.Child(item.Key).PutAsync(new User() { Name = "c" });
            }
        }

        public void RemoveUser()
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Android.Util;
using Firebase.Xamarin.Database.Query;
using ClubManagement.Models;

namespace ClubManagement.Controllers
{
    public class FirebaseController<T> where T : FirebaseModel
    {
        protected const string LinkFirebase = "https://clubmanagement-98743.firebaseio.com/";

        protected ChildQuery FirebaseClient;

        protected FirebaseController()
        {

        }

        public async void Add(T t)
        {
            try
            {
                await FirebaseClient.PostAsync(t);
            }
            catch (Exception e)
            {
                Log.Error("no_internet_connection", e.Message);
            }
        }

        public List<T> Values
        {
            get
            {
                try
                {
                    var users = FirebaseClient.OnceAsync<T>().Result.Select(x =>
                    {
                        x.Object.Id = x.Key;

                        return x.Object;
                    }).ToList();
                    return users;
                }
                catch (Exception e)
                {
                    Log.Error("no_internet_connection", e.Message);
                    return new List<T>();
                }
            }
        }

        public async Task Edit(T t)
        {
            try
            {
                await FirebaseClient.Child(t.Id).PutAsync(t);
            }
            catch (Exception e)
            {
                Log.Error("no_internet_connection", e.Message);
            }
        }

        public async void Delete(T t)
        {
            try
            {
                await FirebaseClient.Child(t.Id).DeleteAsync();
            }
            catch (Exception e)
            {
                Log.Error("no_internet_connection", e.Message);
            }
        }
    }
}
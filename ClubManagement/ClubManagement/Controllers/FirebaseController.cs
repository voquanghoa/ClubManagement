using System.Collections.Generic;
using System.Linq;
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
            await FirebaseClient.PostAsync(t);
        }

        public List<T> Values
        {
            get
            {
                var users = FirebaseClient.OnceAsync<T>().Result;

                return users.Select(x =>
                {
                    x.Object.Id = x.Key;

                    return x.Object;
                }).ToList();
            }
        }

        public async void Edit(T t)
        {
            await FirebaseClient.Child(t.Id).PutAsync(t);
        }
    }
}
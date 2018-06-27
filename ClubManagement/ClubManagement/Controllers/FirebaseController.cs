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

        public void Add(T t)
        {
            FirebaseClient.PostAsync(t);
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

        public void Edit(T t)
        {
            FirebaseClient.Child(t.Id).PutAsync(t);
        }
    }
}
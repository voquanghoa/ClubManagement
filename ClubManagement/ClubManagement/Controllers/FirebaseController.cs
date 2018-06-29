using System.Collections.Generic;
using System.Linq;
using Firebase.Xamarin.Database.Query;
using ClubManagement.Models;

namespace ClubManagement.Controllers
{
    public class FirebaseController<T> where T : FirebaseModel
    {
        protected const string LINK_FIREBASE = "https://clubmanagement-98743.firebaseio.com/";

        protected ChildQuery firebaseClient;

        protected FirebaseController()
        {

        }

        public void Add(T t)
        {
            firebaseClient.PostAsync(t);
        }

        public List<T> Values
        {
            get
            {
                var users = firebaseClient.OnceAsync<T>().Result;

                return users.Select(x =>
                {
                    x.Object.Id = x.Key;

                    return x.Object;
                }).ToList();
            }
        }

        public void Edit(T t)
        {
            firebaseClient.Child(t.Id).PutAsync(t);
        }
    }
}
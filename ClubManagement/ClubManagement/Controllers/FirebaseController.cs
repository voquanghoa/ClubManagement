using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task Add(T t)
        {
            await FirebaseClient.PostAsync(t);
        }

        public List<T> Values
        {
            get
            {
                var users = FirebaseClient.OnceAsync<T>().Result.Select(x =>
                {
                    x.Object.Id = x.Key;

                    return x.Object;
                }).ToList();

                return users;
            }
        }

        public List<T> ValuesJustUpdate { get; private set; }

        public void UpdateValues()
        {
            ValuesJustUpdate = Values;
        }

        public async Task Edit(T t)
        {
            await FirebaseClient.Child(t.Id).PutAsync(t);
        }

        public async Task Delete(T t)
        {
            await FirebaseClient.Child(t.Id).DeleteAsync();
        }
    }
}
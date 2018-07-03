using ClubManagement.Models;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class UserEventsController : FirebaseController<UserEventModel>
    {
        private const string Key = "userEvents";

        public static UserEventsController Instance = new UserEventsController();

        private UserEventsController()
        {
            FirebaseClient = new FirebaseClient(LinkFirebase).Child(Key);
        }
    }
}
using ClubManagement.Models;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class UserEventsController : FirebaseController<UserEventModel>
    {
        private const string KEY = "userEvents";

        public static UserEventsController Instance = new UserEventsController();

        private UserEventsController()
        {
            firebaseClient = new FirebaseClient(LINK_FIREBASE).Child(KEY);
        }
    }
}
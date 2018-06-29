using ClubManagement.Models;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class EventsController : FirebaseController<EventModel>
    {
        private const string KEY = "events";

        public static EventsController Instance = new EventsController();

        private EventsController()
        {
            firebaseClient = new FirebaseClient(LINK_FIREBASE).Child(KEY);
        }
    }
}
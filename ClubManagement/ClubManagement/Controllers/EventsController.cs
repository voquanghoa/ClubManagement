using ClubManagement.Models;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class EventsController : FirebaseController<EventModel>
    {
        private const string Key = "events";

        public static EventsController Instance = new EventsController();

        private EventsController()
        {
            FirebaseClient = new FirebaseClient(LinkFirebase).Child(Key);
        }
    }
}
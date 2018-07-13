using ClubManagement.Models;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class OutComesController : FirebaseController<OutcomeModel>
    {
        private const string Key = "outComes";

        public static OutComesController Instance = new OutComesController();

        private OutComesController()
        {
            FirebaseClient = new FirebaseClient(LinkFirebase).Child(Key);
        }
    }
}
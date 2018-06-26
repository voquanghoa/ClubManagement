using ClubManagement.Models;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class OutComesController : FirebaseController<OutComeModel>
    {
        private const string KEY = "outComes";

        public static OutComesController Instance = new OutComesController();

        private OutComesController()
        {
            firebaseClient = new FirebaseClient(LINK_FIREBASE).Child(KEY);
        }
    }
}
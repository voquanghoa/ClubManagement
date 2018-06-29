using ClubManagement.Models;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class UsersController : FirebaseController<UserModel>
    {
        private const string KEY = "users";

        public static UsersController Instance = new UsersController();

        private UsersController()
        {
            firebaseClient = new FirebaseClient(LINK_FIREBASE).Child(KEY);
        }
    }
}
using ClubManagement.Models;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class UsersController : FirebaseController<UserModel>
    {
        private const string Key = "users";

        public static UsersController Instance = new UsersController();

        private UsersController()
        {
            FirebaseClient = new FirebaseClient(LinkFirebase).Child(Key);
        }
    }
}
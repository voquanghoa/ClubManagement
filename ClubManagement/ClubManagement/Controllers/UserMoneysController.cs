using ClubManagement.Models;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class UserMoneysController : FirebaseController<UserMoneyModel>
    {
        private const string KEY = "userMoneys";

        public static UserMoneysController Instance = new UserMoneysController();

        private UserMoneysController()
        {
            firebaseClient = new FirebaseClient(LINK_FIREBASE).Child(KEY);
        }
    }
}
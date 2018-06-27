using ClubManagement.Models;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class UserMoneysController : FirebaseController<UserMoneyModel>
    {
        private const string Key = "userMoneys";

        public static UserMoneysController Instance = new UserMoneysController();

        private UserMoneysController()
        {
            FirebaseClient = new FirebaseClient(LinkFirebase).Child(Key);
        }
    }
}
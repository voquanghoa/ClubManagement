using ClubManagement.Models;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class MoneysController : FirebaseController<MoneyModel>
    {
        private const string KEY = "moneys";

        public static MoneysController Instance = new MoneysController();

        private MoneysController()
        {
            firebaseClient = new FirebaseClient(LINK_FIREBASE).Child(KEY);
        }
    }
}
using ClubManagement.Models;
using Firebase.Xamarin.Database;

namespace ClubManagement.Controllers
{
    public class MoneysController : FirebaseController<MoneyModel>
    {
        private const string Key = "moneys";

        public static MoneysController Instance = new MoneysController();

        private MoneysController()
        {
            FirebaseClient = new FirebaseClient(LinkFirebase).Child(Key);
        }
    }
}
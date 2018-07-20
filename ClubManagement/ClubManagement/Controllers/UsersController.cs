using System.Threading;
using ClubManagement.Models;
using Firebase.Xamarin.Database;
using Plugin.Geolocator;

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

        public void UpdateUserLocation(UserModel user)
        {
            new Thread(async () =>
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var currentLocation = await locator.GetPositionAsync();
                user.Latitude = currentLocation.Latitude;
                user.Longitude = currentLocation.Longitude;
                await Edit(user);
            }).Start();
        }
    }
}
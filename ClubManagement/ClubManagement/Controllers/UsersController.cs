using System.Threading;
using ClubManagement.Models;
using Firebase.Xamarin.Database;
using Plugin.Geolocator;
using System;
using Firebase.Iid;

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

        public async void UpdateUserLocation(UserModel user)
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var currentLocation = await locator.GetPositionAsync();
            user.Latitude = currentLocation.Latitude;
            user.Longitude = currentLocation.Longitude;
            user.LastLogin = DateTime.Now;

            await Edit(user);
        }

        public async void UpdateUserNotificationToken(UserModel user, string token = null)
        {
            user.NotificationToken = token ?? FirebaseInstanceId.Instance.Token;

            await Edit(user);
        }
    }
}
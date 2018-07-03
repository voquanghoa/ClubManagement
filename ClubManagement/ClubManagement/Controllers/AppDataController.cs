using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.App;
using Android.Preferences;
using ClubManagement.Models;

namespace ClubManagement.Controllers
{
    public class AppDataController
    {
        public static AppDataController Instance = new AppDataController();

        private readonly string userId;

        private AppDataController()
        {
            userId = PreferenceManager.GetDefaultSharedPreferences(Application.Context)
                .GetString("UserId", string.Empty);
        }

        public int NumberOfUnpaidBudgets
        {
            get
            {
                var result = 0;
                new Thread(() =>
                {
                    var numberOfBudgets = MoneysController.Instance.Values.Count;
                    var numberOfPaidBudgets = UserMoneysController.Instance.Values.Count(x => x.UserId == userId);
                    result = numberOfBudgets - numberOfPaidBudgets;
                }).Start();
                return result;
            }
        }

        public List <EventModel> UpcomingEvents 
        {
            get
            {
                var joinedEvents = UserEventsController.Instance.Values.Where(x => x.UserId == userId);
                return EventsController.Instance.Values
                    .Join(joinedEvents, e => e.Id, j => j.EventId, (e, j) => e)
                    .Where(e => e.Time > DateTime.Now)
                    .ToList();
            }
        }
    }
}
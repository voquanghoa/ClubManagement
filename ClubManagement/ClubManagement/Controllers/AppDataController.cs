using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Preferences;
using Android.Util;
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
                var moneyList = MoneysController.Instance.Values ?? new List<MoneyModel>();
                var userMoneyList = UserMoneysController.Instance.Values ?? new List<UserMoneyModel>();
                return moneyList.Count - userMoneyList.Count(x => x.UserId == userId);
            }
        }

        public List <EventModel> UpcomingEvents 
        {
            get
            {
                var joinedEvents = (UserEventsController.Instance.Values ??
                                    new List<UserEventModel>()).Where(x => x.UserId == userId).ToList();
                var eventList = EventsController.Instance.Values ?? new List<EventModel>();
                return joinedEvents.Join(eventList, j => j.EventId, e => e.Id, (j, e) => e)
                    .Where(e => e.Time > DateTime.Now).ToList();
            }
        }

        public List<MoneyState> GetListMoneyState()
        {
            var moneyStates = new List<MoneyState>();
            var moneyList = MoneysController.Instance.Values ?? new List<MoneyModel>();
            var paidMoneyIdList = (UserMoneysController.Instance.Values ?? new List<UserMoneyModel>())
                .Where(x => x.UserId == userId)
                .Select(x => x.MoneyId)
                .ToList(); 
            moneyList.ForEach(x => moneyStates.Add(new MoneyState
            {
                IsPaid = paidMoneyIdList.Contains(x.Id),
                MoneyModel = x
            }));
            return moneyStates;
        }

        public List<MoneyAdminState> GetMoneyAdminStates(string moneyId)
        {
            var moneyAdminStates = new List<MoneyAdminState>();
            var userList = UsersController.Instance.Values ?? new List<UserModel>();
            var paidUserIds = (UserMoneysController.Instance.Values ?? new List<UserMoneyModel>())
                .Where(x => x.MoneyId == moneyId)
                .Select(x => x.UserId)
                .ToList();

            userList.ForEach(x => moneyAdminStates.Add(new MoneyAdminState
            {
                User = x,
                IsPaid = paidUserIds.Contains(x.Id)
            }));
            return moneyAdminStates;
        }
    }
}
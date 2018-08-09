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

        private string userId;

        private string userName;

        private bool isAdmin;

        public string UserId
        {
            get => userId;
        }

        public string UserName
        {
            get => userName;
        }

        public bool IsAdmin
        {
            get => isAdmin;
        }

        private MoneysController moneysController = MoneysController.Instance;

        private UserMoneysController userMoneysController = UserMoneysController.Instance;

        private UsersController usersController = UsersController.Instance;

        public void UpdateUser()
        {
            userId = PreferenceManager.GetDefaultSharedPreferences(Application.Context)
                .GetString("UserId", string.Empty);

            var user = usersController.Values.FirstOrDefault(x => x.Id == UserId);

            userName = user?.Name;
            isAdmin = user?.IsAdmin ?? true;
        }

        public int NumberOfUnpaidBudgets
        {
            get
            {
                var moneyList = MoneysController.Instance.Values ?? new List<MoneyModel>();
                var userMoneyList = UserMoneysController.Instance.Values ?? new List<UserMoneyModel>();
                return moneyList.Count - userMoneyList.Count(x => x.UserId == UserId);
            }
        }

        public List <EventModel> UpcomingEvents 
        {
            get
            {
                var joinedEvents = (UserEventsController.Instance.Values ??
                                    new List<UserEventModel>()).Where(x => x.UserId == UserId).ToList();
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
                .Where(x => x.UserId == UserId)
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

        public List<IncomeModel> Incomes
        {
            get
            {
                return userMoneysController.Values.Join(moneysController.Values,
                x => x.MoneyId,
                y => y.Id,
                (x, y) => new
                {
                    x,
                    y
                }).Join(usersController.Values,
                    x => x.x.UserId,
                    y => y.Id,
                    (x, y) => new IncomeModel()
                    {
                        Title = y.Name,
                        Description = x.y.Description,
                        Amount = x.y.Amount,
                        Date = x.y.Time
                    }).ToList();
            }
        }
    }
}
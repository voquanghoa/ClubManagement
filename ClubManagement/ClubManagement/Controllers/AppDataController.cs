using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Preferences;
using ClubManagement.Models;

namespace ClubManagement.Controllers
{
    public class AppDataController
    {
        public static AppDataController Instance = new AppDataController();       

        public string UserId { get; private set; }

        public string UserName { get; private set; }

        public bool IsAdmin { get; private set; }

        private readonly MoneysController moneysController = MoneysController.Instance;

        private readonly UserMoneysController userMoneysController = UserMoneysController.Instance;

        private readonly UsersController usersController = UsersController.Instance;

        private UserModel user;

        public UserModel User => user;

        public void UpdateUser()
        {
            UserId = PreferenceManager.GetDefaultSharedPreferences(Application.Context)
                .GetString("UserId", string.Empty);

            user = usersController.Values.FirstOrDefault(x => x.Id == UserId);

            UserName = user?.Name;
            IsAdmin = user?.IsAdmin ?? true;
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

        public List <EventModel> GoingEvents 
        {
            get
            {
                var joinedEvents = (UserEventsController.Instance.Values ??
                                    new List<UserEventModel>()).Where(x => x.UserId == UserId).ToList();
                var eventList = EventsController.Instance.Values ?? new List<EventModel>();
                return joinedEvents.Join(eventList, j => j.EventId, e => e.Id, (j, e) => e)
                    .Where(e => e.TimeStart.Date >= DateTime.Today).ToList();
            }
        }

		public int NumberOfUpComingEvents => (EventsController.Instance.Values ?? new List<EventModel>())
			.Count(e => e.TimeStart.Date >= DateTime.Today);

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
            var paidUsers = (UserMoneysController.Instance.Values ?? new List<UserMoneyModel>())
                .Where(x => x.MoneyId == moneyId)
                .ToList();

            userList.ForEach(x => moneyAdminStates.Add(new MoneyAdminState
            {
                User = x,
                IsPaid = paidUsers.Select(userMoney => userMoney.UserId).Contains(x.Id),
                PaidTime = paidUsers.Any(u => u.UserId == x.Id) ? paidUsers.First(u => u.UserId == x.Id).PaidTime : new DateTime()
            }));
            return moneyAdminStates;
        }

        public List<IncomeModel> Incomes
        {
            get
            {
                var moneyList = MoneysController.Instance.Values ?? new List<MoneyModel>();
                var userMoneyList = UserMoneysController.Instance.Values ?? new List<UserMoneyModel>();
                var numberOfUsers = (UsersController.Instance.Values ?? new List<UserModel>()).Count;

                var incomes = new List<IncomeModel>();
                moneyList.ForEach(x =>
                {
                    incomes.Add(new IncomeModel(x)
                    {
                        NumberOfPaidUsers = userMoneyList.Count(u => u.MoneyId == x.Id),
                        NumberOfUsers = numberOfUsers
                    });
                });

                return incomes;
            }
        }
    }
}
using System;
using System.Linq;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;

namespace ClubManagement.Fragments
{
    public class DashboardFragment : Fragment
    {
        private int unpaidBudgets;

        private int upcomingEvents;

        private int timeToNextUpcomingEvent;
        
        private readonly AppDataController appDataController = AppDataController.Instance;

        [InjectView(Resource.Id.tvBudget)]
        private TextView tvBudget;

        [InjectView(Resource.Id.tvEvents)]
        private TextView tvEvents;

        [InjectView(Resource.Id.tvUpcomingEvent)]
        private TextView tvUpcomingEvent;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_dashboard, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

        private void Init()
        {
            unpaidBudgets = appDataController.NumberOfUnpaidBudgets;
            upcomingEvents = appDataController.UpcomingEvents.Count;
            timeToNextUpcomingEvent = (appDataController.UpcomingEvents?.OrderBy(x => x.Time).First().Time - DateTime.Now).Value.Days;

            tvBudget.Text = $"You need to pay {unpaidBudgets} " + (unpaidBudgets > 1 ? "budgets" : "budget");
            tvEvents.Text = $"You have {upcomingEvents} upcoming " + (upcomingEvents > 1 ? "events" : "event");
            tvUpcomingEvent.Text = $"Your next event will be held in {timeToNextUpcomingEvent} " + (timeToNextUpcomingEvent > 1 ? "days" : "day");
        }
    }
}
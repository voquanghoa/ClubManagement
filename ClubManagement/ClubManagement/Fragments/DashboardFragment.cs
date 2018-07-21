using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using ClubManagement.Fragments.Bases;
using Android.Support.V4.Widget;

namespace ClubManagement.Fragments
{
    public class DashboardFragment : SwipeToRefreshDataFragment<string>
    {
        private int unpaidBudgets;

        private List<EventModel> upcomingEvents;

        private int timeToNextUpcomingEvent;
        
        private readonly AppDataController appDataController = AppDataController.Instance;

        [InjectView(Resource.Id.tvBudget)]
        private TextView tvBudget;

        [InjectView(Resource.Id.tvEvents)]
        private TextView tvEvents;

        [InjectView(Resource.Id.tvUpcomingEvent)]
        private TextView tvUpcomingEvent;

        protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

        [InjectOnClick(Resource.Id.btnLogout)]
        private void Logout(object s, EventArgs e)
        {
            DialogExtensions.ShowLogoutDialog(Context);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_dashboard, container, false);
            Cheeseknife.Inject(this, view);
            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            UpdateViewData();
        }

        protected override string QueryData()
        {
            unpaidBudgets = appDataController.NumberOfUnpaidBudgets;
            upcomingEvents = appDataController.UpcomingEvents;
            timeToNextUpcomingEvent = upcomingEvents.Any() ? (appDataController.UpcomingEvents.OrderBy(x => x.Time).First().Time - DateTime.Now).Days : 0;

            return null;
        }

        protected override void DisplayData(string data)
        {
            if (unpaidBudgets != 0)
            {
                tvBudget.Text = $"You need to pay {unpaidBudgets} " + (unpaidBudgets > 1 ? "budgets" : "budget");
                tvBudget.Visibility = ViewStates.Visible;
            }

            if (upcomingEvents.Count != 0)
            {
                tvEvents.Text = $"You have {upcomingEvents.Count} upcoming " + (upcomingEvents.Count > 1 ? "events" : "event");
                tvUpcomingEvent.Text = $"Your next event will be held in {timeToNextUpcomingEvent} " + (timeToNextUpcomingEvent > 1 ? "days" : "day");
                tvEvents.Visibility = ViewStates.Visible;
                tvUpcomingEvent.Visibility = ViewStates.Visible;
            }
        }
    }
}
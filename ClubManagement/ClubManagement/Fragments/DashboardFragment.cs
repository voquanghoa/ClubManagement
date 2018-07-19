using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using ClubManagement.Fragments.Bases;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
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
            if (unpaidBudgets == 0)
            {
                tvBudget.SetHeight(0);
            }
            else
            {
                tvBudget.Text = $"You need to pay {unpaidBudgets} " + (unpaidBudgets > 1 ? "budgets" : "budget");
            }

            if (upcomingEvents.Count == 0)
            {
                tvEvents.SetHeight(0);
                tvUpcomingEvent.SetHeight(0);
            }
            else
            {
                tvEvents.Text = $"You have {upcomingEvents} upcoming " + (upcomingEvents.Count > 1 ? "events" : "event");
                tvUpcomingEvent.Text = $"Your next event will be held in {timeToNextUpcomingEvent} " + (timeToNextUpcomingEvent > 1 ? "days" : "day");
            }
        }
    }
}
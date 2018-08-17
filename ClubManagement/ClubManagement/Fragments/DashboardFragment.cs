using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using ClubManagement.Fragments.Bases;
using Android.Support.V4.Widget;
using Java.Util;

namespace ClubManagement.Fragments
{
    public class DashboardFragment : SwipeToRefreshDataFragment<string>
    {
        [InjectView(Resource.Id.tvDashboardTitle)] private TextView tvDashboardTitle;

        [InjectView(Resource.Id.tvDashboardItemGoingTitle)] private TextView tvDashboadItemGoingTitle;

        [InjectView(Resource.Id.tvDashboardItemGoingCount)] private TextView tvDashboadItemGoingCount;

        [InjectView(Resource.Id.tvDashboardItemGoingEvents)] private TextView tvDashboadItemGoingEvents;

        [InjectView(Resource.Id.tvDashboardItemEventsTitle)] private TextView tvDashboardItemEventsTitle;

        [InjectView(Resource.Id.tvDashboardItemEventsCount)] private TextView tvDashboardItemEventsCount;

        [InjectView(Resource.Id.tvDashboardItemEvents)] private TextView tvDashboardItemEvents;

        [InjectView(Resource.Id.tvDashboardItemNextEventTitle)] private TextView tvDashboardItemNextEventTitle;

        [InjectView(Resource.Id.tvDashboardItemNextEventMonth)] private TextView tvDashboardItemNextEventMonth;

        [InjectView(Resource.Id.tvDashboardItemNextEventDate)] private TextView tvDashboardItemNextEventDate;

        [InjectView(Resource.Id.tvDashboardItemNeedToPayTitle)] private TextView tvDashboardItemNeedToPayTitle;

        [InjectView(Resource.Id.tvDashboardItemNeedToPayCount)] private TextView tvDashboardItemNeedToPayCount;

        [InjectView(Resource.Id.tvDashboardItemNeedToPayFees)] private TextView tvDashboardItemNeedToPayFees;

        private int unpaidBudgets;

        private List<EventModel> upcomingEvents = new List<EventModel>();

        private int timeToNextUpcomingEvent;
        
        private readonly AppDataController appDataController = AppDataController.Instance;

        [InjectView(Resource.Id.tvVersion)] private TextView tvVersion;

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
            GetAndShowAppVersion();
            Init();
            DisplayData(data);
            return view;
        }

        private void Init()
        {
            tvDashboardTitle.SetTextFont(TypefaceStyle.Bold);
            tvDashboadItemGoingTitle.SetTextFont(TypefaceStyle.Normal);
            tvDashboadItemGoingCount.SetTextFont(TypefaceStyle.Normal);
            tvDashboadItemGoingEvents.SetTextFont(TypefaceStyle.Normal);
            tvDashboardItemEventsTitle.SetTextFont(TypefaceStyle.Normal);
            tvDashboardItemEventsCount.SetTextFont(TypefaceStyle.Normal);
            tvDashboardItemEvents.SetTextFont(TypefaceStyle.Normal);
            tvDashboardItemNextEventTitle.SetTextFont(TypefaceStyle.Normal);
            tvDashboardItemNextEventMonth.SetTextFont(TypefaceStyle.Normal);
            tvDashboardItemNextEventDate.SetTextFont(TypefaceStyle.Normal);
            tvDashboardItemNeedToPayTitle.SetTextFont(TypefaceStyle.Normal);
            tvDashboardItemNeedToPayCount.SetTextFont(TypefaceStyle.Normal);
            tvDashboardItemNeedToPayFees.SetTextFont(TypefaceStyle.Normal);
        }
        public override void OnResume()
        {
            base.OnResume();
            UpdateViewData();
        }

        protected override string QueryData()
        {
            try
            {
                unpaidBudgets = appDataController.NumberOfUnpaidBudgets;
                upcomingEvents = appDataController.UpcomingEvents;
                timeToNextUpcomingEvent = upcomingEvents.Any() ? (appDataController.UpcomingEvents.OrderBy(x => x.Time).First().Time - DateTime.Now).Days : 0;

            }
            catch (Exception)
            {
                Toast.MakeText(Context, Resources.GetString(Resource.String.no_internet_connection), ToastLength.Short).Show();
            }
            return null;
        }

        protected override void DisplayData(string data)
        {
            //tvEvents.Visibility = ViewStates.Gone;
            //tvUpcomingEvent.Visibility = ViewStates.Gone;
            //tvBudget.Visibility = ViewStates.Gone;

            //if (unpaidBudgets != 0)
            //{
            //    tvBudget.Text = $"You need to pay {unpaidBudgets} " + (unpaidBudgets > 1 ? "budgets" : "budget");
            //    tvBudget.Visibility = ViewStates.Visible;
            //}

            //if (upcomingEvents != null && upcomingEvents.Any())
            //{
            //    tvEvents.Text = $"You have {upcomingEvents.Count} upcoming " + (upcomingEvents.Count > 1 ? "events" : "event");
            //    tvUpcomingEvent.Text = $"Your next event will be held in {timeToNextUpcomingEvent} " + (timeToNextUpcomingEvent > 1 ? "days" : "day");
            //    tvEvents.Visibility = ViewStates.Visible;
            //    tvUpcomingEvent.Visibility = ViewStates.Visible;
            //}
        }

        private void GetAndShowAppVersion()
        {
            try
            {
                var packageInfo = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0);
                tvVersion.Text = $"Version: {packageInfo.VersionName}";
            }
            catch (PackageManager.NameNotFoundException e)
            {
                Toast.MakeText(Context, e.Message, ToastLength.Short).Show();
            }
        }
    }
}
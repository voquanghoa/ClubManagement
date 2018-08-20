using System;
using System.Collections.Generic;
using System.Globalization;
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

        [InjectView(Resource.Id.itemGoingParentView)] private LinearLayout itemGoingParentView;

        [InjectView(Resource.Id.itemNewEventsParentView)] private LinearLayout itemNewEventsParentView;

        [InjectView(Resource.Id.itemNextEventParentView)] private LinearLayout itemNextEventParentView;

        [InjectView(Resource.Id.itemNeedToPayParentView)] private LinearLayout itemNeedToPayParentView;

        private int unpaidBudgets;

        private EventModel nextEvent;

        private List<EventModel> goingEvents = new List<EventModel>();

        private int numberOfUpComingEvent;

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
            SetTextFont();
            DisplayData(data);
            return view;
        }

        private void SetTextFont()
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
                goingEvents = appDataController.GoingEvents;
                numberOfUpComingEvent = appDataController.NumberOfUpComingEvents;
                nextEvent = goingEvents.Any() ? goingEvents.OrderBy(x => x.Time).First() : null;
            }
            catch (Exception)
            {
                Toast.MakeText(Context, Resources.GetString(Resource.String.no_internet_connection), ToastLength.Short).Show();
            }
            return null;
        }

        protected override void DisplayData(string data)
        {
            itemGoingParentView.Visibility = ViewStates.Gone;
            itemNewEventsParentView.Visibility = ViewStates.Gone;
            itemNextEventParentView.Visibility = ViewStates.Gone;
            itemNeedToPayParentView.Visibility = ViewStates.Gone;

            SetDataToTextview(itemGoingParentView, tvDashboadItemGoingCount, goingEvents.Count,
                tvDashboadItemGoingEvents, "event");
            SetDataToTextview(itemNewEventsParentView, tvDashboardItemEventsCount,
                numberOfUpComingEvent - goingEvents.Count, tvDashboardItemEvents, "event");
            SetDataToTextview(itemNeedToPayParentView, tvDashboardItemNeedToPayCount, unpaidBudgets,
                tvDashboardItemNeedToPayFees, "fee");

            if (nextEvent != null)
            {
                itemNextEventParentView.Visibility = ViewStates.Visible;
                tvDashboardItemNextEventMonth.Text = nextEvent.Time.ToString("MMM", CultureInfo.InvariantCulture);
                tvDashboardItemNextEventDate.Text = nextEvent.Time.Day.ToString();
            }
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

        private void SetDataToTextview(View parent, TextView tvCount, int count, TextView tvUnit, string unit)
        {
            if (count <= 0) return;
            parent.Visibility = ViewStates.Visible;
            tvCount.Text = count.ToString();
            tvUnit.Text = unit + (count > 1 ? "s" : string.Empty);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Fragments.Bases;
using Refractored.Controls;
using Square.Picasso;
using Android.Support.V4.Widget;
using Java.Lang.Reflect;
using ClubManagement.Ultilities;

namespace ClubManagement.Fragments
{
    public class DashboardFragment : SwipeToRefreshDataFragment<string>
    {
        private int unpaidBudgets;

        private List<EventModel> upcomingEvents = new List<EventModel>();

        private int timeToNextUpcomingEvent;

        private readonly AppDataController appDataController = AppDataController.Instance;

        [InjectView(Resource.Id.tvBudget)]
        private TextView tvBudget;

        [InjectView(Resource.Id.tvEvents)]
        private TextView tvEvents;

        [InjectView(Resource.Id.tvUpcomingEvent)]
        private TextView tvUpcomingEvent;

        [InjectView(Resource.Id.tvVersion)]
        private TextView tvVersion;

        [InjectView(Resource.Id.tvUserName)]
        private TextView tvUserName;

        [InjectView(Resource.Id.civAvatar)]
        private CircleImageView civAvatar;

        private const string UrlDefaultImage = "http://png.icons8.com/material-rounded/48/000000/user.png";

        protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_dashboard, container, false);
            Cheeseknife.Inject(this, view);

            tvUserName.Text = appDataController.UserName;
            Picasso.With(this.Context).Load(UrlDefaultImage).Fit().Into(civAvatar);

            civAvatar.Click += CivAvatar_Click;

            GetAndShowAppVersion();
            DisplayData(data);
            return view;
        }

        private void CivAvatar_Click(object sender, EventArgs e)
        {
            var popupMenu = new PopupMenu(this.Context, sender as View);

            var field = popupMenu.Class.GetDeclaredField("mPopup");
            field.Accessible = true;
            var menuPopupHelper = field.Get(popupMenu);
            var setForceIcons = menuPopupHelper.Class.GetDeclaredMethod("setForceShowIcon", Java.Lang.Boolean.Type);
            setForceIcons.Invoke(menuPopupHelper, true);

            popupMenu.Inflate(Resource.Menu.User);
            popupMenu.MenuItemClick += PopupMenu_MenuItemClick;

            popupMenu.Show();
        }

        private void PopupMenu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.changeAvatar:

                    break;
                case Resource.Id.logOut:
                    Context.ShowLogoutDialog();
                    break;
            }
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
            tvEvents.Visibility = ViewStates.Gone;
            tvUpcomingEvent.Visibility = ViewStates.Gone;
            tvBudget.Visibility = ViewStates.Gone;

            if (unpaidBudgets != 0)
            {
                tvBudget.Text = $"You need to pay {unpaidBudgets} " + (unpaidBudgets > 1 ? "budgets" : "budget");
                tvBudget.Visibility = ViewStates.Visible;
            }

            if (upcomingEvents != null && upcomingEvents.Any())
            {
                tvEvents.Text = $"You have {upcomingEvents.Count} upcoming " + (upcomingEvents.Count > 1 ? "events" : "event");
                tvUpcomingEvent.Text = $"Your next event will be held in {timeToNextUpcomingEvent} " + (timeToNextUpcomingEvent > 1 ? "days" : "day");
                tvEvents.Visibility = ViewStates.Visible;
                tvUpcomingEvent.Visibility = ViewStates.Visible;
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
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
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
using ClubManagement.Activities;
using Newtonsoft.Json;

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

        public event EventHandler ItemClick;

        [InjectOnClick(Resource.Id.itemGoingParentView)]
        private void ShowGoingEventsTab(object s, EventArgs e)
        {
            ItemClick?.Invoke(AppConstantValues.EventClickShowGoingEventsTabTag, e);
        }

        [InjectOnClick(Resource.Id.itemNewEventsParentView)]
        private void ShowNewEventsTab(object s, EventArgs e)
        {
            ItemClick?.Invoke(AppConstantValues.EventClickShowNewEventsTabTag, e);
        }

        [InjectOnClick(Resource.Id.itemNextEventParentView)]
        private void ShowNextEvent(object s, EventArgs e)
        {
            var userLoginEventModel = new UserLoginEventModel(nextEvent)
            {
                IsJoined = true
            };

            var eventDetail = JsonConvert.SerializeObject(userLoginEventModel);
            var intent = new Intent(Context, typeof(EventDetailActivity));
            intent.PutExtra("EventDetail", eventDetail);
            StartActivity(intent);
        }

        [InjectOnClick(Resource.Id.itemNeedToPayParentView)]
        private void ShowMoneyScreen(object s, EventArgs e)
        {
            ItemClick?.Invoke(AppConstantValues.EventClickShowMoneyScreenTag, e);
        }

        private int unpaidBudgets;

        private EventModel nextEvent;

        private List<EventModel> goingEvents = new List<EventModel>();

        private int numberOfUpComingEvent;

        private readonly AppDataController appDataController = AppDataController.Instance;

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
            SetTextFont();
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
            tvUserName.SetTextFont(TypefaceStyle.Bold);
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
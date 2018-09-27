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
using ClubManagement.Ultilities;
using Android.Preferences;
using Android.Support.Design.Widget;
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

        [InjectView(Resource.Id.parentViewFabAddFee)]
        private View parentViewFabAddFee;

        [InjectView(Resource.Id.parentViewFabAddOutCome)]
        private View parentViewFabAddOutCome;

        [InjectView(Resource.Id.fabAddEvent)] private FloatingActionButton fabAddEvent;

        [InjectView(Resource.Id.fabAddFee)] private FloatingActionButton fabAddFee;

        [InjectView(Resource.Id.fabAddOutCome)] private FloatingActionButton fabAddOutCome;

        [InjectView(Resource.Id.tvAddEvent)] private View tvAddEvent;

        [InjectView(Resource.Id.bg_fab_menu)] private View bgFabsMenu;

        public event EventHandler ItemClick;

        public event EventHandler AddClick;

        private bool isFabsMenuOpenning;

        public const int RequestAddEventCode = 1;

        public const int RequestAddFeeCode = 2;

        public const int RequestAddOutcomeCode = 3;

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

        private readonly ISharedPreferences preferences =
            PreferenceManager.GetDefaultSharedPreferences(Application.Context);

        [InjectView(Resource.Id.tvVersion)]
        private TextView tvVersion;

        [InjectView(Resource.Id.tvUserName)]
        private TextView tvUserName;

        [InjectView(Resource.Id.civAvatar)]
        private CircleImageView civAvatar;

        private readonly ChangeAvatarFragment changeAvatarFragment = new ChangeAvatarFragment();

        private string avatarUrl;

        protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

        public DashboardFragment()
        {
            changeAvatarFragment.ChangeAvatar += ChangeAvatarFragment_ChangeAvatar;
            
        }

        private void ChangeAvatarFragment_ChangeAvatar(object sender, EventArgs e)
        {
            if (sender is string imageUrl)
            {
                var preferencesEditor = preferences.Edit();
                preferencesEditor.PutString(AppConstantValues.UserAvatarUrl, imageUrl);
                preferencesEditor.Commit();
                LoadAvatar();

                Activity.DoRequest(async () =>
                {
                    appDataController.User.Avatar = imageUrl;

                    await UsersController.Instance.Edit(appDataController.User);
                });
            }
        }

        private void LoadAvatar()
        {
            avatarUrl = preferences.GetString(AppConstantValues.UserAvatarUrl, string.Empty);
			civAvatar.SetImageResource(Resource.Drawable.icon_user);

            if (!string.IsNullOrEmpty(avatarUrl))
            {
                Picasso.With(Context).Load(avatarUrl).Fit().Into(civAvatar);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_dashboard, container, false);
            Cheeseknife.Inject(this, view);

            InitFabsMenu();

            if (appDataController.UserName.Length > 10)
            {
                tvUserName.Text = appDataController.UserName.Substring(0, 10).ToString() + "...";
            }
            else
            {
                tvUserName.Text = appDataController.UserName;
            }
            
            LoadAvatar();

            civAvatar.Click += CivAvatar_Click;

            GetAndShowAppVersion();
            SetTextFont();
            DisplayData(data);
            return view;
        }

        private void CivAvatar_Click(object sender, EventArgs e)
        {
            if  (sender is View view)
            {
                var popupMenu = view.CreatepopupMenu(Resource.Menu.User);
                popupMenu.MenuItemClick += PopupMenu_MenuItemClick;
                popupMenu.Show();
            }
        }

        private void PopupMenu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.changeAvatar:
                    changeAvatarFragment.Show(FragmentManager, null);
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
                nextEvent = goingEvents.Any() ? goingEvents.OrderBy(x => x.TimeStart).First() : null;
            }
            catch (Exception ex)
            {
				this.ShowMessage(ex.Message);
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
                tvDashboardItemNextEventMonth.Text = nextEvent.TimeStart.ToString("MMM", CultureInfo.InvariantCulture);
                tvDashboardItemNextEventDate.Text = nextEvent.TimeStart.Day.ToString();
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
				this.ShowMessage(e.Message);
            }
        }

        private void SetDataToTextview(View parent, TextView tvCount, int count, TextView tvUnit, string unit)
        {
            if (count <= 0) return;
            parent.Visibility = ViewStates.Visible;
            tvCount.Text = count.ToString();
            tvUnit.Text = unit + (count > 1 ? "s" : string.Empty);
        }

        private void InitFabsMenu()
        {
            parentViewFabAddFee.Visibility = ViewStates.Gone;
            parentViewFabAddOutCome.Visibility = ViewStates.Gone;
            fabAddEvent.Visibility = ViewStates.Gone;
            tvAddEvent.Visibility = ViewStates.Gone;
            bgFabsMenu.Visibility = ViewStates.Gone;

            Context.DoWithAdmin(() =>
            {
                fabAddEvent.Visibility = ViewStates.Visible;
                fabAddEvent.Click += (s, e) =>
                {
                    if (isFabsMenuOpenning)
                    {
                        var intent = new Intent(Context, typeof(CreateEventActivity));
                        StartActivityForResult(intent, RequestAddEventCode);
                        CloseFabsMenu();
                    }
                    else
                    {
                        OpenFabsMenu();
                    }
                };

                bgFabsMenu.Click += (s, e) => CloseFabsMenu();

                fabAddFee.Click += (s, e) =>
                {
                    var intent = new Intent(Context, typeof(CreateFeeActivity));
                    StartActivityForResult(intent,RequestAddFeeCode);
                    CloseFabsMenu();
                };

                fabAddOutCome.Click += (s, e) =>
                {
                    var intent = new Intent(Context, typeof(CreateOutcomeActivity));
                    StartActivityForResult(intent, RequestAddOutcomeCode);
                    CloseFabsMenu();
                };
            });

        }

        private void OpenFabsMenu()
        {
            isFabsMenuOpenning = true;
            parentViewFabAddOutCome.Visibility = ViewStates.Visible;
            parentViewFabAddFee.Visibility = ViewStates.Visible;
            tvAddEvent.Visibility = ViewStates.Visible;
            bgFabsMenu.Visibility = ViewStates.Visible;
            bgFabsMenu.Animate().Alpha(1f);
            fabAddEvent.SetImageResource(Resource.Drawable.icon_add_event);
        }

        private void CloseFabsMenu()
        {
            isFabsMenuOpenning = false;
            parentViewFabAddOutCome.Visibility = ViewStates.Gone;
            parentViewFabAddFee.Visibility = ViewStates.Gone;
            tvAddEvent.Visibility = ViewStates.Gone;
            bgFabsMenu.Visibility = ViewStates.Gone;
            fabAddEvent.SetImageResource(Resource.Drawable.icon_add);
            bgFabsMenu.Animate().Alpha(0f);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok.GetHashCode())
            {
                AddClick.Invoke(requestCode, null);
            }
        }
    }
}
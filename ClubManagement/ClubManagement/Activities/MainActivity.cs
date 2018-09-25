using System.Collections.Generic;
using Android.App;
using Android.Support.V4.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Fragments;
using ClubManagement.Ultilities;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Content;
using Android.Runtime;

namespace ClubManagement.Activities
{
    [Activity(Label = "ClubManagement", Theme = "@style/AppTheme")]
    public class MainActivity : FragmentActivity
    {
        private bool doubleBackpress;

        [InjectView(Resource.Id.bottom_navigation_tabbar)]
        private BottomNavigationView bottomNavigationView;

        private static readonly DashboardFragment DashboardFragment = new DashboardFragment();

        private static readonly EventFragment EventFragment = new EventFragment();

        private static readonly MoneyFragment MoneyFragment = new MoneyFragment();

        private static readonly BalanceFragment BalanceFragment = new BalanceFragment();

        private static readonly NotificationFragment NotificationFragment = new NotificationFragment();

        private readonly Dictionary<int, Fragment> fragmentMapIds = new Dictionary<int, Fragment>
        {
            {
                Resource.Id.dashboardTab,
                DashboardFragment
            },
            {
                Resource.Id.eventTab,
                EventFragment
            },
            {
                Resource.Id.moneyTab,
                MoneyFragment
            },
            {
                Resource.Id.balanceTab,
                BalanceFragment
            },
            {
                Resource.Id.notificationTab,
                NotificationFragment
            }
        };

        public MainActivity()
        {
            DashboardFragment.AddClick += (s, e) =>
            {
                if (s is int requestCode)
                {
                    switch (requestCode)
                    {
                        case DashboardFragment.RequestAddFeeCode:
                            MoneyFragment.SelectedTabIndex = 2;
                            DisplayFragment(Resource.Id.moneyTab);
                            bottomNavigationView.SelectedItemId = Resource.Id.moneyTab;
                            break;
                        case DashboardFragment.RequestAddEventCode:
                            MoneyFragment.SelectedTabIndex = 0;
                            DisplayFragment(Resource.Id.eventTab);
                            bottomNavigationView.SelectedItemId = Resource.Id.eventTab;
                            break;
                    }
                }
            };

            DashboardFragment.ItemClick += (s, e) =>
            {
                if (!(s is string tag)) return;
                switch (tag)
                {
                    case AppConstantValues.EventClickShowGoingEventsTabTag:
                        DisplayFragment(Resource.Id.eventTab);
                        EventFragment.SelectedTabIndex = 1;
                        bottomNavigationView.SelectedItemId = Resource.Id.eventTab;
                        return;
                    case AppConstantValues.EventClickShowNewEventsTabTag:
                        DisplayFragment(Resource.Id.eventTab);
                        EventFragment.SelectedTabIndex = 0;
                        bottomNavigationView.SelectedItemId = Resource.Id.eventTab;
                        return;
                    case AppConstantValues.EventClickShowMoneyScreenTag:
                        DisplayFragment(Resource.Id.moneyTab);
                        bottomNavigationView.SelectedItemId = Resource.Id.moneyTab;
                        return;
                }
            };
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            Cheeseknife.Inject(this);
            ChangeStatusBarColor();
            BottomNavigationHelper.RemoveShiftMode(bottomNavigationView);
            bottomNavigationView.NavigationItemSelected += BottomNavigation_NavigationItemSelected;
            DisplayFragment(Resource.Id.dashboardTab);
        }
        

        private void DisplayFragment(int tag)
        {
            var fragmentTransaction = SupportFragmentManager.BeginTransaction();
            var fragment = SupportFragmentManager.FindFragmentByTag(tag.ToString());
                     
            if (fragment == null)
            {
                fragment = fragmentMapIds[tag];
				fragmentTransaction.Replace(Resource.Id.content_frame, fragment, tag.ToString());
				fragmentTransaction.AddToBackStack(null);
            }
            else
            {
				fragmentTransaction.Replace(Resource.Id.content_frame, fragment, tag.ToString());
            }

            fragmentTransaction.Commit();
        }

        private void BottomNavigation_NavigationItemSelected(object sender,
            BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
			DisplayFragment(e.Item.ItemId);
        }

        public override void OnBackPressed()
        {
            if (doubleBackpress)
            {
                Finish();
                return;
            }

            doubleBackpress = true;
			this.ShowMessage(Resource.String.back_to_exit);
            new Handler().PostDelayed(() => { doubleBackpress = false; }, 2000);
        }

        private void ChangeStatusBarColor()
        {
            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Resources.GetColor(Resource.Color.color_dark_blue, null));
        }
    }
}


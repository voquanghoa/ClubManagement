using System.Collections.Generic;
using Android.App;
using Android.Support.V4.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Widget;
using ClubManagement.Fragments;
using ClubManagement.Ultilities;
using Fragment = Android.Support.V4.App.Fragment;
using System;
using Android.Content;
using ClubManagement.Models;
using Newtonsoft.Json;

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
            DashboardFragment.ItemClick += (s, e) =>
            {
                if (!(s is string tag)) return;
                switch (tag)
                {
                    case AppConstantValues.EventClickShowGoingEventsTabTag:
                        DisplayFragment(Resource.Id.eventTab);
                        bottomNavigationView.SelectedItemId = Resource.Id.eventTab;
                        return;
                    case AppConstantValues.EventClickShowNewEventsTabTag:
                        DisplayFragment(Resource.Id.eventTab);
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
            Toast.MakeText(this, Resources.GetString(Resource.String.back_to_exit), ToastLength.Short).Show();
            new Handler().PostDelayed(() => { doubleBackpress = false; }, 2000);
        }
    }
}


using System.Collections.Generic;
using Android.App;
using Android.Support.V4.App;
using Android.OS;
using Android.Support.Design.Widget;
using ClubManagement.Fragments;
using ClubManagement.Ultilities;
using Fragment = Android.Support.V4.App.Fragment;
using System;

namespace ClubManagement.Activities
{
    [Activity(Label = "ClubManagement", Theme = "@style/AppTheme")]
    public class MainActivity : FragmentActivity
    {
        [InjectView(Resource.Id.bottom_navigation_tabbar)]
        private BottomNavigationView bottomNavigationView;

        private DashboardFragment dashboardFragment = new DashboardFragment();

        private EventFragment eventFragment = new EventFragment();

        private MoneyFragment moneyFragment = new MoneyFragment();

        private BalanceFragment balanceFragment = new BalanceFragment();

        private readonly Dictionary<int, Fragment> fragmentMapIds = new Dictionary<int, Fragment>
        {
            {
                Resource.Id.dashboardTab,
                new DashboardFragment()
            },
            {
                Resource.Id.eventTab,
                new EventFragment()
            },
            {
                Resource.Id.moneyTab,
                new MoneyFragment()
            },
            {
                Resource.Id.balanceTab,
                new BalanceFragment()
            },
        };

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

            var addToStack = true;

            if (fragment == null)
            {
                fragment = fragmentMapIds[tag];
            }
            else
            {
                addToStack = false;
            }

            fragmentTransaction.Replace(Resource.Id.content_frame, fragment, tag.ToString());

            if (addToStack) fragmentTransaction.AddToBackStack(null);

            fragmentTransaction.Commit();
        }

        private void BottomNavigation_NavigationItemSelected(object sender,
            BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
			DisplayFragment(e.Item.ItemId);
        }

        //public override void OnBackPressed()
        //{
        //}
    }
}


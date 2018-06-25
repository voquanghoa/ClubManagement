using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using ClubManagement.Fragments;
using ClubManagement.Ultilities;

namespace ClubManagement.Activities
{
    [Activity(Label = "ClubManagement", MainLauncher = true, Theme = "@style/AppTheme")]
    public class MainActivity : Activity
    {
        [InjectView(Resource.Id.bottom_navigation_tabbar)]
        private BottomNavigationView bottomNavigationView;

		private readonly Dictionary<int, Fragment> fragmentMapIds = new Dictionary<int, Fragment>()
        {
            {Resource.Id.dashboardTab, new DashboardFragment()},
            {Resource.Id.eventTab, new EventFragment()},
            {Resource.Id.moneyTab, new MoneyFragment()},
            {Resource.Id.balanceTab, new BalanceFragment()},

        };


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            Cheeseknife.Inject(this);
            BottomNavigationHelper.RemoveShiftMode(bottomNavigationView);
            bottomNavigationView.ItemIconTintList = null;
            bottomNavigationView.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

			DisplayFragment(Resource.Id.dashboardTab);
        }
        

        private void DisplayFragment(int fragmentMenuId)
        {
			var fragment = fragmentMapIds[fragmentMenuId];

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

        private void BottomNavigation_NavigationItemSelected(object sender,
            BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
			DisplayFragment(e.Item.ItemId);
        }
    }
}


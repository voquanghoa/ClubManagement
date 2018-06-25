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

        private readonly DashboardFragment dashboardFragment = new DashboardFragment();

        private readonly EventFragment eventFragment = new EventFragment();

        private readonly MoneyFragment moneyFragment = new MoneyFragment();

        private readonly BalanceFragment balanceFragment = new BalanceFragment();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            Cheeseknife.Inject(this);
            BottomNavigationHelper.RemoveShiftMode(bottomNavigationView);
            bottomNavigationView.ItemIconTintList = null;
            bottomNavigationView.NavigationItemSelected += BottomNavigation_NavigationItemSelected;
            LoadFragment(Resource.Id.dashboardTab);
        }

        private void LoadFragment(int id)
        {
            switch (id)
            {
                case Resource.Id.dashboardTab:
                    DisplayFragment(dashboardFragment);
                    break;
                case Resource.Id.eventTab:
                    DisplayFragment(eventFragment);
                    break;
                case Resource.Id.moneyTab:
                    DisplayFragment(moneyFragment);
                    break;
                case Resource.Id.balanceTab:
                    DisplayFragment(balanceFragment);
                    break;
            }
        }

        private void DisplayFragment(Fragment fragment)
        {
            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

        private void BottomNavigation_NavigationItemSelected(object sender,
            BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }
    }
}


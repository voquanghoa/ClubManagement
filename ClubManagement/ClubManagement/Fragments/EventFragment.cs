using Android.App;
using Android.OS;
using Android.Views;

namespace ClubManagement.Fragments
{
    public class EventFragment : Fragment
    {
        private View view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.Main, container, false);

            Activity.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            var allTab = Activity.ActionBar.NewTab();
            allTab.SetText("All");
            //tab.SetIcon(Resource.Drawable.tab1_icon);
            allTab.TabSelected += (sender, args) =>
            {
                // Do something when tab is selected
            };
            Activity.ActionBar.AddTab(allTab);

            var upcomingTab = Activity.ActionBar.NewTab();
            upcomingTab.SetText("Upcoming");
            //upcomingTab.cas
            //tab.SetIcon(Resource.Drawable.tab1_icon);
            upcomingTab.TabSelected += (sender, args) =>
            {
                // Do something when tab is selected
            };
            Activity.ActionBar.AddTab(upcomingTab);

            var joinedTab = Activity.ActionBar.NewTab();
            joinedTab.SetText("Joined");
            //tab.SetIcon(Resource.Drawable.tab1_icon);
            joinedTab.TabSelected += (sender, args) =>
            {
                // Do something when tab is selected
            };
            Activity.ActionBar.AddTab(joinedTab);

            return view;//base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using ClubManagement.Controllers;
using ClubManagement.Fragments.Bases;
using ClubManagement.Models;
using PagerAdapter = ClubManagement.CustomAdapters.PagerAdapter;

namespace ClubManagement.Fragments
{
	public class MoneyFragment : SwipeToRefreshDataFragment<List<MoneyState>>
    {
        [InjectView(Resource.Id.tlMoney)] private TabLayout tlMoney;

        [InjectView(Resource.Id.vpMoney)] private ViewPager vpMoney;

        private readonly AppDataController appDataController = AppDataController.Instance;

        private List<MoneyState> listMoneyStates;

        private PagerAdapter adapter;

        private readonly ListMoneyFragment allMoneyFragment = new ListMoneyFragment();

        private readonly ListMoneyFragment paidMoneyFragment = new ListMoneyFragment();

        private readonly ListMoneyFragment unpaidMoneyFragment = new ListMoneyFragment();

		protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_money, container, false);
            Cheeseknife.Inject(this, view);
            
			adapter = new PagerAdapter(Activity.SupportFragmentManager);

            adapter.AddFramgent(allMoneyFragment, "All");
            adapter.AddFramgent(paidMoneyFragment, "Already paid");
            adapter.AddFramgent(unpaidMoneyFragment, "Unpaid");
            vpMoney.Adapter = adapter;
            tlMoney.SetupWithViewPager(vpMoney);

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
			SwipeRefreshLayout.Refreshing = true;
            UpdateViewData();
        }

		protected override List<MoneyState> QueryData() => appDataController.GetListMoneyState();

		protected override void DisplayData(List<MoneyState> data)
		{
			allMoneyFragment.MoneyStates = data;
			paidMoneyFragment.MoneyStates = data.Where(x => x.IsPaid).ToList();
			unpaidMoneyFragment.MoneyStates = data.Where(x => !x.IsPaid).ToList();
		}
	}
}
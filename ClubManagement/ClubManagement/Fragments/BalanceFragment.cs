using System;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Views;
using ClubManagement.Models;
using System.Collections.Generic;
using ClubManagement.Ultilities;
using PagerAdapter = ClubManagement.CustomAdapters.PagerAdapter;
using ClubManagement.Fragments.Bases;
using Android.Support.V4.Widget;

namespace ClubManagement.Fragments
{
	public class BalanceFragment : Fragment
    {
        [InjectView(Resource.Id.tlBalance)] private TabLayout tlBalance;

        [InjectView(Resource.Id.vpBalance)] private ViewPager vpBalance;

        private List<IncomeModel> balances;

        private PagerAdapter adapter;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragmentBalance, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

		private void Init()
        {
            adapter = new PagerAdapter(Activity.SupportFragmentManager);
            adapter.AddFramgent(new BalanceSummaryFragment(), AppConstantValues.BalanceFragmentSummaryTab);
            adapter.AddFramgent(new BalancesFragment(BalancesFragment.Type.Income), AppConstantValues.BalanceFragmentIncomeTab);
            adapter.AddFramgent(new BalancesFragment(BalancesFragment.Type.Outcome), AppConstantValues.BalanceFragmentOutcomeTab);
            vpBalance.Adapter = adapter;
            tlBalance.SetupWithViewPager(vpBalance);
        }
    }
}
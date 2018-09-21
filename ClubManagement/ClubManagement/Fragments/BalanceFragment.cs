using System;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using ClubManagement.Models;
using System.Collections.Generic;
using ClubManagement.Ultilities;
using PagerAdapter = ClubManagement.CustomAdapters.PagerAdapter;
using ClubManagement.Fragments.Bases;
using Android.Support.V4.Widget;
using ClubManagement.Controllers;
using System.Linq;
using Android.Widget;

namespace ClubManagement.Fragments
{
    public class BalanceFragment : SwipeToRefreshDataFragment<List<OutcomeModel>>
    {
        [InjectView(Resource.Id.tlBalance)] private TabLayout tlBalance;

        [InjectView(Resource.Id.vpBalance)] private ViewPager vpBalance;

        private List<IncomeModel> balances;

        private PagerAdapter adapter;

        protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

        private BalanceSummaryFragment balanceSummaryFragment = new BalanceSummaryFragment();

        private BalancesFragment IncomesFragment = new BalancesFragment(BalancesFragment.Type.Income);

        private BalancesFragment OutcomesFragment = new BalancesFragment(BalancesFragment.Type.Outcome);

        private List<OutcomeModel> incomes = new List<OutcomeModel>();

        private List<OutcomeModel> outcomes = new List<OutcomeModel>();

        private long sumIncomes;

        private long sumOutcomes;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            adapter = new PagerAdapter(ChildFragmentManager);
            adapter.AddFramgent(balanceSummaryFragment, AppConstantValues.BalanceFragmentSummaryTab);
            adapter.AddFramgent(IncomesFragment, AppConstantValues.BalanceFragmentIncomeTab);
            adapter.AddFramgent(OutcomesFragment, AppConstantValues.BalanceFragmentOutcomeTab);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragmentBalance, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            UpdateViewData();
        }

        protected override void DisplayData(List<OutcomeModel> data)
        {
            IncomesFragment.Incomes = incomes;
            OutcomesFragment.Outcomes = outcomes;
            balanceSummaryFragment.SumIncomes = sumIncomes;
            balanceSummaryFragment.SumOutcomes = sumOutcomes;

            adapter.NotifyDataSetChanged();
        }

        protected override List<OutcomeModel> QueryData()
        {
            try
            {
                incomes = AppDataController.Instance.Incomes.Select(x => (OutcomeModel)x).OrderByDescending(x => x.Date).ToList();
                outcomes = OutComesController.Instance.Values.OrderByDescending(x => x.Date).ToList();
                sumIncomes = AppDataController.Instance.Incomes.Sum(x => x.Amount);
                sumOutcomes = OutComesController.Instance.Values.Sum(x => x.Amount);
            }
            catch (Exception)
            {
				this.ShowMessage(Resource.String.no_internet_connection);
            }

            return null;
        }

        private void Init()
        {
            vpBalance.Adapter = adapter;
            tlBalance.SetupWithViewPager(vpBalance);
        }
    }
}
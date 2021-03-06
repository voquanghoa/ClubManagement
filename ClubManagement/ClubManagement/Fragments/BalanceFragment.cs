﻿using System;
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
using Android.Support.V4.App;
using Android.Widget;

namespace ClubManagement.Fragments
{
    public class BalanceFragment : SwipeToRefreshDataFragment<List<OutcomeModel>>
    {
        [InjectView(Resource.Id.tlBalance)] private TabLayout tlBalance;

        [InjectView(Resource.Id.vpBalance)] private ViewPager vpBalance;

        private PagerAdapter adapter;

        protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

        private readonly BalanceSummaryFragment balanceSummaryFragment = new BalanceSummaryFragment();

        private List<IncomeModel> incomes = new List<IncomeModel>();

        private List<OutcomeModel> outcomes = new List<OutcomeModel>();

        private readonly BalanceListFragment incomeFragment = new BalanceListFragment(BalanceListFragment.IncomeType);

        private readonly BalanceListFragment outcomeFragment = new BalanceListFragment(BalanceListFragment.OutcomeType);

        private long sumIncomes;

        private long sumOutcomes;

        public int SelectedTabIndex { set; get; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            adapter = new PagerAdapter(ChildFragmentManager);
            adapter.AddFramgent(balanceSummaryFragment, AppConstantValues.BalanceFragmentSummaryTab);
            adapter.AddFramgent(incomeFragment, AppConstantValues.BalanceFragmentIncomeTab);
            adapter.AddFramgent(outcomeFragment, AppConstantValues.BalanceFragmentOutcomeTab);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragmentBalance, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            tlBalance.GetTabAt(SelectedTabIndex).Select();
        }

        public override void OnResume()
        {
            base.OnResume();
            UpdateViewData();
        }

        protected override void DisplayData(List<OutcomeModel> data)
        {
            balanceSummaryFragment.SumIncomes = sumIncomes;
            balanceSummaryFragment.SumOutcomes = sumOutcomes;
            incomeFragment.IncomeModels = incomes;
            outcomeFragment.OutcomeModels = outcomes;
            adapter.NotifyDataSetChanged();
        }

        protected override List<OutcomeModel> QueryData()
        {
            try
            {
                incomes = AppDataController.Instance.Incomes.OrderByDescending(x => x.Time).ToList();
                outcomes = OutComesController.Instance.Values.OrderByDescending(x => x.Date).ToList();
                sumIncomes = incomes.Sum(x => x.NumberOfPaidUsers * x.Amount);
                sumOutcomes = outcomes.Sum(x => x.Amount);
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
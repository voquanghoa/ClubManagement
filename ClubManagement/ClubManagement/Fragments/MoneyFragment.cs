﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using ClubManagement.Controllers;
using ClubManagement.Fragments.Bases;
using ClubManagement.Models;
using Android.Widget;
using PagerAdapter = ClubManagement.CustomAdapters.PagerAdapter;
using ClubManagement.Ultilities;

namespace ClubManagement.Fragments
{
    public class MoneyFragment : SwipeToRefreshDataFragment<List<MoneyState>>
    {
        [InjectView(Resource.Id.tlMoney)] private TabLayout tlMoney;

        [InjectView(Resource.Id.vpMoney)] private ViewPager vpMoney;

        private readonly MoneySummaryFragment moneySummaryFragment = new MoneySummaryFragment();

        private readonly MoneyListFragment moneyPaidListFragment = new MoneyListFragment();

        private readonly MoneyListFragment moneyUnpaidListFragment = new MoneyListFragment();

        private PagerAdapter pagerAdapter;

        public int SelectedTabIndex { set; get; }

        public MoneyFragment()
        {
            data = new List<MoneyState>();
            moneyPaidListFragment.AdapterItemDeleteClick += (s, e) =>
            {
                if (s is string message && message == "Success")
                {
                    UpdateViewData();
                }
            };

            moneyUnpaidListFragment.AdapterItemDeleteClick += (s, e) =>
            {
                if (s is string message && message == "Success")
                {
                    UpdateViewData();
                }
            };
        }

        private readonly AppDataController appDataController = AppDataController.Instance;

        protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            InitTabs();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_money, container, false);

            Cheeseknife.Inject(this, view);

            Init();

            return view;
        }

        private void InitTabs()
        {
            pagerAdapter = new PagerAdapter(ChildFragmentManager);
            pagerAdapter.AddFramgent(moneySummaryFragment, "Summary");
            pagerAdapter.AddFramgent(moneyPaidListFragment, "Paid");
            pagerAdapter.AddFramgent(moneyUnpaidListFragment, "Unpaid");
        }

        private void Init()
        {
            vpMoney.Adapter = pagerAdapter;
            tlMoney.SetupWithViewPager(vpMoney);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            tlMoney.GetTabAt(SelectedTabIndex).Select();
        }

        public override void OnResume()
        {
            base.OnResume();
            UpdateViewData();
        }

        protected override List<MoneyState> QueryData()
        {
            try
            {
                return appDataController.GetListMoneyState().OrderByDescending(x => x.MoneyModel.Time).ToList();
            }
            catch (Exception ex)
            {
				this.ShowMessage(ex.Message);
                return data;
            }
        }

        protected override void DisplayData(List<MoneyState> data)
        {
            moneySummaryFragment.MoneyStates = data;
            moneyPaidListFragment.MoneyStates = data.Where(x => x.IsPaid).OrderByDescending(x => x.MoneyModel.Time).ToList();
            moneyUnpaidListFragment.MoneyStates = data.Where(x => !x.IsPaid).OrderByDescending(x => x.MoneyModel.Time).ToList();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;
using ClubManagement.Controllers;
using ClubManagement.CustomAdapters;
using ClubManagement.Fragments.Bases;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using Android.Support.V7.Widget;
using Android.Widget;

namespace ClubManagement.Fragments
{
    public class MoneyFragment : SwipeToRefreshDataFragment<List<MoneyState>>
    {
        [InjectView(Resource.Id.tlMoney)] private TabLayout tlMoney;

        [InjectView(Resource.Id.rvMoney)] private RecyclerView rvMoney;

        [InjectView(Resource.Id.fabAdd)] private FloatingActionButton fabAdd;

        [InjectOnClick(Resource.Id.fabAdd)]
        private void Add(object s, EventArgs e)
        {
            Context.DoWithAdmin(() =>
            {
                AddBudgetDialogFragment.Show(FragmentManager, null);
            });
        }

        public MoneyFragment()
        {
            AddBudgetDialogFragment.SaveClick += (s, e) =>
            {
                if (s is MoneyModel moneyModel)
                {
                    data.Insert(0, new MoneyState
                    {
                        MoneyModel = moneyModel,
                        IsPaid = false
                    });
                    adapter.MoneyStates = data;
                }
            };
        }

        public  AddBudgetDialogFragment AddBudgetDialogFragment = new AddBudgetDialogFragment();

        private readonly MoneyListAdapter adapter = new MoneyListAdapter();

        private readonly AppDataController appDataController = AppDataController.Instance;

        protected override SwipeRefreshLayout SwipeRefreshLayout => View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_money, container, false);

            Cheeseknife.Inject(this, view);

            SetupTabView();

            Init();

            return view;
        }

        private void Init()
        {
			fabAdd.ShowIfAdmin();
        }
        private void SetupTabView()
        {
            rvMoney.SetLayoutManager(new LinearLayoutManager(Context));
            rvMoney.SetAdapter(adapter);
            tlMoney.TabSelected += (s, e) => DisplayData(data);
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
            catch (Exception)
            {
                Toast.MakeText(Context, Resources.GetString(Resource.String.no_internet_connection), ToastLength.Short).Show();
                return new List<MoneyState>();
            }
        }

        protected override void DisplayData(List<MoneyState> data)
        {
            if (data != null)
            {
                switch (tlMoney.SelectedTabPosition)
                {
                    case 0:
                        adapter.MoneyStates = data;
                        break;
                    case 1:
                        adapter.MoneyStates = data.Where(x => x.IsPaid).ToList();
                        break;
                    case 2:
                        adapter.MoneyStates = data.Where(x => !x.IsPaid).ToList();
                        break;
                }
            }
        }
    }
}
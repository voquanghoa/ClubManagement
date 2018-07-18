﻿using System.Collections.Generic;
using Android.Support.V4.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Controllers;
using ClubManagement.CustomAdapters;
using ClubManagement.Models;

namespace ClubManagement.Fragments
{
    public class ListMoneyFragment : Fragment
    {
        private readonly AppDataController appDataController = AppDataController.Instance;

        [InjectView(Resource.Id.rvMoney)] private RecyclerView rvMoney;

        private MoneyListAdapter adapter;

        private readonly List<MoneyState> moneyStates;

        public ListMoneyFragment(List<MoneyState> moneyStates)
        {
            this.moneyStates = moneyStates;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_money_list, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

        private void Init()
        {
            rvMoney.SetLayoutManager(new LinearLayoutManager(Context));
            adapter = new MoneyListAdapter(moneyStates);
            rvMoney.SetAdapter(adapter);
        }
    }
}
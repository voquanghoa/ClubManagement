using System;
using System.Collections.Generic;
using System.Threading;
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

        private List<MoneyState> moneyStates = new List<MoneyState>();

        public List<MoneyState> MoneyStates
        {
            get => moneyStates;
            set
            {
				moneyStates.Clear();
				moneyStates.AddRange(value);
				adapter?.NotifyDataSetChanged();
            }
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
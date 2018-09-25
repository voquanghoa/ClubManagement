using System;
using System.Collections.Generic;
using Android.Support.V4.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.CustomAdapters;
using ClubManagement.Models;

namespace ClubManagement.Fragments
{
    public class BalanceListFragment : Fragment
    {
        [InjectView(Resource.Id.rvBalance)] private RecyclerView rvBalance;

        private IncomeListAdapter incomeListAdapter;

        public BalanceListFragment()
        {
            incomeListAdapter = new IncomeListAdapter
            {
                IncomeModels = new List<IncomeModel>()
            };
        }

        public List<IncomeModel> IncomeModels
        {
            set
            {
                if (incomeListAdapter == null)
                {
                    incomeListAdapter = new IncomeListAdapter();
                }

                incomeListAdapter.IncomeModels = value;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_balance_recyclerview, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

        private void Init()
        {
            rvBalance.SetLayoutManager(new LinearLayoutManager(Context));
            rvBalance.SetAdapter(incomeListAdapter);
        }
    }
}
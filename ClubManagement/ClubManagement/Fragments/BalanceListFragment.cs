using System;
using System.Collections.Generic;
using Android.Support.V4.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using ClubManagement.CustomAdapters;
using ClubManagement.Models;

namespace ClubManagement.Fragments
{
    public class BalanceListFragment : Fragment
    {
        public const int IncomeType = 1;

        public const int OutcomeType = 2;

        [InjectView(Resource.Id.rvBalance)] private RecyclerView rvBalance;

        private IncomeListAdapter incomeListAdapter;

        private OutcomeListAdapter outcomeListAdapter;

        private readonly int type;

        public BalanceListFragment(int type)
        {
            this.type = type;
            if (this.type == IncomeType)
            {
                incomeListAdapter = new IncomeListAdapter
                {
                    IncomeModels = new List<IncomeModel>()
                };
            }

            if (this.type == OutcomeType)
            {
                outcomeListAdapter = new OutcomeListAdapter
                {
                    OutcomeModels = new List<OutcomeModel>()
                };
                outcomeListAdapter.DeleteClick += (s, e) =>
                {
                    Log.Debug("asdsad", "asdsadasd");
                    if (s is OutcomeModel outcomeModel)
                    {
                        if (outcomeListAdapter.OutcomeModels.Remove(outcomeModel))
                        {
                            OutcomeModels = outcomeListAdapter.OutcomeModels;
                        }
                    }
                };
            }
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

        public List<OutcomeModel> OutcomeModels
        {
            set
            {
                if (outcomeListAdapter == null)
                {
                    outcomeListAdapter = new OutcomeListAdapter();
                }

                outcomeListAdapter.OutcomeModels = value;
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
            if (type == IncomeType)
            {
                rvBalance.SetAdapter(incomeListAdapter);
            }

            if (type == OutcomeType)
            {
                rvBalance.SetAdapter(outcomeListAdapter);
            }
        }
    }
}
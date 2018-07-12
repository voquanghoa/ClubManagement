using Android.OS;
using Android.Support.V7.Widget;
using Android.App;
using Android.Views;
using ClubManagement.Adapters;
using ClubManagement.Models;
using System.Collections.Generic;
using Fragment = Android.Support.V4.App.Fragment;

namespace ClubManagement.Fragments
{
    public class BalancesFragment : Fragment
    {
        [InjectView(Resource.Id.rvBalance)] private RecyclerView rvBalance;

        private BalancesAdapter adapter;

        private readonly List<BalanceModel> balances;

        public BalancesFragment(List<BalanceModel> balances)
        {
            this.balances = balances;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragmentBalances, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

        private void Init()
        {
            rvBalance.SetLayoutManager(new LinearLayoutManager(this.Context));
            adapter = new BalancesAdapter(balances);
            rvBalance.SetAdapter(adapter);
        }
    }
}
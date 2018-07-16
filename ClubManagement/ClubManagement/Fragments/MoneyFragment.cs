using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using ClubManagement.Controllers;
using ClubManagement.Models;
using PagerAdapter = ClubManagement.CustomAdapters.PagerAdapter;

namespace ClubManagement.Fragments
{
    public class MoneyFragment : Fragment
    {
        [InjectView(Resource.Id.tlMoney)] private TabLayout tlMoney;

        [InjectView(Resource.Id.vpMoney)] private ViewPager vpMoney;

        private readonly AppDataController appDataController = AppDataController.Instance;

        private List<MoneyState> listMoneyStates;

        private PagerAdapter adapter;

        private readonly ListMoneyFragment allMoneyFragment = new ListMoneyFragment();

        private readonly ListMoneyFragment paidMoneyFragment = new ListMoneyFragment();

        private readonly ListMoneyFragment unpaidMoneyFragment = new ListMoneyFragment();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_money, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

        private void Init()
        {
            adapter = new PagerAdapter(Activity.SupportFragmentManager);
            SetData();
            adapter.AddFramgent(allMoneyFragment, "All");
            adapter.AddFramgent(paidMoneyFragment, "Already paid");
            adapter.AddFramgent(unpaidMoneyFragment, "Unpaid");
            vpMoney.Adapter = adapter;
            tlMoney.SetupWithViewPager(vpMoney);
        }

        public override void OnResume()
        {
            base.OnResume();
            SetData();
        }

        private void SetData()
        {
            listMoneyStates = appDataController.GetListMoneyState();
            allMoneyFragment.MoneyStates = listMoneyStates;
            paidMoneyFragment.MoneyStates = listMoneyStates.Where(x => x.IsPaid).ToList();
            unpaidMoneyFragment.MoneyStates = listMoneyStates.Where(x => !x.IsPaid).ToList();
        }
    }
}
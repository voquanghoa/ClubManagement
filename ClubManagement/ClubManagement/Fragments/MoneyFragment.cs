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
            listMoneyStates = appDataController.GetListMoneyState();
            adapter.AddFramgent(new ListMoneyFragment(listMoneyStates), "All");
            adapter.AddFramgent(new ListMoneyFragment(listMoneyStates.Where(x => x.IsPaid).ToList()), "Already paid");
            adapter.AddFramgent(new ListMoneyFragment(listMoneyStates.Where(x => !x.IsPaid).ToList()), "Unpaid");
            vpMoney.Adapter = adapter;
            tlMoney.SetupWithViewPager(vpMoney);
        }
    }
}
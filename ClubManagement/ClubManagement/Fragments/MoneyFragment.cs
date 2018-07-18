using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Controllers;
using ClubManagement.CustomAdapters;
using ClubManagement.Models;
using ClubManagement.Ultilities;

namespace ClubManagement.Fragments
{
    public class MoneyFragment : Fragment
    {
        [InjectView(Resource.Id.tlMoney)] private TabLayout tlMoney;

        [InjectView(Resource.Id.rvMoney)] private RecyclerView rvMoney;

        private readonly MoneyListAdapter adapter = new MoneyListAdapter();

        private readonly AppDataController appDataController = AppDataController.Instance;

        private List<MoneyState> listMoneyStates;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_money, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

        private void Init()
        {
            listMoneyStates = appDataController.GetListMoneyState();
            adapter.MoneyStates = listMoneyStates;
            rvMoney.SetLayoutManager(new LinearLayoutManager(Context));
            rvMoney.SetAdapter(adapter);
            tlMoney.AddTab(tlMoney.NewTab().SetText(AppConstantValues.MoneyFragmentAllTabTitle));
            tlMoney.AddTab(tlMoney.NewTab().SetText(AppConstantValues.MoneyFragmentPaidTabTitle));
            tlMoney.AddTab(tlMoney.NewTab().SetText(AppConstantValues.MoneyFragmentUnpaidTabTitle));
            tlMoney.TabSelected += (s, e) =>
            {
                switch (e.Tab.Text)
                {
                    case AppConstantValues.MoneyFragmentAllTabTitle:
                        adapter.MoneyStates = listMoneyStates;
                        break;
                    case AppConstantValues.MoneyFragmentPaidTabTitle:
                        adapter.MoneyStates = listMoneyStates.Where(x => x.IsPaid).ToList();
                        break;
                    case AppConstantValues.MoneyFragmentUnpaidTabTitle:
                        adapter.MoneyStates = listMoneyStates.Where(x => !x.IsPaid).ToList();
                        break;
                }
            };
        }

        public override void OnResume()
        {
            base.OnResume();
            listMoneyStates = appDataController.GetListMoneyState();
            adapter.MoneyStates = listMoneyStates;
        }
    }
}
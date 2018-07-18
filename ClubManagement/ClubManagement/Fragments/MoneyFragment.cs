using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
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

        [InjectOnClick(Resource.Id.btnLogout)]
        private void Logout(object s, EventArgs e)
        {
            DialogExtensions.ShowLogoutDialog(Context);
        }

        private const string AllTabTitle = "All";

        private const string PaidTabTitle = "Already paid";

        private const string UnpaidTabTitle = "Unpaid";

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
            tlMoney.AddTab(tlMoney.NewTab().SetText(AllTabTitle));
            tlMoney.AddTab(tlMoney.NewTab().SetText(PaidTabTitle));
            tlMoney.AddTab(tlMoney.NewTab().SetText(UnpaidTabTitle));
            tlMoney.TabSelected += (s, e) =>
            {
                switch (e.Tab.Text)
                {
                    case AllTabTitle:
                        adapter.MoneyStates = listMoneyStates;
                        break;
                    case PaidTabTitle:
                        adapter.MoneyStates = listMoneyStates.Where(x => x.IsPaid).ToList();
                        break;
                    case UnpaidTabTitle:
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
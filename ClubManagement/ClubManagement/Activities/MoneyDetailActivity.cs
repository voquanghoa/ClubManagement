using System;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.CustomAdapters;
using ClubManagement.Models;
using System.Collections.Generic;

namespace ClubManagement.Activities
{
    [Activity(Label = "MoneyDetailActivity", Theme = "@style/AppTheme")]
    public class MoneyDetailActivity : Activity
    {
        private readonly AppDataController appDataController = AppDataController.Instance;

        private MoneyAdminListAdapter adapter;

        private List<MoneyAdminState> moneyAdminStates;

        private string moneyId;

        [InjectView(Resource.Id.tvDescription)]
        private TextView tvDescription;

        [InjectView(Resource.Id.tvPayState)] private TextView tvPayState;

        [InjectView(Resource.Id.rvUser)] private RecyclerView rvUser;

        [InjectOnClick(Resource.Id.btnBack)]
        private void Back(object s, EventArgs e)
        {
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_money_detail);
            Cheeseknife.Inject(this);
            Init();
        }

        private void Init()
        {
            var description = Intent.GetStringExtra("Description");
            var budget = Intent.GetIntExtra("Budget", 0);
            moneyId = Intent.GetStringExtra("MoneyId");
            var time = Intent.GetStringExtra("Time");

            tvDescription.Text = $"{time} - Budget : {budget}$\n\n{description}";
            rvUser.SetLayoutManager(new LinearLayoutManager(this));
            moneyAdminStates = appDataController.GetMoneyAdminStates(moneyId);
            adapter = new MoneyAdminListAdapter(moneyAdminStates, tvPayState, moneyId);
            rvUser.SetAdapter(adapter);
        }
    }
}
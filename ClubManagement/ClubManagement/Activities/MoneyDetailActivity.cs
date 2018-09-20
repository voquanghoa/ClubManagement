using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.CustomAdapters;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using Newtonsoft.Json;

namespace ClubManagement.Activities
{
    [Activity(Label = "MoneyDetailActivity", Theme = "@style/AppTheme")]
    public class MoneyDetailActivity : Activity
    {
        private string moneyId;

        [InjectView(Resource.Id.tvDescription)]
        private TextView tvDescription;

        [InjectView(Resource.Id.tvDeadlineTime)]
        private TextView tvDeadlineTime;

        [InjectView(Resource.Id.tvFee)] private TextView tvFee;

        [InjectView(Resource.Id.rvPaid)] private RecyclerView rvPaid;

        [InjectView(Resource.Id.rvUnpaid)] private RecyclerView rvUnpaid;

        [InjectView(Resource.Id.tvMembersUnpaid)]
        private TextView tvMembersUnpaid;

        [InjectView(Resource.Id.tvMembersPaid)]
        private TextView tvMembersPaid;

        [InjectView(Resource.Id.refresher)] private SwipeRefreshLayout refreshLayout;

        [InjectOnClick(Resource.Id.btnBack)]
        private void Back(object s, EventArgs e)
        {
            Finish();
        }

        private MoneyAdminListAdapter paidAdapter;

        private MoneyAdminListAdapter unpaidAdapter;

        private List<MoneyAdminState> moneyAdminStates = new List<MoneyAdminState>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_money_detail);
            Cheeseknife.Inject(this);
            Init();
        }

        private void Init()
        {
            var moneyModel = JsonConvert.DeserializeObject<MoneyModel>(Intent.GetStringExtra("MoneyModel"));
            moneyId = moneyModel.Id;

            tvDescription.Text = moneyModel.Description;
            tvDeadlineTime.Text = $"Deadline: {moneyModel.Time:MMM dd, yyyy}";
            tvFee.Text = moneyModel.Amount.ToCurrency();
            rvUnpaid.SetLayoutManager(new LinearLayoutManager(this));
            rvPaid.SetLayoutManager(new LinearLayoutManager(this));

            
            this.DoRequest(() =>
            {
                refreshLayout.Refreshing = true;
                moneyAdminStates = AppDataController.Instance.GetMoneyAdminStates(moneyId);
            }, () =>
            {
                paidAdapter = new MoneyAdminListAdapter(moneyId)
                {
                    MoneyAdminStates = moneyAdminStates.Where(x => x.IsPaid).ToList()
                };

                unpaidAdapter = new MoneyAdminListAdapter(moneyId)
                {
                    MoneyAdminStates = moneyAdminStates.Where(x => !x.IsPaid).ToList()
                };


                unpaidAdapter.ItemPayClick += (s, e) => PayEvent(s);

                paidAdapter.ItemPayClick += (s, e) => PayEvent(s);

                SetText();
                rvUnpaid.SetAdapter(unpaidAdapter);
                rvPaid.SetAdapter(paidAdapter);
                refreshLayout.Refreshing = false;
                refreshLayout.Enabled = false;
            });      
        }

        private void SetText()
        {
            tvMembersPaid.Text = $"{moneyAdminStates.Count - unpaidAdapter.MoneyAdminStates.Count} / {moneyAdminStates.Count} members paid";
            tvMembersUnpaid.Text = $"{moneyAdminStates.Count - paidAdapter.MoneyAdminStates.Count} / {moneyAdminStates.Count} members unpaid";
        }

        private void PayEvent(object s)
        {
            if (!(s is MoneyAdminState moneyAdminState)) return;
            if (moneyAdminState.IsPaid)
            {
                unpaidAdapter.MoneyAdminStates.Remove(moneyAdminState);
                paidAdapter.MoneyAdminStates.Insert(0, moneyAdminState);
            }
            else
            {
                paidAdapter.MoneyAdminStates.Remove(moneyAdminState);
                unpaidAdapter.MoneyAdminStates.Insert(0, moneyAdminState);
            }

            RunOnUiThread(() =>
            {
                unpaidAdapter.NotifyDataSetChanged();
                paidAdapter.NotifyDataSetChanged();
                SetText();
            });
        }
    }
}
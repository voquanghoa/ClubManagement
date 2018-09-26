using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using ClubManagement.CustomAdapters;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using Newtonsoft.Json;

namespace ClubManagement.Activities
{
    [Activity(Label = "OutcomeDetailActivity")]
    public class OutcomeDetailActivity : Activity
    {
        private OutcomeAmountListAdapter adapter;

        private List<OutcomeAmountItem> items;

        [InjectOnClick(Resource.Id.btnBack)]
        private void Back(object s, EventArgs e)
        {
            Finish();
        }

        [InjectView(Resource.Id.rvItems)] private RecyclerView rvItems;

        [InjectView(Resource.Id.tvTitle)] private TextView tvTitle;

        [InjectView(Resource.Id.tvTime)] private TextView tvTime;

        [InjectView(Resource.Id.tvTotal)] private TextView tvTotal;

        [InjectView(Resource.Id.imgGroup)] private ImageView imgGroup;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_outcome_detail);
            Cheeseknife.Inject(this);
            Init();
        }

        private void Init()
        {
            var outcome = JsonConvert.DeserializeObject<OutcomeModel>(Intent.GetStringExtra("outcome"));
            tvTitle.Text = outcome.Title;
            tvTime.Text = outcome.Date.ToString("MMM dd, yyyy");
            items = outcome.Items;
            adapter = new OutcomeAmountListAdapter {OutcomeAmountItems = items};
            rvItems.SetLayoutManager(new LinearLayoutManager(this));
            rvItems.SetAdapter(adapter);
            tvTotal.Text = $"Total: {items.Sum(x => x.Amount).ToCurrency()}";
        }
    }
}
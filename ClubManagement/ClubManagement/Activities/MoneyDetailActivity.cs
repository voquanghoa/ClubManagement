using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.CustomAdapters;

namespace ClubManagement.Activities
{
    [Activity(Label = "MoneyDetailActivity", Theme = "@style/AppTheme")]
    public class MoneyDetailActivity : Activity
    {
        private readonly AppDataController appDataController = AppDataController.Instance;

        private MoneyAdminListAdapter adapter;

        [InjectView(Resource.Id.tvDescription)]
        private TextView tvDescription;

        [InjectView(Resource.Id.tvPayState)] private TextView tvPayState;

        [InjectView(Resource.Id.rvUser)] private RecyclerView rvUser;

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
            var moneyId = Intent.GetStringExtra("MoneyId");
            var time = Intent.GetStringExtra("Time");

            tvDescription.Text = $"{time} - Budget : {budget}$\n\n{description}";
            tvPayState.Visibility = ViewStates.Invisible;

            rvUser.SetLayoutManager(new LinearLayoutManager(this));
            adapter = new MoneyAdminListAdapter(appDataController.GetMoneyAdminStates(moneyId));
            rvUser.SetAdapter(adapter);
        }
    }
}
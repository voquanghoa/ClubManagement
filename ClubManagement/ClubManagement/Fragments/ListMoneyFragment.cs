using Android.Support.V4.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Controllers;
using ClubManagement.CustomAdapters;

namespace ClubManagement.Fragments
{
    public class ListMoneyFragment : Fragment
    {
        private readonly AppDataController appDataController = AppDataController.Instance;

        [InjectView(Resource.Id.rvMoney)] private RecyclerView rvMoney;

        private MoneyListAdapter adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_money_list, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

        private void Init()
        {
            rvMoney.SetLayoutManager(new LinearLayoutManager(Context));
            adapter = new MoneyListAdapter(appDataController.GetListMoneyState());
            rvMoney.SetAdapter(adapter);
        }
    }
}
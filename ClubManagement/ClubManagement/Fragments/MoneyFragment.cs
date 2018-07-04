using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;

namespace ClubManagement.Fragments
{
    public class MoneyFragment : Fragment
    {
        [InjectView(Resource.Id.tlMoney)] private TabLayout tlMoney;

        [InjectView(Resource.Id.vpMoney)] private ViewPager vpMoney;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_money, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

        private void Init()
        {
            var adapter = new PagerAdapter(Activity.SupportFragmentManager);
            adapter.AddFramgent(new ListMoneyFragment(), "All");
            adapter.AddFramgent(new ListMoneyFragment(), "Already paid");
            adapter.AddFramgent(new ListMoneyFragment(), "Unpaid");
            vpMoney.Adapter = adapter;
            tlMoney.SetupWithViewPager(vpMoney);
        }
    }
}
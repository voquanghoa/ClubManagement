using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using System.Linq;
using System.Threading.Tasks;
using Android.App;

namespace ClubManagement.Fragments
{
    public class BalanceSummaryFragment : Android.Support.V4.App.Fragment
    {
        [InjectView(Resource.Id.tvFinalBalance)]
        private TextView tvFinalBalance;

        [InjectView(Resource.Id.tvIncome)]
        private TextView tvIncome;

        [InjectView(Resource.Id.tvOutcome)]
        private TextView tvOutcome;

        public int SumIncomes;

        public int SumOutcomes;

        private int finalBalance;

        private string numberSign;

        private object locker = new object();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragmentBalanceSummary, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

        private void Init()
        {
            finalBalance = SumIncomes - SumOutcomes;

            numberSign = finalBalance > 0 ? "+" : "";

            tvFinalBalance.Text = $"Final balance: {numberSign}{finalBalance * 1000} đ";
            tvIncome.Text = $"Income total: +{SumIncomes * 1000} đ";
            tvOutcome.Text = $"Outcome total: -{SumOutcomes * 1000} đ";
        }
    }
}
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using System.Linq;

namespace ClubManagement.Fragments
{
    public class BalanceSummaryFragment : Fragment
    {
        [InjectView(Resource.Id.tvFinalBalance)]
        private TextView tvFinalBalance;

        [InjectView(Resource.Id.tvIncome)]
        private TextView tvIncome;

        [InjectView(Resource.Id.tvOutcome)]
        private TextView tvOutcome;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragmentBalanceSummary, container, false);

            Cheeseknife.Inject(this, view);

            Init();

            return view;
        }

        private void Init()
        {
            var sumIncomes = AppDataController.Instance.Incomes.Sum(x => x.Amount);

            var sumOutcomes = OutComesController.Instance.Values.Sum(x => x.Amount);

            var finalBalance = sumIncomes - sumOutcomes;

            var numberSign = finalBalance > 0 ? "+" : "";

            tvFinalBalance.Text = $"Final balance: {numberSign}{finalBalance}$";
            tvIncome.Text = $"Income total: +{sumIncomes}$";
            tvOutcome.Text = $"Outcome total: -{sumOutcomes}$";
        }
    }
}
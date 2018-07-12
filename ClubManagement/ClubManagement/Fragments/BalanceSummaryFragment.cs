using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;

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

            return view;
        }
    }
}
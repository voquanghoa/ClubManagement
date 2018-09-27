using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;
using MikePhil.Charting.Util;
using System.Collections.Generic;
using System.Linq;

namespace ClubManagement.Fragments
{
    public class BalanceSummaryFragment : Android.Support.V4.App.Fragment
    {
        [InjectView(Resource.Id.tvStatus)]
        private TextView tvStatus;

        [InjectView(Resource.Id.tvFinalBalance)]
        private TextView tvFinalBalance;

        [InjectView(Resource.Id.tvIncome)]
        private TextView tvIncome;

        [InjectView(Resource.Id.tvOutcome)]
        private TextView tvOutcome;

        [InjectView(Resource.Id.barChart)]
        private BarChart barChart;

        public long SumIncomes;

        public long SumOutcomes;

        private long finalBalance;

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

            if (finalBalance > 0)
            {
                tvStatus.Text = GetString(Resource.String.we_still_have_money);
                tvFinalBalance.SetTextColor(Context.GetColorStateList(Resource.Color.text_color_blue));
                tvFinalBalance.Text = $"+{finalBalance.ToCurrency()}";
            }
            else if (finalBalance < 0)
            {
                tvStatus.Text = GetString(Resource.String.We_are_broke_now);
                tvFinalBalance.SetTextColor(Context.GetColorStateList(Resource.Color.text_color_red));
                tvFinalBalance.Text = $"-{finalBalance.ToCurrency()}";
            }
            else
            {
                tvStatus.Text = GetString(Resource.String.We_are_broke_now);
                tvFinalBalance.SetTextColor(Context.GetColorStateList(Resource.Color.text_color_red));
                tvFinalBalance.Text = finalBalance.ToCurrency();
            }
            
            tvIncome.Text = $"+{SumIncomes.ToCurrency()}";
            tvOutcome.Text = $"-{SumOutcomes.ToCurrency()}";

            barChart.Description.Enabled = false;
            barChart.AxisRight.Enabled = false;
            barChart.AxisLeft.Enabled = false;
            barChart.Legend.Enabled = false;
            barChart.XAxis.Position = XAxis.XAxisPosition.Bottom;
            barChart.XAxis.SetDrawGridLines(false);
            barChart.XAxis.Granularity = 1;
            barChart.SetExtraOffsets(0, 0, 0, 10);

            var labels = new List<string>()
            {
                GetString(Resource.String.income),
                GetString(Resource.String.outcome)
            };

            var barGroup = new List<BarEntry>()
            {
                new BarEntry(0, SumIncomes / 1000f),
                new BarEntry(1, SumOutcomes / 1000f)
            };

            var barDataSet = new BarDataSet(barGroup, null);
            barDataSet.SetColors(new int[] { Resource.Color.income_color, Resource.Color.outcome_color }, Context);
            barDataSet.SetDrawValues(false);

            barChart.XAxis.ValueFormatter = new IndexAxisValueFormatter(labels);
            barChart.XAxis.TextColor = ContextCompat.GetColor(Context, Resource.Color.text_color_blue);
            barChart.XAxis.TextSize = Resources.GetDimension(Resource.Dimension.text_size_normal);
            barChart.Data = new BarData(barDataSet); ;
            barChart.Invalidate();
        }
    }
}
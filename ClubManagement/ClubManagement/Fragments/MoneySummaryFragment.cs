using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;
using MikePhil.Charting.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ClubManagement.Fragments
{
    public class MoneySummaryFragment : Fragment
    {
        [InjectView(Resource.Id.tvNeedPay)]
        private TextView tvNeedPay;

        [InjectView(Resource.Id.tvPaid)]
        private TextView tvPaid;

        [InjectView(Resource.Id.tvStatusNeedPay)]
        private TextView tvStatusNeedPay;

        [InjectView(Resource.Id.spinner)]
        private Spinner spinner;

        [InjectView(Resource.Id.barChart)]
        private BarChart barChart;

        private List<string> years;

        private List<MoneyState> moneyStates;

        private ArrayAdapter adapter;

        private const string Total = "Total";

        private const string Unit = "Unit: 1,000VND";

        private DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();

        public List<MoneyState> MoneyStates
        {
            set
            {
                moneyStates = value;

                var startYear = AppDataController.Instance.User.CreatedTime.Year;

                years = Enumerable.Range(startYear, DateTime.Now.Year - startYear + 1)
                    .Select(x => x.ToString())
                    .Reverse()
                    .ToList();

                years.Add(Total);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.FragmentMoneySummary, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Cheeseknife.Inject(this, view);

            spinner.ItemSelected += Spinner_ItemSelected;

            if (moneyStates != null) InitMoneyView();
        }

        private void InitMoneyView()
        {
            adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleSpinnerItem, years);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            var sumMoneyNeedPay = moneyStates.Sum(x => !x.IsPaid ? x.MoneyModel.Amount : 0);

            if (sumMoneyNeedPay == 0)
            {
                tvStatusNeedPay.Text = GetString(Resource.String.paid_all);
                tvNeedPay.Visibility = ViewStates.Gone;
            }
            else
            {
                tvNeedPay.Text = sumMoneyNeedPay.ToCurrency();
            }

            barChart.Description.Text = Unit;
            barChart.XAxis.Position = XAxis.XAxisPosition.Bottom;
            barChart.AxisRight.Enabled = false;
            barChart.Legend.Enabled = false;
            barChart.XAxis.SetDrawGridLines(false);
            barChart.AxisLeft.SetDrawGridLines(false);
            barChart.AxisLeft.AxisMinimum = 0;
            barChart.AxisLeft.Granularity = 1;

            UpdatePaidView(DateTime.Now.Year.ToString());
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            UpdatePaidView(years[e.Position].ToString());
        }

        private void UpdatePaidView(string year)
        {
            var sumMoneyPaidByYear = moneyStates.Sum(x => x.IsPaid
                    && (x.MoneyModel.Time.Year.ToString().Equals(year)
                        || year.Equals(Total))
                ? x.MoneyModel.Amount
                : 0);

            tvPaid.Text = sumMoneyPaidByYear.ToCurrency();

            if (sumMoneyPaidByYear == 0)
            {
                barChart.Visibility = ViewStates.Invisible;
            }
            else
            {
                barChart.Visibility = ViewStates.Visible;

                var labels = new List<string>();
                var barGroup = new List<BarEntry>();

                if (year.Equals(Total))
                {
                    var moneyYears = moneyStates
                        .GroupBy(x => x.MoneyModel.Time.Year)
                        .OrderBy(x => x.Key)
                        .Select(x => new
                        {
                            lable = x.Key.ToString(),
                            value = x.Sum(y => y.IsPaid ? y.MoneyModel.Amount : 0)
                        });

                    labels = moneyYears.Select(x => x.lable).ToList();

                    barGroup = moneyYears
                        .Select((x, index) => new BarEntry(index, x.value / 1000f))
                        .ToList();
                }
                else
                {
                    var moneyMonths = moneyStates.Where(x => x.MoneyModel.Time.Year.ToString().Equals(year))
                        .GroupBy(x => x.MoneyModel.Time.Month)
                        .OrderBy(x => x.Key)
                        .Select(x => new
                        {
                            lable = dateTimeFormatInfo.GetAbbreviatedMonthName(x.Key),
                            value = x.Sum(y => y.IsPaid ? y.MoneyModel.Amount : 0)
                        }).ToList();

                    labels = moneyMonths.Select(x => x.lable).ToList();

                    barGroup = moneyMonths
                        .Select((x, index) => new BarEntry(index, x.value / 1000f))
                        .ToList();
                }

                var barDataSet = new BarDataSet(barGroup, null);
                barDataSet.SetColors(ColorTemplate.ColorfulColors.ToArray());
                barDataSet.ValueFormatter = new ValueFormatter();

                barChart.XAxis.ValueFormatter = new IndexAxisValueFormatter(labels);
                barChart.Data = new BarData(barDataSet); ;
                barChart.Invalidate();
            }
        }
    }
}
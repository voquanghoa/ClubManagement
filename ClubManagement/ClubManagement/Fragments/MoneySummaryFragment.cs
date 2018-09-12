using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using System;
using System.Collections.Generic;
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

        private List<string> years;

        private List<MoneyState> moneyStates;

        private ArrayAdapter adapter;

        private const string Total = "Total";

        public List<MoneyState> MoneyStates
        {
            set
            {
                moneyStates = value;

                var startYear = 2010;

                years = Enumerable.Range(startYear, DateTime.Now.Year - startYear + 1)
                    .Select(x=>x.ToString())
                    .Reverse()
                    .ToList();

                years.Add(Total);

                adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleSpinnerItem, years);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spinner.Adapter = adapter;

                UpdateMoneyView();
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

            if (moneyStates != null) UpdateMoneyView();
        }

        private void UpdateMoneyView()
        {
            spinner.Adapter = adapter;

            tvPaid.Text = moneyStates.Sum(x => x.IsPaid && x.MoneyModel.Time.Year == DateTime.Now.Year
                ? x.MoneyModel.Amount
                : 0).ToCurrency();

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
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            tvPaid.Text = moneyStates.Sum(x => x.IsPaid
                    && (x.MoneyModel.Time.Year.ToString().Equals(years[e.Position])
                        || years[e.Position].Equals(Total))
                ? x.MoneyModel.Amount
                : 0).ToCurrency();
        }
    }
}
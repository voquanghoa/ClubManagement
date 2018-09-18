using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V4.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.CustomAdapters;
using ClubManagement.Models;

namespace ClubManagement.Fragments
{
    public class MoneyListFragment : Fragment
    {
        [InjectView(Resource.Id.rvMoney)] private RecyclerView rvMoney;

        [InjectView(Resource.Id.tvNoFee)] private TextView tvNoFee;

        private MoneyListAdapter adapter;

        public event EventHandler AdapterItemDeleteClick;

        public MoneyListFragment()
        {
            adapter = new MoneyListAdapter();
            adapter.ItemDeleteClick += (s, e) =>
            {
                AdapterItemDeleteClick?.Invoke(s, e);
            };
        }

        public List<MoneyState> MoneyStates 
        {
            set
            {
                if (adapter == null)
                {
                    adapter = new MoneyListAdapter();
                }

                if (tvNoFee != null)
                {
                    tvNoFee.Visibility = !value.Any() ? ViewStates.Visible : ViewStates.Gone;
                }
                adapter.MoneyStates = value;
            }
        } 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = LayoutInflater.Inflate(Resource.Layout.recyclerview_money, container, false);

            Cheeseknife.Inject(this, view);

            Init();

            return view;
        }

        private void Init()
        {
            rvMoney.SetLayoutManager(new LinearLayoutManager(Context));
            rvMoney.SetAdapter(adapter);
        }
    }
}
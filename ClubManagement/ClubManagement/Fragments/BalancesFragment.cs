using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Adapters;
using ClubManagement.Models;
using System.Collections.Generic;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Widget;
using ClubManagement.Controllers;
using System.Linq;

namespace ClubManagement.Fragments
{
    public class BalancesFragment : Fragment
    {
        [InjectView(Resource.Id.rvBalance)] private RecyclerView rvBalance;

        private BalancesAdapter adapter;

        public List<OutcomeModel> Incomes = new List<OutcomeModel>();

        public List<OutcomeModel> Outcomes = new List<OutcomeModel>();

        public enum Type { Income, Outcome};

        private Type type;

        private OutcomeDialogFragment outcomeDialogFragment = new OutcomeDialogFragment();

        public BalancesFragment(Type type)
        {
            this.type = type;

            adapter = new BalancesAdapter(type);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragmentBalances, container, false);
            Cheeseknife.Inject(this, view);
            Init(view);
            return view;
        }
        
        private void Init(View view)
        {
            rvBalance.SetLayoutManager(new LinearLayoutManager(this.Context));
            rvBalance.SetAdapter(adapter);

            if (type == Type.Income)
            {
                view.FindViewById<ImageButton>(Resource.Id.imgbtnAdd).Visibility = ViewStates.Gone;

                adapter.Balances = Incomes;
            }
            else
            {
                view.FindViewById<ImageButton>(Resource.Id.imgbtnAdd).Click += AddOutcome_Click;

                adapter.Balances = Outcomes;

                outcomeDialogFragment.SaveClick += (s, ne) =>
                {
                    Outcomes.Add((OutcomeModel)s);

                    adapter.Balances = Outcomes;
                };
            }
        }

        private void AddOutcome_Click(object sender, System.EventArgs e)
        {
            outcomeDialogFragment.Show(FragmentManager, null);
        }
    }
}
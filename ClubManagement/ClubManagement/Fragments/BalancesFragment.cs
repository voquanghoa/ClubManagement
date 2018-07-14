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

        private  List<OutcomeModel> balances;

        public enum Type { Income, Outcome};

        private Type type;

        private OutcomeDialogFragment outcomeDialogFragment = new OutcomeDialogFragment();

        public BalancesFragment(Type type)
        {
            this.type = type;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragmentBalances, container, false);
            Cheeseknife.Inject(this, view);

            if (type == Type.Income)
            {
                balances = AppDataController.Instance.Incomes.Select(x => (OutcomeModel)x).ToList();

                view.FindViewById<ImageButton>(Resource.Id.imgbtnAdd).Visibility = ViewStates.Gone;
            }
            else
            {
                balances = OutComesController.Instance.Values;

                view.FindViewById<ImageButton>(Resource.Id.imgbtnAdd).Click += AddOutcome_Click;

                outcomeDialogFragment.SaveClick += (s, ne) =>
                {
                    balances.Add((OutcomeModel)s);

                    adapter.Balances = balances;
                };
            }

            Init();
            return view;
        }

        private void AddOutcome_Click(object sender, System.EventArgs e)
        {
            outcomeDialogFragment.Show(FragmentManager, null);
        }

        private void Init()
        {
            rvBalance.SetLayoutManager(new LinearLayoutManager(this.Context));
            adapter = new BalancesAdapter(balances, type);
            rvBalance.SetAdapter(adapter);
        }
    }
}
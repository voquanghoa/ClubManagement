using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using ClubManagement.Adapters;
using ClubManagement.Models;
using System.Collections.Generic;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Ultilities;
using Android.Support.V7.Widget.Helper;

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

        private ItemTouchHelper itemTouchHelper;

        public BalancesFragment(Type type)
        {
            this.type = type;

            adapter = new BalancesAdapter(type);

            var swipeToDeleteCallback = new SwipeLeftToDeleteCallback(ItemTouchHelper.ActionStateIdle, ItemTouchHelper.Left);
            swipeToDeleteCallback.SwipeLeft += SwipeToDeleteCallback_SwipeLeft;

            itemTouchHelper = new ItemTouchHelper(swipeToDeleteCallback);
        }

        private void SwipeToDeleteCallback_SwipeLeft(object sender, ClickEventArgs e)
        {
            if (sender is BalanceAdapterViewHolder eventViewHolder)
            {
                Context.ShowConfirmDialog(Resource.String.delete_outcome, Resource.String.confirm_delete,
                    () =>
                    {
                        OutComesController.Instance.Delete(Outcomes[e.Position]);
                        Outcomes.RemoveAt(e.Position);
                        adapter.NotifyItemRemoved(e.Position);
                    }, () =>
                    {
                        adapter.NotifyDataSetChanged();
                    }).Show();
            }
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
                Context.DoWithAdmin(() =>
                {
                    itemTouchHelper.AttachToRecyclerView(rvBalance);
                }, () =>
                {
                    view.FindViewById<ImageButton>(Resource.Id.imgbtnAdd).Visibility = ViewStates.Gone;
                });

                adapter.Balances = Outcomes;
            }
        }
    }
}
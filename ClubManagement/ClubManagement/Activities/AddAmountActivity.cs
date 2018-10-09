using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.CustomAdapters;
using ClubManagement.Fragments;
using ClubManagement.Models;

namespace ClubManagement.Activities
{
    [Activity(Label = "AddAmountActivity")]
    public class AddAmountActivity : Activity
    {
        private AmountItemListAdapter adapter;

        [InjectView(Resource.Id.tvTotal)] private TextView tvTotal;

        [InjectView(Resource.Id.confirm_delete_view)]
        private LinearLayout linearLayout;

        [InjectView(Resource.Id.rvAmountItems)]
        private RecyclerView rvItems;

        [InjectOnClick(Resource.Id.btnYes)]
        private void ConfirmDelete(object s, EventArgs e)
        {
            adapter.DeleteChoosedItem();
            linearLayout.Visibility = ViewStates.Gone;
            adapter.IsDeleting = false;
        }

        [InjectOnClick(Resource.Id.btnNo)]
        private void Cancel(object s, EventArgs e)
        {
            linearLayout.Visibility = ViewStates.Gone;
            adapter.IsDeleting = false;
        }

        [InjectOnClick(Resource.Id.tvAddItem)]
        private void AddItem(object s, EventArgs e)
        {
            var dialog = new AddAmountDialog(this);
            adapter.IsDeleting = false;
            dialog.DoneClick += (ss, ee) => { adapter.AddItem(dialog.AmountItem); };
            dialog.Show();
        }


        [InjectOnClick(Resource.Id.btnBack)]
        private void Back(object s, EventArgs e)
        {
        }

        [InjectOnClick(Resource.Id.btnDelete)]
        private void Delete(object s, EventArgs e)
        {
            adapter.IsDeleting = true;
            linearLayout.Visibility = ViewStates.Visible;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_add_amount);
            Cheeseknife.Inject(this);
            Init();
        }

        private void Init()
        {
            linearLayout.Visibility = ViewStates.Gone;
            adapter = new AmountItemListAdapter
            {
                Items = new List<AmountItem>
                {
                    new AmountItem
                    {
                        Item = new OutcomeAmountItem
                        {
                            Name = "1",
                            Amount = 1000
                        }
                    },
                    new AmountItem
                    {
                        Item = new OutcomeAmountItem
                        {
                            Name = "2",
                            Amount = 1000
                        }
                    },
                    new AmountItem
                    {
                        Item = new OutcomeAmountItem
                        {
                            Name = "3",
                            Amount = 1000
                        }
                    },
                    new AmountItem
                    {
                        Item = new OutcomeAmountItem
                        {
                            Name = "4",
                            Amount = 1000
                        },
                        IsChooseToDelete = false
                    },
                    new AmountItem
                    {
                        Item = new OutcomeAmountItem
                        {
                            Name = "5",
                            Amount = 1000
                        }
                    }
                }
            };

            rvItems.SetLayoutManager(new LinearLayoutManager(this));
            rvItems.SetAdapter(adapter);
        }
    }
}
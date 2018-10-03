using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using ClubManagement.CustomAdapters;
using ClubManagement.Models;
using ClubManagement.Ultilities;

namespace ClubManagement.Activities
{
    [Activity(Label = "AddAmountActivity")]
    public class AddAmountActivity : Activity
    {
        private AmountItemListAdapter adapter;

        [InjectView(Resource.Id.tvTotal)] private TextView tvTotal;

        [InjectView(Resource.Id.rvAmountItems)]
        private RecyclerView rvItems;

        [InjectOnClick(Resource.Id.tvAddItem)]
        private void AddItem(object s, EventArgs e)
        {
            this.ShowMessage("asbdkasdksakjd");
            adapter.AddItem(new AmountItem
            {
                Item = new OutcomeAmountItem
                {
                    Name = "2",
                    Amount = 123123
                },
                IsChooseToDelete = false
            });
        }

        [InjectOnClick(Resource.Id.btnBack)]
        private void Back(object s, EventArgs e)
        {
            //Finish();
            adapter.DeleteChoosedItem();
        }

        [InjectOnClick(Resource.Id.btnDelete)]
        private void Delete(object s, EventArgs e)
        {
            adapter.IsDeleting = true;
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
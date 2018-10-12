using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.CustomAdapters;
using ClubManagement.Fragments;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using Newtonsoft.Json;

namespace ClubManagement.Activities
{
    [Activity(Label = "AddAmountActivity")]
    public class AddAmountActivity : Activity
    {
        private AmountItemListAdapter adapter;

        private List<AmountItem> items = new List<AmountItem>();

        [InjectView(Resource.Id.tvTotal)] private TextView tvTotal;

        [InjectView(Resource.Id.confirm_delete_view)]
        private LinearLayout linearLayout;

        [InjectView(Resource.Id.rvAmountItems)]
        private RecyclerView rvItems;

        [InjectOnClick(Resource.Id.btnYes)]
        private void ConfirmDelete(object s, EventArgs e)
        {
            adapter.DeleteChoosedItem();
            UpdateTotalView();
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
            var dialog = new AddAmountDialog(this, AddAmountDialog.TypeAdd, new OutcomeAmountItem());
            adapter.IsDeleting = false;
            dialog.DoneClick += (ss, ee) =>
            {
                adapter.AddItem(dialog.AmountItem); 
                UpdateTotalView();
            };
            dialog.Show();
        }


        [InjectOnClick(Resource.Id.btnBack)]
        private void Back(object s, EventArgs e)
        {
            var resultIntent = new Intent();
            resultIntent.PutExtra("items", JsonConvert.SerializeObject(adapter.Items.Select(x => x.Item).ToList()));
            SetResult(Result.Ok, resultIntent);
            Finish();
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
            if (Intent.HasExtra("items"))
            {
                items = JsonConvert.DeserializeObject<List<OutcomeAmountItem>>(Intent.GetStringExtra("items"))
                    .Select(
                        x => new AmountItem
                        {
                            Item = x
                        })
                    .ToList();
            }

            linearLayout.Visibility = ViewStates.Gone;
            adapter = new AmountItemListAdapter
            {
                Items = items
            };

            rvItems.SetLayoutManager(new LinearLayoutManager(this));
            rvItems.SetAdapter(adapter);
            UpdateTotalView();
        }

        private void UpdateTotalView()
        {
            tvTotal.Text = $"Total: {adapter.Items.Sum(x => x.Item.Amount).ToCurrency()}";
        }
    }
}
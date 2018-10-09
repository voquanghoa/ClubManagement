using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using ClubManagement.Models;
using ClubManagement.Ultilities;

namespace ClubManagement.CustomAdapters
{
    public class AddAmountDialog : Dialog
    {
        public AmountItem AmountItem { get; set; }

        public event EventHandler DoneClick;

        [InjectView(Resource.Id.edtItemName)] private EditText edtItemName;

        [InjectView(Resource.Id.edtAmount)] private EditText edtAmount;

        [InjectOnClick(Resource.Id.btnCancel)]
        private void Cancel(object s, EventArgs e)
        {
            Dismiss();
        }

        [InjectOnClick(Resource.Id.btnDone)]
        private void Done(object s, EventArgs e)
        {
            if (string.IsNullOrEmpty(edtAmount.Text) || string.IsNullOrEmpty(edtItemName.Text))
            {
                return;
            }
            AmountItem = new AmountItem
            {
                Item = new OutcomeAmountItem
                {
                    Name = edtItemName.Text,
                    Amount = long.TryParse(edtAmount.Text, out var amount) ? amount : 0
                }
            };

            DoneClick?.Invoke(s, e);
            Dismiss();
        }

        public AddAmountDialog(Context context) : base(context)
        {

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var view = LayoutInflater.Inflate(Resource.Layout.add_item_dialog, null, false);
            SetContentView(view);
            Cheeseknife.Inject(this, view);
        }
    }
}
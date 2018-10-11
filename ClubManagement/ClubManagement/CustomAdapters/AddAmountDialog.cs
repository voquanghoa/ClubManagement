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
        public const int TypeAdd = 1;

        public const int TypeEdit = 2;

        public AmountItem AmountItem { get; set; }

        public event EventHandler DoneClick;

        [InjectView(Resource.Id.edtItemName)] private EditText edtItemName;

        [InjectView(Resource.Id.edtAmount)] private EditText edtAmount;

        private OutcomeAmountItem item = new OutcomeAmountItem();

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
                Toast.MakeText(Context, Resource.String.please_fill_all_fields, ToastLength.Short).Show();
                return;
            }

            if (type == TypeAdd)
            {
                AmountItem = new AmountItem
                {
                    Item = new OutcomeAmountItem
                    {
                        Name = edtItemName.Text,
                        Amount = long.Parse(edtAmount.Text)
                    }
                };

                DoneClick?.Invoke(s, e);
            }
            else
            {
                item.Amount = long.Parse(edtAmount.Text);
                item.Name = edtItemName.Text;
                DoneClick?.Invoke(item, e);
            }

            Dismiss();
        }

        private int type;

        public AddAmountDialog(Context context, int type, OutcomeAmountItem item) : base(context)
        {
            this.type = type;
            this.item = item;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var view = LayoutInflater.Inflate(Resource.Layout.add_item_dialog, null, false);
            SetContentView(view);
            Cheeseknife.Inject(this, view);
            Init();
        }

        private void Init()
        {
            edtAmount.Text = item.Amount.ToString();
            edtItemName.Text = item.Name ?? "";
        }
    }
}
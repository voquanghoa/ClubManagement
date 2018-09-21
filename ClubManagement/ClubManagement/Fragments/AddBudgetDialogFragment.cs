using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;

namespace ClubManagement.Fragments
{
    public class AddBudgetDialogFragment : DialogFragment
    {
        private readonly DatePickerFragment datePickerFragment = new DatePickerFragment(false);

        [InjectView(Resource.Id.edtTime)] private EditText edtTime;

        public EventHandler SaveClick;

        [InjectView(Resource.Id.edtDescription)]
        private EditText edtDescription;

        [InjectView(Resource.Id.edtAmount)] private EditText edtAmount;

        [InjectOnClick(Resource.Id.btnSave)]
        private void Save(object s, EventArgs e)
        {
            if (!string.IsNullOrEmpty(edtAmount.Text)
                                                             && DateTime.TryParse(edtTime.Text, out var date)
                                                             && !string.IsNullOrEmpty(edtDescription.Text))
            {
                if (int.TryParse(edtAmount.Text, out var amount) && amount > 0)
                {
                    var moneyModel = new MoneyModel
                    {
                        Description = edtDescription.Text,
                        Amount = amount,
                        Time = date
                    };
                    try
                    {
                        MoneysController.Instance.Add(moneyModel);
                        SaveClick?.Invoke(moneyModel, e);
                        Dismiss();
                    }
                    catch (Exception)
                    {
						this.ShowMessage(Resource.String.no_internet_connection);
                    }
                }
                else
                {
					this.ShowMessage(Resource.String.amount_parse_error_message);
                }

                edtTime.Text = string.Empty;
                edtAmount.Text = string.Empty;
                edtDescription.Text = string.Empty;
            }
            else
            {
                Toast.MakeText(Context, Resources.GetString(Resource.String.fill_all_fields), ToastLength.Short).Show();
            }
        }

        [InjectOnClick(Resource.Id.btnCancel)]
        private void Cancel(object s, EventArgs e)
        {
            Dismiss();
        }

        [InjectOnClick(Resource.Id.edtTime)]
        private void PickDate(object s, EventArgs e)
        {
            datePickerFragment.Show(FragmentManager, null);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.dialog_fragment_add_budget, container, false);
            Cheeseknife.Inject(this, view);
            Init();
            return view;
        }

        private void Init()
        {
            datePickerFragment.PickDate += (sender, e) =>
            {
                if (sender is DateTime date)
                {
                    edtTime.Text = date.ToShortDateString();
                }
            };
        }
    }
}
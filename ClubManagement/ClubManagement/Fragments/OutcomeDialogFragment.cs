using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;

namespace ClubManagement.Fragments
{
    public class OutcomeDialogFragment : DialogFragment
    {
        [InjectView(Resource.Id.edtTitle)]
        private EditText edtTitle;

        [InjectView(Resource.Id.edtDescription)]
        private EditText edtDescription;

        [InjectView(Resource.Id.edtAmount)]
        private EditText edtAmount;

        [InjectView(Resource.Id.edtDate)]
        private EditText edtDate;

        [InjectOnClick(Resource.Id.btnCancel)]
        private void OnClickCancel(object sender, EventArgs e)
        {
            Dismiss();
        }

        private DatePickerFragment datePickerFragment = new DatePickerFragment(false);

        public event EventHandler SaveClick;

        [InjectOnClick(Resource.Id.btnSave)]
        private void OnClickSave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(edtAmount.Text)
                && DateTime.TryParse(edtDate.Text, out var date) 
                && !string.IsNullOrEmpty(edtTitle.Text)
                && !string.IsNullOrEmpty(edtDescription.Text))
            {
                if (int.TryParse(edtAmount.Text, out var amount) && amount > 0)
                {
                    var outcomeModel = new OutcomeModel()
                    {
                        Title = edtTitle.Text,
                        Description = edtDescription.Text,
                        Amount = amount,
                        Date = date
                    };
                    try
                    {
                        OutComesController.Instance.Add(outcomeModel);
                        SaveClick?.Invoke(outcomeModel, e);
                        Dismiss();
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(Context, Resources.GetString(Resource.String.no_internet_connection), ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(Context, Resources.GetString(Resource.String.amount_parse_error_message), ToastLength.Short).Show();
                }

                edtDescription.Text = string.Empty;
                edtAmount.Text = string.Empty;
                edtDate.Text = string.Empty;
                edtTitle.Text = string.Empty;
            }
            else
            {
                Toast.MakeText(Context, Resources.GetString(Resource.String.add_outcome_dialog_error), ToastLength.Short).Show();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.DialogFragmentOutcome, container, false);

            Cheeseknife.Inject(this, view);

            datePickerFragment.PickDate += DatePickerFragment_PickDate;
            edtDate.Click += EdtDate_Click;

            return view;
        }

        private void DatePickerFragment_PickDate(object sender, EventArgs e)
        {
            if (sender is DateTime date)
            {
                edtDate.Text = date.ToShortDateString();
            }
        }

        private void EdtDate_Click(object sender, EventArgs e)
        {
            datePickerFragment.Show(FragmentManager, null);
        }
    }
}
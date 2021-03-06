﻿using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;

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
                        Amount = amount,
                        Date = date
                    };
                    try
                    {
                        OutComesController.Instance.Add(outcomeModel);
                        SaveClick?.Invoke(outcomeModel, e);
                        Dismiss();
                    }
                    catch (Exception ex)
                    {
						this.ShowMessage(ex.Message);
                    }
                }
                else
                {
					this.ShowMessage(Resource.String.amount_parse_error_message);
                }

                edtDescription.Text = string.Empty;
                edtAmount.Text = string.Empty;
                edtDate.Text = string.Empty;
                edtTitle.Text = string.Empty;
            }
            else
            {
				this.ShowMessage(Resource.String.add_outcome_dialog_error);
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
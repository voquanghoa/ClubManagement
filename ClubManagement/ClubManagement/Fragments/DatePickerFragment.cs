using System;
using Android.OS;
using Android.Widget;
using Android.App;
using DialogFragment = Android.Support.V4.App.DialogFragment;

namespace ClubManagement.Fragments
{
    public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        private readonly bool canChoosePastDate;

        public DatePickerFragment(bool canChoosePastDate)
        {
            this.canChoosePastDate = canChoosePastDate;
        }
        public event EventHandler PickDate;

        private DatePickerDialog datePickerDialog;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var currently = DateTime.Now;
            datePickerDialog = new DatePickerDialog(Activity, this, currently.Year, currently.Month - 1, currently.Day);
            if (canChoosePastDate)
            {
                datePickerDialog.DatePicker.MinDate = Java.Lang.JavaSystem.CurrentTimeMillis();
            }
            return datePickerDialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            PickDate?.Invoke(selectedDate, null);
        }
    }
}
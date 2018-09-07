using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace ClubManagement.Fragments
{
    public class CustomDatePickerDialog : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        private DatePickerDialog datePickerDialog;

        public DateTime MinTime { get; }

        public CustomDatePickerDialog(DateTime minTime)
        {
            MinTime = minTime;
        }

        public event EventHandler PickDate;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            datePickerDialog = new DatePickerDialog(Activity, this, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
            datePickerDialog.DatePicker.MinDate = (long)(MinTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return datePickerDialog;
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            PickDate?.Invoke(new DateTime(year, month + 1, dayOfMonth), null);
        }
    }
}
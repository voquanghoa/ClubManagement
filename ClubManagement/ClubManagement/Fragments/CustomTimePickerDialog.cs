using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace ClubManagement.Fragments
{
    public class CustomTimePickerDialog : DialogFragment, TimePickerDialog.IOnTimeSetListener
    {
        public event EventHandler PickTime;

        private TimePickerDialog timePickerDialog;

        public CustomTimePickerDialog(DateTime currentTime)
        {
            CurrentTime = currentTime;
        }

        public DateTime CurrentTime { get; set; }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            var time = new DateTime(CurrentTime.Year, CurrentTime.Month, CurrentTime.Day, hourOfDay, minute, 0);
            PickTime?.Invoke(time, null);
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            timePickerDialog = new TimePickerDialog(Activity, this, CurrentTime.Hour, CurrentTime.Minute, true);
            return timePickerDialog;
        }
    }
}
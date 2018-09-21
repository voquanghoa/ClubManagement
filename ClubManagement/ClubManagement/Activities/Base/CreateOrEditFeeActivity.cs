using System;

using Android.App;
using Android.OS;
using Android.Widget;
using ClubManagement.Ultilities;
using ClubManagement.Fragments;

namespace ClubManagement.Activities.Base
{
    public class CreateOrEditFeeActivity : Activity
    {
        [InjectOnClick(Resource.Id.btnCancel)]
        protected void Cancel(object sender, EventArgs e)
        {
            this.ShowConfirmDialog(
                Resource.String.create_fee_cancel_title,
                Resource.String.cross_create_event_message,
                Finish,
                () => { }).Show();
        }

        [InjectView(Resource.Id.edtDeadline)]
        protected EditText edtDeadline;

        [InjectOnClick(Resource.Id.edtDeadline)]
        protected void PickDeadline(object sender, EventArgs eventArgs)
        {
            var datePickerDialog = new CustomDatePickerDialog(DateTime.Now);

            datePickerDialog.PickDate += (s, e) =>
            {
                if (s is DateTime dateTime)
                {
                    deadLine = dateTime;
                    edtDeadline.Text = dateTime.ToDateString();
                }
            };

            datePickerDialog.Show(FragmentManager, "");
        }

        private DateTime deadLine;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityCreateFee);
            Cheeseknife.Inject(this);
        }
    }
}
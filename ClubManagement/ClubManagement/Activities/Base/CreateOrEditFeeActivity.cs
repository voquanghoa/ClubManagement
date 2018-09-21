using System;

using Android.App;
using Android.OS;
using Android.Widget;
using ClubManagement.Ultilities;
using ClubManagement.Fragments;
using Android.Content;
using Android.Runtime;

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

        [InjectView(Resource.Id.imgViewChooseFeeGroup)]
        protected ImageView imgViewChooseFeeGroup;

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

        [InjectView(Resource.Id.edtChooseFeeGroup)]
        protected EditText edtChooseFeeGroup;

        [InjectOnClick(Resource.Id.edtChooseFeeGroup)]
        protected void ChooseFeeGroup(object sender, EventArgs eventArgs)
        {
            StartActivityForResult(typeof(ChooseFeeGroupActivity), 0);
        }

        private DateTime deadLine;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityCreateFee);
            Cheeseknife.Inject(this);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                var feeGroup = AppConstantValues.FeeGrooups.Find(x => x.Id == data.GetIntExtra("Id", 1));
                imgViewChooseFeeGroup.SetImageResource(feeGroup.ImageId);
                edtChooseFeeGroup.Text = GetString(feeGroup.TitleId);
            }
        }
    }
}
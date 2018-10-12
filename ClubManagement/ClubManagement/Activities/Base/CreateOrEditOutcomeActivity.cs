using System;

using Android.App;
using Android.OS;
using Android.Widget;
using ClubManagement.Ultilities;
using ClubManagement.Fragments;
using Android.Content;
using Android.Runtime;
using ClubManagement.Models;

namespace ClubManagement.Activities.Base
{
    public class CreateOrEditOutcomeActivity : Activity
    {
        [InjectView(Resource.Id.edtTitle)]
        protected EditText edtTitle;

        [InjectView(Resource.Id.edtAmount)]
        protected EditText edtAmount;

        [InjectView(Resource.Id.edtDeadline)]
        protected EditText edtDeadline; 

        [InjectView(Resource.Id.imgViewChooseOutcomeGroup)]
        protected ImageView imgViewChooseOutcomeGroup;

        [InjectOnClick(Resource.Id.edtDeadline)]
        protected void PickDeadline(object sender, EventArgs eventArgs)
        {
            var datePickerDialog = new CustomDatePickerDialog();

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

        [InjectView(Resource.Id.edtChooseOutcomeGroup)]
        protected EditText edtChooseOutcomeGroup;

        [InjectView(Resource.Id.tvTitle)]
        protected TextView tvTitle;

        [InjectOnClick(Resource.Id.edtChooseOutcomeGroup)]
        protected void ChooseOutcomeGroup(object sender, EventArgs eventArgs)
        {
            var intent = new Intent(this, typeof(ChooseFeeOrOutcomeGroupActivity));
            intent.PutExtra("Title", GetString(Resource.String.choose_outcome_group_title));

            StartActivityForResult(intent, 0);
        }

        protected DateTime deadLine;

        protected FeeOrOutcomeGroupModel outcomeGroup;

        protected bool IsFieldsEmpty() =>
            (string.IsNullOrEmpty(edtTitle.Text) ||
            string.IsNullOrEmpty(edtChooseOutcomeGroup.Text) ||
            string.IsNullOrEmpty(edtDeadline.Text) ||
            !long.TryParse(edtAmount.Text, out long amount) || amount == 0);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityCreateOutcome);
            Cheeseknife.Inject(this);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok && requestCode == 0)
            {
                outcomeGroup = AppConstantValues.FeeGrooups.Find(x => x.Id.Equals(data.GetStringExtra("Id")));
                imgViewChooseOutcomeGroup.SetImageResource(outcomeGroup.ImageId);
                edtChooseOutcomeGroup.Text = GetString(outcomeGroup.TitleId);
            }
        }
    }
}
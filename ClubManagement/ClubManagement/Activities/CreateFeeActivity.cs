using Android.App;
using Android.OS;
using Android.Widget;
using ClubManagement.Activities.Base;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using System;

namespace ClubManagement.Activities
{
    [Activity(Label = "CreateFeeActivity")]
    public class CreateFeeActivity : CreateOrEditFeeActivity
    {
        [InjectOnClick(Resource.Id.btnCancel)]
        private void Cancel(object sender, EventArgs e)
        {
            this.ShowConfirmDialog(
				Resource.String.confirm,
                Resource.String.create_fee_cancel_message,
                Finish,
                () => { }).Show();
        }

        [InjectOnClick(Resource.Id.btnDone)]
        private void Done(object sender, EventArgs e)
        {
            if (IsFieldsEmpty())
            {
                this.ShowMessage(Resource.String.please_fill_all_fields);
                return;
            }

            var userMoneyModel = new MoneyModel()
            {
                Description = edtDescription.Text,
                Amount = long.Parse(edtAmount.Text),
                Group = feeGroup.Id,
                Time = deadLine
            };

            var progressDialog = this.CreateDialog(GetString(Resource.String.adding_fee),
                GetString(Resource.String.wait));
            progressDialog.Show();

            this.DoRequest(MoneysController.Instance.Add(userMoneyModel), () =>
            {
                progressDialog.Dismiss();
                this.ShowMessage(Resource.String.create_fee_success);

                SetResult(Result.Ok);
                Finish();
            });
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            deadLine = DateTime.Now;
            edtDeadline.Text = deadLine.ToDateString();
        }
    }
}
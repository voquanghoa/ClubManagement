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
    public class CreateOutcomeActivity : CreateOrEditOutcomeActivity
    {
        [InjectOnClick(Resource.Id.btnCancel)]
        private void Cancel(object sender, EventArgs e)
        {
            this.ShowConfirmDialog(
				Resource.String.confirm,
                Resource.String.create_outcome_cancel_message,
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

            var outcomeModel = new OutcomeModel()
            {
                Title = edtTitle.Text,
                Amount = long.Parse(edtAmount.Text),
                Group = outcomeGroup.Id,
                Date = deadLine
            };

            var progressDialog = this.CreateDialog(GetString(Resource.String.adding_outcome),
                GetString(Resource.String.wait));
            progressDialog.Show();

            this.DoRequest(OutComesController.Instance.Add(outcomeModel), () =>
            {
                progressDialog.Dismiss();
                this.ShowMessage(Resource.String.create_outcome_success);

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
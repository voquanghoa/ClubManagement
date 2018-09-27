using System;

using Android.App;
using Android.Content;
using Android.OS;
using ClubManagement.Activities.Base;
using ClubManagement.Ultilities;
using ClubManagement.Models;
using Newtonsoft.Json;
using ClubManagement.Controllers;

namespace ClubManagement.Activities
{
    [Activity(Label = "EditFeeActivity")]
    public class EditOutcomeActivity : CreateOrEditOutcomeActivity
    {
        [InjectOnClick(Resource.Id.btnCancel)]
        private void Cancel(object sender, EventArgs e)
        {
            this.ShowConfirmDialog(
				Resource.String.confirm,
                Resource.String.edit_outcome_cancel_message,
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
            };

            outcomeModel.Title = edtTitle.Text;
            outcomeModel.Amount = long.Parse(edtAmount.Text);
            outcomeModel.Group = outcomeGroup.Id;
            outcomeModel.Date = deadLine;

            var progressDialog = this.CreateDialog(GetString(Resource.String.editing_outcome),
                GetString(Resource.String.wait));
            progressDialog.Show();

            this.DoRequest(async () =>
            {
                await OutComesController.Instance.Edit(outcomeModel);
            }, () =>
            {
                progressDialog.Dismiss();
                this.ShowMessage(Resource.String.edit_outcome_success);
                Finish();
            });
        }

        private OutcomeModel outcomeModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Init();
        }

        private void Init()
        {
            var content = Intent.GetStringExtra("OutcomeDetail");
            outcomeModel = JsonConvert.DeserializeObject<OutcomeModel>(content);
            outcomeGroup = AppConstantValues.FeeGrooups.Find(x => x.Id.Equals(outcomeModel.Group));
            deadLine = outcomeModel.Date;

            tvTitle.Text = GetString(Resource.String.edit_outcome);
            edtTitle.Text = outcomeModel.Title;
            edtAmount.Text = outcomeModel.Amount.ToString();
            edtChooseOutcomeGroup.Text = GetString(outcomeGroup.TitleId);
            imgViewChooseOutcomeGroup.SetImageResource(outcomeGroup.ImageId);
            edtDeadline.Text = deadLine.ToDateString();
        }
    }
}
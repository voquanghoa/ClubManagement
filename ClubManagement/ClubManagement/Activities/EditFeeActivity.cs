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
    public class EditFeeActivity : CreateOrEditFeeActivity
    {
        [InjectOnClick(Resource.Id.btnCancel)]
        private void Cancel(object sender, EventArgs e)
        {
            this.ShowConfirmDialog(
				Resource.String.confirm,
                Resource.String.edit_fee_cancel_message,
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

            var nameChanged = !moneyModel.Description.Equals(edtDescription.Text);
            var amountChanged = moneyModel.Amount != long.Parse(edtAmount.Text);
            var deadlineChanged = moneyModel.Time != deadLine;

            moneyModel.Description = edtDescription.Text;
            moneyModel.Amount = long.Parse(edtAmount.Text);
            moneyModel.Group = feeGroup.Id;
            moneyModel.Time = deadLine;

            var progressDialog = this.CreateDialog(GetString(Resource.String.editing_fee),
                GetString(Resource.String.wait));
            progressDialog.Show();

            //Admin changed the name, amount and deadline of the fee Spring roll

            var message = "";

            if (nameChanged && amountChanged && deadlineChanged)
            {
                message = "Admin changed the name, amount and deadline of the fee";
            }
            else if (nameChanged && amountChanged || amountChanged && deadlineChanged || deadlineChanged && nameChanged)
            {
                if (nameChanged && amountChanged)
                {
                    message = "Admin changed the name and amount of the fee";
                }

                if (amountChanged && deadlineChanged)
                {
                    message = "Admin changed the amount and deadline of the fee";
                }

                if (deadlineChanged && nameChanged)
                {
                    message = "Admin changed the name and deadline of the fee";
                }
            }
            else
            {
                if (nameChanged)
                {
                    message = "Admin changed the name of the fee";
                }

                if (amountChanged)
                {
                    message = "Admin changed the amount of the fee";
                }

                if (deadlineChanged)
                {
                    message = "Admin changed the location of the fee";
                }
            }

            this.DoRequest(MoneysController.Instance.Edit(moneyModel), () =>
            {
                progressDialog.Dismiss();
                this.ShowMessage(Resource.String.edit_fee_success);

                if (nameChanged || amountChanged || deadlineChanged)
                {
                    var notificationsController = NotificationsController.Instance;

                    notificationsController.UpdateNotificationAsync(new NotificationModel()
                    {
                        Message = $"{message} {moneyModel.Description}",
                        Type = AppConstantValues.NotificationEditFee,
                        TypeId = moneyModel.Id,
                        LastUpdate = DateTime.Now
                    });

                    notificationsController
                        .PushNotifyAsync(AppConstantValues.NotificationEditFee, moneyModel.Description);
                }

                Finish();
            });
        }

        private MoneyModel moneyModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Init();
        }

        private void Init()
        {
            var content = Intent.GetStringExtra("MoneyDetail");
            moneyModel = JsonConvert.DeserializeObject<MoneyModel>(content);
            feeGroup = AppConstantValues.FeeGrooups.Find(x => x.Id.Equals(moneyModel.Group));
            deadLine = moneyModel.Time;

            tvTitle.Text = GetString(Resource.String.edit_fee);
            edtDescription.Text = moneyModel.Description;
            edtAmount.Text = moneyModel.Amount.ToString();
            edtChooseFeeGroup.Text = GetString(feeGroup.TitleId);
            imgViewChooseFeeGroup.SetImageResource(feeGroup.ImageId);
            edtDeadline.Text = deadLine.ToDateString();
        }
    }
}
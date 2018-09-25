using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
        [InjectOnClick(Resource.Id.btnDone)]
        private void Done(object sender, EventArgs e)
        {
            if (IsFieldsEmpty())
            {
                Toast.MakeText(this, GetString(Resource.String.please_fill_all_fields), ToastLength.Short).Show();
                return;
            };

            moneyModel.Description = edtDescription.Text;
            moneyModel.Amount = long.Parse(edtAmount.Text);
            moneyModel.Group = feeGroup.Id;
            moneyModel.Time = deadLine;

            var progressDialog = this.CreateDialog(GetString(Resource.String.editing_fee),
                GetString(Resource.String.wait));
            progressDialog.Show();

            this.DoRequest(async () =>
            {
                await MoneysController.Instance.Edit(moneyModel);
            }, () =>
            {
                progressDialog.Dismiss();
                Toast.MakeText(this, Resource.String.edit_event_success, ToastLength.Short).Show();
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
            var content = Intent.GetStringExtra("EventDetail");
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
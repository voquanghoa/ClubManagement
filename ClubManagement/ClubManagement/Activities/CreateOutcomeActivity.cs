using Android.App;
using Android.OS;
using Android.Widget;
using ClubManagement.Activities.Base;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Newtonsoft.Json;

namespace ClubManagement.Activities
{
    [Activity(Label = "CreateFeeActivity")]
    public class CreateOutcomeActivity : CreateOrEditOutcomeActivity
    {
        private List<OutcomeAmountItem> items = new List<OutcomeAmountItem>();

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
                Date = deadLine,
                Items = items
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
            edtAmount.Focusable = false;
            edtAmount.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(AddAmountActivity));
                if (items.Count >= 0)
                {
                    intent.PutExtra("items", JsonConvert.SerializeObject(items));
                }
                StartActivityForResult(intent, 1);
            };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1 && resultCode == Result.Ok)
            {
                items = JsonConvert.DeserializeObject<List<OutcomeAmountItem>>(data.GetStringExtra("items"));
                edtAmount.Text = items.Sum(x => x.Amount).ToString();
            }
        }
    }
}
using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;

namespace ClubManagement.Fragments
{
    public class OutcomeDialogFragment : DialogFragment
    {
        [InjectView(Resource.Id.edtTitle)]
        private EditText edtTitle;

        [InjectView(Resource.Id.edtDescription)]
        private EditText edtDescription;

        [InjectView(Resource.Id.edtAmount)]
        private EditText edtAmount;

        [InjectView(Resource.Id.edtDate)]
        private EditText edtDate;

        [InjectOnClick(Resource.Id.btnCancel)]
        private void OnClickCancel(object sender, EventArgs e)
        {
            Dismiss();
        }

        public event EventHandler SaveClick;

        [InjectOnClick(Resource.Id.btnSave)]
        private void OnClickSave(object sender, EventArgs e)
        {
            var outcomeModel = new OutcomeModel()
            {
                Title = edtTitle.Text,
                Description = edtDescription.Text,
                Amount = int.Parse(edtAmount.Text),
                Date = DateTime.Parse(edtDate.Text)
            };

            OutComesController.Instance.Add(outcomeModel);

            SaveClick.Invoke(outcomeModel, e);

            Dismiss();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.DialogFragmentOutcome, container, false);

            Cheeseknife.Inject(this, view);

            return view;
        }
    }
}
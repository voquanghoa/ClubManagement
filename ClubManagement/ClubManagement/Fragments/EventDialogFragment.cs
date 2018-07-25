using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using Android.Gms.Location.Places.UI;
using Android.Content;
using Android.Gms.Location.Places;
using System.Threading.Tasks;

namespace ClubManagement.Fragments
{
    public class EventDialogFragment : DialogFragment
    {
        [InjectView(Resource.Id.edtTitle)]
        private EditText edtTitle;

        [InjectView(Resource.Id.edtDescription)]
        private EditText edtDescription;

        [InjectView(Resource.Id.edtLocation)]
        private EditText edtLocation;

        [InjectOnClick(Resource.Id.edtLocation)]
        private void OnClickEdtLocation(object sender, EventArgs e)
        {
            var builder = new PlacePicker.IntentBuilder();

            StartActivityForResult(builder.Build(this.Activity), PlacePickerRequset);
        }

        private const int PlacePickerRequset = 1;

        private const int ResultOk = -1;

        [InjectView(Resource.Id.edtDate)]
        private EditText edtDate;

        [InjectOnClick(Resource.Id.edtDate)]
        private void OnClickEdtDate(object sender, EventArgs e)
        {
            datePickerFragment.Show(FragmentManager, null);
        }

        [InjectOnClick(Resource.Id.btnCancel)]
        private void OnClickCancel(object sender, EventArgs e)
        {
            Dismiss();
        }

        private DatePickerFragment datePickerFragment = new DatePickerFragment();

        public event EventHandler SaveClick;

        private IPlace place;

        [InjectOnClick(Resource.Id.btnSave)]
        private void OnClickSave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(edtTitle.Text)
                && !string.IsNullOrEmpty(edtDescription.Text)
                && !string.IsNullOrEmpty(edtLocation.Text)
                && DateTime.TryParse(edtDate.Text, out var date) 
                && place != null)
            {
                var eventModel = new EventModel()
                {
                    Title = edtTitle.Text,
                    Description = edtDescription.Text,
                    Longitude = place.LatLng.Longitude,
                    Latitude = place.LatLng.Latitude,
                    Place = place.AddressFormatted.ToString(),
                    Time = date,
                    CreatedTime = DateTime.Now,
                    CreatedBy = AppDataController.Instance.UserName
                };
                try
                {
                    EventsController.Instance.Add(eventModel);
                    SaveClick?.Invoke(eventModel, e);
                    Dismiss();
                }
                catch (Exception)
                {
                    Toast.MakeText(Context, Resources.GetString(Resource.String.no_internet_connection), ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(Context, Resources.GetString(Resource.String.add_outcome_dialog_error), ToastLength.Short).Show();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.DialogFragmentEvent, container, false);

            Cheeseknife.Inject(this, view);

            datePickerFragment.PickDate += DatePickerFragment_PickDate;

            return view;
        }

        private void DatePickerFragment_PickDate(object sender, EventArgs e)
        {
            if (sender is DateTime date)
            {
                edtDate.Text = date.ToShortDateString();
            }
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == PlacePickerRequset && resultCode == ResultOk)
            {
                place = PlacePicker.GetPlace(this.Context, data);

                edtLocation.Text = place.AddressFormatted.ToString();
            }
        }
    }
}
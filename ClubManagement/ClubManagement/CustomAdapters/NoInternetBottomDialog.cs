using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using ClubManagement.Ultilities;
using Plugin.Connectivity;

namespace ClubManagement.CustomAdapters
{
    class NoInternetBottomDialog : BottomSheetDialog
    {
        [InjectOnClick(Resource.Id.btnRetry)]
        private void Retry(object s, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
				this.Context.ShowMessage(Resource.String.no_internet_connection);
                return;
            }
            Dismiss();
        }


        public NoInternetBottomDialog(Context context) : base(context)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var view = LayoutInflater.Inflate(Resource.Layout.no_internet_bottom_dialog_fragment_layout, null);
            Cheeseknife.Inject(this, view);
            SetContentView(view);
            SetCancelable(false);
        }
    }
}
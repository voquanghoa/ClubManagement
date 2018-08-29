using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Graphics.Drawables;
using Android.Graphics;
using EventArgs = System.EventArgs;
using EventHandler = System.EventHandler;
using Android.Widget;

namespace ClubManagement.Fragments
{
    public class UnjoinEventFragment : DialogFragment
    {
        [InjectOnClick(Resource.Id.btnCancel)]
        private void OnClickCancel(object sender, EventArgs e)
        {
            Dismiss();
        }

        [InjectOnClick(Resource.Id.btnNotGoing)]
        private void OnClickNotGoing(object sender, EventArgs e)
        {
            NotGoing.Invoke(this.Dialog, e);
        }

        public event EventHandler NotGoing;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.DialogFragmentUnjoinEvent, container);    

            Cheeseknife.Inject(this, view);

            view.SetMinimumWidth((int)(Resources.DisplayMetrics.WidthPixels * 0.9));

            Dialog.Window.Attributes.Y = Resources.DisplayMetrics.HeightPixels;
            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            Dialog.Window.SetDimAmount(0);

            return view;
        }
    }
}
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Content;
using Android.Provider;
using Result = Android.App.Result;
using Firebase.Storage;
using Firebase;
using Android.Gms.Tasks;
using ClubManagement.Ultilities;
using Android.Widget;
using EventArgs = System.EventArgs;
using EventHandler = System.EventHandler;
using Android.Net;

namespace ClubManagement.Fragments
{
    public class ChangeAvatarFragment : DialogFragment, IOnSuccessListener, IOnFailureListener
    {
        [InjectOnClick(Resource.Id.btnCancel)]
        private void OnClickCancel(object sender, EventArgs e)
        {
            Dismiss();
        }

        private const int RequestImageCapture = 1;
        
        private const int RequestPickAvatar = 2;

        [InjectOnClick(Resource.Id.btnTakePhoto)]
        private void OnClickTakePhoto(object sender, EventArgs e)
        {
            var takePictureIntent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(takePictureIntent, RequestImageCapture);
        }

        [InjectOnClick(Resource.Id.btnSelectPhoto)]
        private void OnClickSelectPhoto(object sender, EventArgs e)
        {
            var intent = new Intent(Intent.ActionGetContent);
            intent.SetType("image/*");
            StartActivityForResult(intent, RequestPickAvatar);
        }

        private Android.App.ProgressDialog progressDialog;

        private const string storageUrl = "gs://clubmanagement-98743.appspot.com";

        public event EventHandler ChangeAvatar;

        public ChangeAvatarFragment()
        {
            FirebaseApp.InitializeApp(Context);
            storageReference = firebaseStorage.GetReferenceFromUrl(storageUrl);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == RequestImageCapture && resultCode== Result.Ok.GetHashCode())
            {
                var imageBitmap = (Bitmap)data.Extras.Get("data");
                var uriPath = MediaStore.Images.Media.InsertImage(Activity.ContentResolver, imageBitmap, null, null);

                UpdateAvatar(Uri.Parse(uriPath));
            }

            if (requestCode == RequestPickAvatar && resultCode == Result.Ok.GetHashCode())
            {
                UpdateAvatar(data.Data);
            }
        }

        private FirebaseStorage firebaseStorage = FirebaseStorage.Instance;

        private StorageReference storageReference;

        private void UpdateAvatar(Uri uri)
        {
            var image = storageReference.Child($"Images/{System.Guid.NewGuid()}");

            var title = Resources.GetString(Resource.String.change_avatar);
            var message = Resources.GetString(Resource.String.wait);
            progressDialog = Context.CreateDialog(title, message);
            progressDialog.Show();

            image.PutFile(uri)
                .AddOnSuccessListener(this)
                .AddOnFailureListener(this);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.DialogFragmentChangeAvatar, container);    

            Cheeseknife.Inject(this, view);

            Dialog.Window.Attributes.Y = Resources.DisplayMetrics.HeightPixels;
            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent)); Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            Dialog.Window.SetDimAmount(0);

            return view;
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            progressDialog.Dismiss();

            var message = Resources.GetString(Resource.String.change_avatar_failure);
            Activity.RunOnUiThread(() => 
                Toast.MakeText(Context, message, ToastLength.Short).Show());

            Dismiss();
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapShot = (UploadTask.TaskSnapshot)result;
            ChangeAvatar.Invoke(snapShot.DownloadUrl.ToString(), new EventArgs());

            progressDialog.Dismiss();

            var message = Resources.GetString(Resource.String.change_avatar_success);
            Activity.RunOnUiThread(() =>
                Toast.MakeText(Context, message, ToastLength.Short).Show());

            Dismiss();
        }
    }
}
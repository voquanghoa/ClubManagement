using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Square.Picasso;
using Android.Gms.Maps.Model;

namespace ClubManagement.Models
{
    public class Target : Java.Lang.Object, ITarget
    {
        public Marker Marker { set; get; }

        public float Alpha { set; get; }

        public void OnBitmapFailed(Drawable p0)
        {
            
        }

        public void OnBitmapLoaded(Bitmap p0, Picasso.LoadedFrom p1)
        {
            var size = Math.Min(Math.Max(p0.Height, p0.Width), 50);

            var result = Bitmap.CreateBitmap(size, size, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(result);

            Paint paint = new Paint();
            Rect rect = new Rect(0, 0, size, size);

            var width = Math.Min(p0.Width, size);
            var heigth = Math.Min(p0.Height, size);

            p0 = Bitmap.CreateScaledBitmap(p0, width, heigth, false);

            var radius = size / 2;

            canvas.DrawCircle(radius, radius, radius, paint);
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(p0, rect, rect, paint);

            Marker.SetIcon(BitmapDescriptorFactory.FromBitmap(result));
            Marker.Alpha = Alpha;
        }

        public void OnPrepareLoad(Drawable p0)
        {
            
        }
    }
}
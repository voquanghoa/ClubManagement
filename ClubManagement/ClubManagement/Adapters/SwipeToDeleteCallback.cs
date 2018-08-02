using System;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.App;

namespace ClubManagement.Adapters
{
    public class SwipeLeftToDeleteCallback : ItemTouchHelper.SimpleCallback
    {
        public event EventHandler<ClickEventArgs> SwipeLeft;

        public SwipeLeftToDeleteCallback(int dragDirs, int swipeDirs) : base(dragDirs, swipeDirs)
        {
        }

        protected SwipeLeftToDeleteCallback(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return false;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            SwipeLeft?.Invoke(viewHolder, new ClickEventArgs() { Position = viewHolder.AdapterPosition });
        }

        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            var itemView = viewHolder.ItemView;
            var paint = new Paint();
            var icon = BitmapFactory.DecodeResource(Application.Context.Resources, 
                Resource.Drawable.icon_trash);
            var margin = 10;

            var rect = new Rect(itemView.Left, itemView.Top, itemView.Right, itemView.Bottom);
            paint.Color = Color.Red;
            c.DrawRect(rect, paint);
            c.DrawBitmap(icon, 
                itemView.Right - icon.Width - margin, 
                rect.CenterY() - icon.Height / 2, 
                paint);

            base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
        }
    }
}
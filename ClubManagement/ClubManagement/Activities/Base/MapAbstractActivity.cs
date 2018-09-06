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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using ClubManagement.Ultilities;
using static Android.Gms.Maps.GoogleMap;
using Android.Graphics;

namespace ClubManagement.Activities.Base
{
    public abstract class MapAbstractActivity : Activity, IOnMapReadyCallback, IInfoWindowAdapter
    {
		protected GoogleMap googleMap;

		protected event EventHandler MapReady;

		protected Marker AddMapMarker(double lat, double lng, string title)
        {
            var markerOption = new MarkerOptions()
                .SetPosition(new LatLng(lat, lng))
                .SetTitle(title);
            
            return googleMap.AddMarker(markerOption);
        }

        protected void MoveMapCamera(double lat, double lng)
        {
            var builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(lat, lng));
			builder.Zoom(AppConstantValues.DefaultMapZoomLevel);

            var cameraPosition = builder.Build();
            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            googleMap.MoveCamera(cameraUpdate);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            this.googleMap = googleMap;
            MapReady?.Invoke(googleMap, EventArgs.Empty);
            googleMap.SetInfoWindowAdapter(this);
        }

        public View GetInfoContents(Marker marker) => null;

        public View GetInfoWindow(Marker marker)
        {
            LinearLayout info = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical
            };

            info.SetBackgroundColor(Color.Argb(200, 255, 255, 255));
            info.SetPadding(4, 4, 4, 4);

            var title = new TextView(this);
            title.SetTextColor(Color.Black);
            title.Gravity = GravityFlags.Center;
            title.SetTypeface(null, TypefaceStyle.Bold);
            title.SetText(marker.Title, TextView.BufferType.Normal);

            var snippet = new TextView(this);
            title.SetTextColor(Color.Black);
            title.Gravity = GravityFlags.Center;
            title.SetTypeface(null, TypefaceStyle.Bold);
            snippet.SetText(marker.Snippet, TextView.BufferType.Normal);

            info.AddView(title);
            info.AddView(snippet);

            return info;
        }
    }
}
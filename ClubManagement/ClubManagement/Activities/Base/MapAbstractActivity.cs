using System;

using Android.App;
using Android.Views;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using ClubManagement.Ultilities;
using static Android.Gms.Maps.GoogleMap;
using Android.Widget;
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
            var view = LayoutInflater.Inflate(Resource.Layout.InfoMarker, null);

            view.FindViewById<TextView>(Resource.Id.tvTitle).Text = marker.Title;
            view.FindViewById<TextView>(Resource.Id.tvSnippet).Text = marker.Snippet;

            return view;
        }
    }
}
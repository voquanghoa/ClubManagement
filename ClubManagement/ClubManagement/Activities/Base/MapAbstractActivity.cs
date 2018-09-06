using System;

using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using ClubManagement.Ultilities;

namespace ClubManagement.Activities.Base
{
    public abstract class MapAbstractActivity : Activity, IOnMapReadyCallback
    {
		protected GoogleMap googleMap;

		protected event EventHandler MapReady;

		protected Marker AddMapMarker(double lat, double lng, string title, int iconResourceId)
        {
			var markerOption = new MarkerOptions()
				.SetPosition(new LatLng(lat, lng))
				.SetTitle(title)
				.SetIcon(BitmapDescriptorFactory.FromResource(iconResourceId));
            
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
        }
    }
}
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

namespace ClubManagement.Activities.Base
{
    public abstract class MapAbstractActivity : Activity, IOnMapReadyCallback
    {
        private GoogleMap googleMap;

        protected void AddMarkerMap(double lat, double lng, string title, int iconResourceId)
        {
            googleMap?.AddMarker(new MarkerOptions()
                ?.SetPosition(new LatLng(lat, lng))
                ?.SetTitle(title)
                ?.SetIcon(BitmapDescriptorFactory.FromResource(iconResourceId)));
        }

        protected void MoveCameraMap(double lat, double lng)
        {
            var builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(lat, lng));
            builder.Zoom(12);

            var cameraPosition = builder.Build();
            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            googleMap.MoveCamera(cameraUpdate);
        }

        protected abstract void HandleWhenMapReady(GoogleMap googleMap);

        public void OnMapReady(GoogleMap googleMap)
        {
            this.googleMap = googleMap;

            HandleWhenMapReady(googleMap);
        }
    }
}
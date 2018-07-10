using ClubManagement.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace ClubManagement.Controllers
{
    public class MapsController
    {
        public static MapsController Instance = new MapsController();

        private MapsController()
        {

        }

        public string GetAddress(double latitude, double longitude)
        {
            var webclient = new WebClient();

            var content = webclient.DownloadString($"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key=AIzaSyDOpe15Np8iCzvlVpqzDo83RIVL_eHd-Mo");

            var result = JsonConvert.DeserializeObject<ResultsGeocodeAPIModel>(content);

            return result.Addresses.FirstOrDefault()?.FormattedAddress ?? "";
        }

        public string GetGoTime(double fromLat, double fromLng, double toLat, double toLng)
        {
            var webclient = new WebClient();

            var content = webclient.DownloadString($"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={fromLat},{fromLng}&destinations={toLat},{toLng}&key=AIzaSyDOpe15Np8iCzvlVpqzDo83RIVL_eHd-Mo");

            var result = JsonConvert.DeserializeObject<ResultsMatrixDistanceAPIModel>(content);

            var duration = result.Rows.First().Elements.Last().Duration;

            var goTime = duration == null ? "0m" : duration.Text.Replace("hours", "h").Replace("mins", "m").Replace(" ", "");

            return goTime;
        }
    }
}
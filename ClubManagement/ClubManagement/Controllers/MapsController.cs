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

            var result = JsonConvert.DeserializeObject<Results>(content);

            return result.Addresses.FirstOrDefault()?.FormattedAddress ?? "";
        }
    }
}
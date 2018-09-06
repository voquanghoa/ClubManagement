using ClubManagement.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using Android.Util;

namespace ClubManagement.Controllers
{
    public class MapsController
    {
        public static MapsController Instance = new MapsController();

        private MapsController()
        {

        }

        public ElementMatrixDistanceAPIModel GetGoTime(double fromLat, double fromLng, double toLat, double toLng)
        {
            var webclient = new WebClient();

            var content = webclient.DownloadString(
                $"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={fromLat},{fromLng}&destinations={toLat},{toLng}&key=AIzaSyDOpe15Np8iCzvlVpqzDo83RIVL_eHd-Mo");

            var result = JsonConvert.DeserializeObject<ResultsMatrixDistanceAPIModel>(content);

            var elementMatrixDistanceAPIModel = result.Rows.First().Elements.Last();

            return elementMatrixDistanceAPIModel;
        }
    }
}
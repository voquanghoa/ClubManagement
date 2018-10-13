using ClubManagement.Models;
using Firebase.Xamarin.Database;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ClubManagement.Controllers
{
    class NotificationsController : FirebaseController<NotificationModel>
    {
        private const string Key = "notifications";

        private const string ServerKey = "AAAANOQCTs4:APA91bGpGZmwwk_5C2GMkp1N18n9yA5_ylJlVpOKj1_1FHOCATx-8JxCVecuREhJhxuKcgySDFf8gERpiapCyQ74VowI_clSmcla6fsGRECG_jh1pBLyQvwMPBLR16nXUYbLNtp1hWq-";

        private const string Api = "https://fcm.googleapis.com/fcm/send";

        public static NotificationsController Instance = new NotificationsController();

        private NotificationsController()
        {
            FirebaseClient = new FirebaseClient(LinkFirebase).Child(Key);
        }

        public async void UpdateNotificationAsync(NotificationModel notificationModel)
        {
            var notificationCurrent = Values?.Where(x => x.TypeId == notificationModel.TypeId)?.FirstOrDefault();

            if (!string.IsNullOrEmpty(notificationCurrent?.Id))
            {
                notificationModel.Id = notificationCurrent.Id;

                await Edit(notificationModel);
            }
            else
            {
                await Add(notificationModel);
            }
        }

        public async Task<string> PushNotifyAsync(string Title, string Message)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/json;charset=UTF-8");
                    client.Headers.Add("Authorization", $"key={ServerKey}");

                    var obj = new
                    {
                        registration_ids = UsersController.Instance.Values
                            .Select(x => x.NotificationToken)
                            .Where(x => !string.IsNullOrEmpty(x))
                            .ToArray(),
                        notification = new
                        {
                            title = Title,
                            body = Message
                        }
                    };

                    string json = JsonConvert.SerializeObject(obj);

                    return await client.UploadStringTaskAsync(Api, "POST", json); ;
                }

            }
            catch (WebException e)
            {
                return e.Message;
            }
        }

        public async Task<string> PushNotifyAsync(string id, string Title, string Message)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/json;charset=UTF-8");
                    client.Headers.Add("Authorization", $"key={ServerKey}");

                    var obj = new
                    {
                        to = id,
                        notification = new
                        {
                            title = Title,
                            body = Message
                        }
                    };

                    string json = JsonConvert.SerializeObject(obj);

                    return await client.UploadStringTaskAsync(Api, "POST", json); ;
                }

            }
            catch (WebException e)
            {
                return e.Message;
            }
        }
    }
}
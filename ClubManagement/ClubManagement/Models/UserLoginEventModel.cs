using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class UserLoginEventModel : EventModel
    {
        public UserLoginEventModel()
        {

        }

        public UserLoginEventModel(EventModel eventModel)
        {
            base.Id = eventModel.Id;
            base.Title = eventModel.Title;
            base.Description = eventModel.Description;
            base.Time = eventModel.Time;
            base.CreatedBy = eventModel.CreatedBy;
            base.CreatedTime = eventModel.CreatedTime;
            base.Latitude = eventModel.Latitude;
            base.Longitude = eventModel.Longitude;
            base.Place = eventModel.Place;
        }

        [JsonProperty("isJoined")]
        public bool IsJoined { get; set; }

        public int NumberOfJoinedUsers { get; set; }
    }
}
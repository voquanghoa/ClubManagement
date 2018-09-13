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
            Id = eventModel.Id;
            Title = eventModel.Title;
            Description = eventModel.Description;
            TimeStart = eventModel.TimeStart;
            TimeEnd = eventModel.TimeEnd;
            CreatedBy = eventModel.CreatedBy;
            CreatedTime = eventModel.CreatedTime;
            Latitude = eventModel.Latitude;
            Longitude = eventModel.Longitude;
            Address = eventModel.Address;
            Place = eventModel.Place;
            ImageUrl = eventModel.ImageUrl;
        }

        [JsonProperty("isJoined")]
        public bool IsJoined { get; set; }

        public int NumberOfJoinedUsers { get; set; }
    }
}
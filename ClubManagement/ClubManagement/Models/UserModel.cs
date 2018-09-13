using Newtonsoft.Json;
using System;

namespace ClubManagement.Models
{
    public class UserModel : FirebaseModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; } = "";

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("lastLogin")]
        public DateTime LastLogin { get; set; } = new DateTime();

        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; } = new DateTime(2017, 1, 1);
    }
}
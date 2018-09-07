using System;

namespace ClubManagement.Models
{
    public class PersonGoTimeModel
    {
        public string Name { get; set; }

        public ElementMatrixDistanceAPIModel DistanceAndTime { get; set; }

        public string Avatar { get; set; }

        public bool Selected { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime LastLogin { get; set; }
    }
}
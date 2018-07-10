using System;

namespace ClubManagement.Adapters
{
    public class PersonGoTimeClickEventArgs : EventArgs
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
using System;

namespace ClubManagement.Models
{
    class OutcomeTimeItem : OutcomeItem
    {
        public DateTime Time { get; set; }

        public override int GetType()
        {
            return TimeType;
        }
    }
}
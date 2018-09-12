using System;

namespace ClubManagement.Models
{
    public class FeeDeadlineTimeItem : FeeItem
    {
        public DateTime DeadlineTime { get; set; }

        public override int GetType()
        {
            return TypeTimeHeader;
        }
    }
}
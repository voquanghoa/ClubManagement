using System;

namespace ClubManagement.Models
{
    public class MoneyAdminState
    {
        public UserModel User { get; set; }

        public bool IsPaid { get; set; }

        public DateTime PaidTime { get; set; }
    }
}
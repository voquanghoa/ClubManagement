using System;

namespace ClubManagement.Models
{
    public class BalanceModel
    {
        public string Title { set; get; } = "";

        public string Description { set; get; } = "";

        public int Money { set; get; } = 0;

        public DateTime Date { set; get; } = new DateTime();
    }
}
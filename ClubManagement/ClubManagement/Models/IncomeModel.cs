using System;

namespace ClubManagement.Models
{
    public class IncomeModel : MoneyModel
    {
        public int NumberOfUsers { get; set; }

        public int NumberOfPaidUsers { get; set; }

        public IncomeModel(MoneyModel moneyModel)
        {
            Id = moneyModel.Id;
            Description = moneyModel.Description;
            Amount = moneyModel.Amount;
            Group = moneyModel.Group;
            Time = moneyModel.Time;
        }
    }
}
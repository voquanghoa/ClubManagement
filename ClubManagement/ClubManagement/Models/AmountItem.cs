namespace ClubManagement.Models
{
    public class AmountItem
    {
        public OutcomeAmountItem Item { get; set; }

        public bool IsChooseToDelete { get; set; } = false;

        public bool IsDeleting { get; set; }
    }
}
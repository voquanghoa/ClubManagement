namespace ClubManagement.Models
{
    public class FeeDetailItem : FeeItem
    {

        public MoneyState MoneyState { get; set; }

        public override int GetType()
        {
            return TypeDetail;
        }
    }
}
namespace ClubManagement.Models
{
    public class OutcomeDetailItem : OutcomeItem
    {
        public OutcomeModel OutcomeModel { get; set; }

        public override int GetType()
        {
            return DetailType;
        }
    }
}
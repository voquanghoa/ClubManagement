namespace ClubManagement.Models
{
    public abstract class OutcomeItem
    {
        public const int TimeType = 1;

        public const int DetailType = 2;

        public new abstract int GetType();
    }
}
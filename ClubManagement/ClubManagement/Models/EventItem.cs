namespace ClubManagement.Models
{
    public abstract class EventItem
    {
        public const int TypeHeader = 1;
        public const int TypeDescription = 2;

        public new abstract int GetType();

    }
}
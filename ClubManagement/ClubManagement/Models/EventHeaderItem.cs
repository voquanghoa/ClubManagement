namespace ClubManagement.Models
{
    public class EventHeaderItem : EventItem
    {
        public string Header { get; set; }

        public override int GetType()
        {
            return TypeHeader;
        }
    }
}
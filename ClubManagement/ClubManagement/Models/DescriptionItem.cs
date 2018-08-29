namespace ClubManagement.Models
{
    public class DescriptionItem : EventItem
    {
        public UserLoginEventModel EventModel { get; set; }

        public override int GetType()
        {
            return TypeDescription;
        }
    }
}
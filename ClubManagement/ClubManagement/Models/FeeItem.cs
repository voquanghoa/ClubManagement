namespace ClubManagement.Models
{
    public abstract class FeeItem
    {
        public const int TypeTimeHeader = 1;

        public const int TypeDetail = 2;

        public new abstract int GetType();
    }
}
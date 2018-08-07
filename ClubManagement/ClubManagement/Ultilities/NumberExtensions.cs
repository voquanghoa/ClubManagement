namespace ClubManagement.Ultilities
{
    public static class NumberExtensions
    {
        public static string ToNumberFormat(this int name) => name.ToString("N0");
    }
}
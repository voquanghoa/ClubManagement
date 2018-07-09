using Android.Views;

namespace ClubManagement.Interfaces
{
    public interface IItemClickListener
    {
        void OnClick(View view, int position);
    }
}
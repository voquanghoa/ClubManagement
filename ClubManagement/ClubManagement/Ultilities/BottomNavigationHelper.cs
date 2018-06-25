using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Util;
using Java.Lang;

namespace ClubManagement.Ultilities
{
    public static class BottomNavigationHelper
    {
        public static void RemoveShiftMode(BottomNavigationView bottomNavigationView)
        {
            var bottomNavigationMenuView = (BottomNavigationMenuView)bottomNavigationView.GetChildAt(0);
            try
            {
                var shiftingMode = bottomNavigationMenuView.Class.GetDeclaredField("mShiftingMode");
                shiftingMode.Accessible = true;
                shiftingMode.SetBoolean(bottomNavigationMenuView, false);
                shiftingMode.Accessible = false;
                for (var i = 0; i < bottomNavigationMenuView.ChildCount; i++)
                {
                    var item = (BottomNavigationItemView)bottomNavigationMenuView.GetChildAt(i);
                    item.SetShiftingMode(false);
                    item.SetChecked(item.ItemData.IsChecked);
                }
            }
            catch (NoSuchFieldException)
            {
                Log.Error("ERROR NO SUCH FIELD", "Unable to get shift mode field");
            }
            catch (IllegalAccessException)
            {
                Log.Error("ERROR ILLEGAL ALG", "Unable to change value of shift mode");
            }
        }
    }
}

using Android.App;
using Android.OS;
using ClubManagement.Activities.Base;

namespace ClubManagement.Activities
{
    [Activity(Label = "CreateFeeActivity")]
    public class CreateFeeActivity : CreateOrEditFeeActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using ClubManagement.Adapters;
using ClubManagement.Ultilities;

namespace ClubManagement.Activities
{
    [Activity(Label = "ChooseFeeGroupActivity")]
    public class ChooseFeeGroupActivity : Activity
    {
        [InjectView(Resource.Id.recyclerView1)]
        private RecyclerView recyclerView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ActivityChooseFeeGroup);

            Cheeseknife.Inject(this);

            recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            var adapter = new FeeGroupAdapter(AppConstantValues.FeeGrooups);

            adapter.ItemClick += Adapter_ItemClick;

            recyclerView.SetAdapter(adapter);
        }

        private void Adapter_ItemClick(object sender, ClickEventArgs e)
        {
            if (sender is int id)
            {
                SetResult(Result.Ok, new Intent().PutExtra("Id", id));
                Finish();
            }
        }
    }
}
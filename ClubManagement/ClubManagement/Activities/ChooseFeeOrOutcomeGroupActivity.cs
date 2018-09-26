
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using ClubManagement.Adapters;
using ClubManagement.Ultilities;

namespace ClubManagement.Activities
{
    [Activity(Label = "ChooseFeeGroupActivity")]
    public class ChooseFeeOrOutcomeGroupActivity : Activity
    {
        [InjectView(Resource.Id.recyclerView1)]
        private RecyclerView recyclerView;

        [InjectView(Resource.Id.tvTitle)]
        private TextView tvTitle;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ActivityChooseFeeGroup);

            Cheeseknife.Inject(this);

            recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            var adapter = new FeeOrOutcomeGroupAdapter(AppConstantValues.FeeGrooups);

            adapter.ItemClick += Adapter_ItemClick;

            recyclerView.SetAdapter(adapter);

            var title = Intent.GetStringExtra("Title");

            if (!string.IsNullOrEmpty(title)) tvTitle.Text = title;
        }

        private void Adapter_ItemClick(object sender, ClickEventArgs e)
        {
            if (sender is string id)
            {
                SetResult(Result.Ok, new Intent().PutExtra("Id", id));
                Finish();
            }
        }
    }
}
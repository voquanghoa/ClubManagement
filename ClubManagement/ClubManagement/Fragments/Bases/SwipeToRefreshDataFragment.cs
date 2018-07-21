using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;

namespace ClubManagement.Fragments.Bases
{
    public abstract class SwipeToRefreshDataFragment<T> : Fragment
    {
        protected T data;

        protected abstract SwipeRefreshLayout SwipeRefreshLayout { get; }

        private object locker = new object();

        private bool isLoadingData = false;

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            SwipeRefreshLayout.Refresh += HandleRefresh;
            UpdateViewData();
        }

        private void HandleRefresh(object sender, EventArgs e)
        {
            UpdateViewData();
        }

        protected async void UpdateViewData()
        {
            if (!isLoadingData)
            {
                SwipeRefreshLayout.Refreshing = true;
                isLoadingData = true;
                this.data = await Task.Run(() => QueryData());
                isLoadingData = false;
                lock (locker)
                {
                    if (Context is Activity activity)
                    {
                        activity.RunOnUiThread(() =>
                        {
                            if (View != null)
                            {
                                DisplayData(this.data);
                                SwipeRefreshLayout.Refreshing = false;
                            }
                        });
                    }
                }
            }
        }

        protected abstract T QueryData();

        protected abstract void DisplayData(T data);
    }
}
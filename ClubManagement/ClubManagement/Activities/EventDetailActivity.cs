using System;
using Android.OS;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Ultilities;
using Newtonsoft.Json;
using System.Linq;
using Android.Content;
using Android.Views;
using ClubManagement.Fragments;
using FragmentActivity = Android.Support.V4.App.FragmentActivity;
using Android.App;
using System.Globalization;

namespace ClubManagement.Activities
{
    [Activity(Label = "EventDetailActivity", Theme = "@style/AppTheme")]
    public class EventDetailActivity : FragmentActivity
    {
        [InjectView(Resource.Id.tvMonth)]
        private TextView tvMonth;

        [InjectView(Resource.Id.tvDate)]
        private TextView tvDate;

        [InjectView(Resource.Id.tvTitle)]
        private TextView tvTitle;

        [InjectView(Resource.Id.tvStatus)]
        private TextView tvStatus;

        [InjectView(Resource.Id.tvUsers)]
        private TextView tvUsers;

        [InjectOnClick(Resource.Id.tvUsers)]
        private void UsersClick(object s, EventArgs e)
        {
            var intent = new Intent(this, typeof(GuestsActivity));
            intent.PutExtra("NumberPeople", tvUsers.Text);
            intent.PutExtra("EventDetail", content);

            StartActivity(intent);
        }

        [InjectView(Resource.Id.tvTime)]
        private TextView tvTime;

        [InjectView(Resource.Id.tvAddress)]
        private TextView tvAddress;

        [InjectOnClick(Resource.Id.tvAddress)]
        private void AddressClick(object s, EventArgs e)
        {
            var intent = new Intent(this, typeof(MemberLocationActivity));
            intent.PutExtra("EventDetail", content);
            intent.PutExtra("NumberPeople", tvUsers.Text);

            StartActivity(intent);
        }

        [InjectView(Resource.Id.tvDescription)]
        private TextView tvDescription;

        [InjectOnClick(Resource.Id.btnBack)]
        private void Back(object s, EventArgs e)
        {
            Finish();
        }

        [InjectView(Resource.Id.btnOption)]
        private ImageButton btnOption;

        [InjectOnClick(Resource.Id.btnOption)]
        private void Option(object s, EventArgs e)
        {
            if (s is View view)
            {
                var menuRes = eventDetail.TimeStart.Date.CompareTo(DateTime.Now.Date) < 0
                    ? Resource.Menu.DetailEventOptionForPastEvent
                    : Resource.Menu.DetailEventOption;

                var popupMenu = view.CreatepopupMenu(menuRes);
                popupMenu.MenuItemClick += PopupMenu_MenuItemClick; ;
                popupMenu.Show();
            }
        }

        private void PopupMenu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.edit:
                    var intent = new Intent(this, typeof(EditEventActivity));

                    intent.PutExtra("EventDetail", content);

                    StartActivity(intent);
                    break;
                case Resource.Id.delete:
                    this.ShowConfirmDialog(Resource.String.title_confirm_delete,
                        Resource.String.message_confirm_delete,
                        () => 
                        {
                            var processDialog = new ProgressDialog(this);

                            processDialog.Show();

                            this.DoRequest(async () =>
                            {
                                await EventsController.Instance.Delete(eventDetail);

                                processDialog.Dismiss();

                                Finish();
                            });
                        }).Show();
                    break;
            }
        }

        private UserEventsController userEventsController = UserEventsController.Instance;

        private UnjoinEventFragment unjoinEventFragment = new UnjoinEventFragment();

        private string userId = AppDataController.Instance.UserId;

        private UserLoginEventModel eventDetail;

        private bool currentIsJoined;

        private string content;

        public EventDetailActivity()
        {
            unjoinEventFragment.NotGoing += UnjoinEventFragment_NotGoing;
        }

        private void UnjoinEventFragment_NotGoing(object sender, EventArgs e)
        {
            if (sender is Dialog dialog)
            {
                UpdateUserEvents(!currentIsJoined);

                dialog.Dismiss();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EventDetail);

            Cheeseknife.Inject(this);

            if (!AppDataController.Instance.IsAdmin)
            {
                btnOption.Visibility = ViewStates.Gone;
            }

            content = Intent.GetStringExtra("EventDetail");

            eventDetail = JsonConvert.DeserializeObject<UserLoginEventModel>(content);
            
            tvTitle.Text = eventDetail.Title;
            tvDescription.Text = eventDetail.Description;
            tvTime.Text = "Time";
            tvAddress.Text = eventDetail.Place;
            tvMonth.Text = eventDetail.TimeStart.ToString("MMM", CultureInfo.InvariantCulture);
            tvDate.Text = eventDetail.TimeStart.Day.ToString();
            tvUsers.Text = "0";

            var count = 0;

            this.DoRequest(() => count = userEventsController.Values
                    .Where(x => x.EventId == eventDetail.Id).Count()
                , () => tvUsers.Text = count.ToString());

            currentIsJoined = eventDetail.IsJoined;

            if (eventDetail.TimeStart < DateTime.Now)
            {
                tvStatus.Text = !eventDetail.IsJoined
                    ? Resources.GetString(Resource.String.event_happened)
                    : Resources.GetString(Resource.String.you_joined);
            }
            else
            {
                tvStatus.ChangeTextViewStatus(currentIsJoined);

                tvStatus.Click += (s, e) =>
                {
                    if (!currentIsJoined)
                    {
                        UpdateUserEvents(!currentIsJoined);
                    }
                    else
                    {
                        unjoinEventFragment.Show(SupportFragmentManager, null);
                    }
                };
            }
        }

        private void UpdateUserEvents(bool isJoined)
        {
            currentIsJoined = isJoined;
            tvStatus.ChangeTextViewStatus(isJoined);

            this.DoRequest(() =>
            {
                if (isJoined)
                {
                    userEventsController.Add(new UserEventModel()
                    {
                        EventId = eventDetail.Id,
                        UserId = userId
                    });
                }
                else
                {
                    var userEvent = userEventsController.Values
                        .First(x => x.EventId == eventDetail.Id && x.UserId == userId);

                    userEventsController.Delete(userEvent);
                }
            }, () => { });
        }
    }
}
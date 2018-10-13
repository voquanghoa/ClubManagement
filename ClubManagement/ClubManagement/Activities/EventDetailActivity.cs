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
using Android.Graphics;
using Square.Picasso;
using Android.Runtime;
using System.Threading.Tasks;

namespace ClubManagement.Activities
{
    [Activity(Label = "EventDetailActivity", Theme = "@style/AppTheme")]
    public class EventDetailActivity : FragmentActivity
    {
        [InjectView(Resource.Id.tvMonth)]
        private TextView tvMonth;

        [InjectView(Resource.Id.imgViewPhoto)]
        private ImageView imgViewPhoto;

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
            intent.PutExtra("EventDetail", content);
            intent.PutExtra("NumberPeople", tvUsers.Text.Split(' ').FirstOrDefault());

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
            intent.PutExtra("NumberPeople", tvUsers.Text.Split(' ').FirstOrDefault());

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
                var menuRes = eventDetail.TimeEnd <= DateTime.Now.Date
                    ? Resource.Menu.DetailEventOptionForPastEvent
                    : Resource.Menu.DetailEventOption;

                var popupMenu = view.CreatepopupMenu(menuRes);
                popupMenu.MenuItemClick += PopupMenu_MenuItemClick;
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

                    StartActivityForResult(intent, 0);
                    break;
                case Resource.Id.delete:
                    this.ShowConfirmDialog(Resource.String.title_confirm_delete,
                        Resource.String.message_confirm_delete,
                        () => 
                        {
                            var processDialog = new ProgressDialog(this);

                            processDialog.Show();

                            this.DoRequest(EventsController.Instance.Delete(eventDetail), () =>
                            {
                                processDialog.Dismiss();
								this.ShowMessage(Resource.String.delete_event_success);

                                if (eventDetail.TimeEnd > DateTime.Now.Date)
                                {
                                    var notificationsController = NotificationsController.Instance;

                                    notificationsController.UpdateNotificationAsync(new NotificationModel()
                                    {
                                        Message = $"Event {eventDetail.Title} was deleted",
                                        Type = AppConstantValues.NotificationDeleteEvent,
                                        TypeId = eventDetail.Id,
                                        LastUpdate = DateTime.Now
                                    });

                                    notificationsController
                                        .PushNotifyAsync(AppConstantValues.NotificationDeleteEvent, eventDetail.Title);
                                }

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

            if (eventDetail.TimeEnd > DateTime.Now)
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

            Update();
        }

        private void Update()
        {
            var split = string.IsNullOrEmpty(eventDetail.Place)
                    | string.IsNullOrEmpty(eventDetail.Address)
                ? ""
                : "\n";

            tvTitle.Text = eventDetail.Title;
            tvDescription.Text = eventDetail.Description;
            tvTime.Text = "Time";
            tvAddress.Text = $"{eventDetail.Place}{split}{eventDetail.Address}";
            tvMonth.Text = eventDetail.TimeStart.ToString("MMM", CultureInfo.InvariantCulture).ToUpper();
            tvDate.Text = eventDetail.TimeStart.Day.ToString();
            tvUsers.Text = "0 Going";

            if (!string.IsNullOrEmpty(eventDetail.ImageUrl))
            {
                Picasso.With(this).Load(eventDetail.ImageUrl).Into(imgViewPhoto);
            }

            tvAddress.PaintFlags = tvAddress.PaintFlags | PaintFlags.UnderlineText;

            var count = 0;

            this.DoRequest(Task.Run(() => count = userEventsController.Values
                    .Where(x => x.EventId == eventDetail.Id).Count())
                , () => tvUsers.Text = $"{count} Going");

            currentIsJoined = eventDetail.IsJoined;

            if (eventDetail.TimeEnd <= DateTime.Now)
            {
                tvStatus.Text = Resources.GetString(Resource.String.this_event_happened);
            }
            else
            {
                tvStatus.ChangeTextViewStatus(currentIsJoined);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                content = data.GetStringExtra("EventDetail");
                eventDetail = JsonConvert.DeserializeObject<UserLoginEventModel>(content);

                Update();
            }
        }

        private void UpdateUserEvents(bool isJoined)
        {
            currentIsJoined = isJoined;
            tvStatus.ChangeTextViewStatus(isJoined);

            this.DoRequest(Task.Run(() =>
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
            }));
        }
    }
}
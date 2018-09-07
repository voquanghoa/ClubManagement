using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using ClubManagement.Controllers;
using ClubManagement.Adapters;
using Android.Support.V7.Widget;
using ClubManagement.Models;
using Android.Views.Animations;
using ClubManagement.Ultilities;
using Newtonsoft.Json;

namespace ClubManagement.Activities
{
    [Activity(Label = "GuestActivity")]
    public class GuestsActivity : Activity
    {
        [InjectOnClick(Resource.Id.btnBack)]
        private void Back(object s, EventArgs e)
        {
            Finish();
        }

        [InjectView(Resource.Id.tvUsers)]
        private TextView tvUsers;

        private UserEventsController userEventsController = UserEventsController.Instance;

        private UsersController usersController = UsersController.Instance;

        private GuestsAdapter adapter = new GuestsAdapter();

        private RecyclerView recyclerView;

        private List<GuestModel> guests = new List<GuestModel>();

        public bool displayPersons = true;

        private EventModel eventDetail;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityGuests);
            Cheeseknife.Inject(this);

            Init();
        }

        private void Init()
        {
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewGuests);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            recyclerView.SetAdapter(adapter);

            adapter.Guests = guests;

            var layoutAnimation = AnimationUtils.LoadLayoutAnimation(this, Resource.Animation.layout_animation_fall_down);
            recyclerView.LayoutAnimation = layoutAnimation;

            tvUsers.Text = Intent.GetStringExtra("NumberPeople") + " Going";

            var content = Intent.GetStringExtra("EventDetail");
            eventDetail = JsonConvert.DeserializeObject<UserLoginEventModel>(content);

            UpdateData();
        }

        private void UpdateData()
        {
            this.DoRequest(() =>
            {
                var letters = new List<string>();

                guests = userEventsController.Values.Where(x => x.EventId == eventDetail.Id)
                    .Join(usersController.Values,
                        x => x.UserId,
                        y => y.Id,
                        (x, y) => y)
                    .OrderBy(x => x.Name)
                    .Select(x =>
                    {
                        var guestModel = new GuestModel() { Avatar = x.Avatar, Name = x.Name };
                        var firstLetter = x.Name.FirstOrDefault().ToString();

                        if (guestModel.IsHeadLetter = letters.Any(y => y.Equals(firstLetter.ToUpper())))
                        {
                            guestModel.IsHeadLetter = false;
                        }
                        else
                        {
                            guestModel.IsHeadLetter = true;
                            letters.Add(firstLetter.ToUpper());
                        }

                        return guestModel;
                    })
                    .ToList();
            }, () =>
            {
                adapter.Guests = guests;
                recyclerView.ScheduleLayoutAnimation();
            });
        }
    }
}
using System;
using System.Linq;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using ClubManagement.Adapters;

namespace ClubManagement.Fragments
{
    public class PersonGoTimesFragment : Fragment
    {
        private View view;

        private UserEventsController userEventsController = UserEventsController.Instance;

        private UsersController usersController = UsersController.Instance;

        private MapsController mapsController = MapsController.Instance;

        private EventModel eventDetail;

        public bool displayPersons = true;

        public event EventHandler<PersonGoTimeClickEventArgs> PersonGoTimeClick;

        public event EventHandler DisplayPersonsClick;

        public PersonGoTimesFragment()
        {
            
        }

        public PersonGoTimesFragment(EventModel eventDetail)
        {
            this.eventDetail = eventDetail;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (displayPersons)
            {
                view = inflater.Inflate(Resource.Layout.FragmentPersonGoTimes, container, false);

                InitDisplayPersons();
            }
            else
            {
                view = new ImageButton(this.Context);

                view.SetBackgroundResource(Resource.Drawable.icon_list_person);

                view.Click += DisplayPersons_Click;
            }

            return view;
        }

        private void InitDisplayPersons()
        {
            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewPersonsGoTime);

            recyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

            var q = userEventsController.Values.Where(x => x.EventId == eventDetail.Id).ToList();

            var personGoTimes = userEventsController.Values.Where(x => x.EventId == eventDetail.Id)
                .Join(usersController.Values,
                    x => x.UserId,
                    y => y.Id,
                    (x, y) => y)
                .Select(x =>
                {
                    var personGoTimeModel = new PersonGoTimeModel()
                    {
                        Name = x.Name,
                        GoTime = mapsController.GetGoTime(x.Latitude, x.Longitude, eventDetail.Latitude, eventDetail.Longitude),
                        Selected = false,
                        Latitude = x.Latitude,
                        Longitude = x.Longitude
                    };

                    return personGoTimeModel;
                }).ToList();

            var adapter = new PersonGoTimesAdapter(personGoTimes);

            recyclerView.SetAdapter(adapter);

            var previousPosition = 0;

            adapter.ItemClick += (s, e) =>
            {
                personGoTimes[previousPosition].Selected = false;
                personGoTimes[e.Position].Selected = true;
                previousPosition = e.Position;
                adapter.NotifyDataSetChanged();

                PersonGoTimeClick.Invoke(s, new PersonGoTimeClickEventArgs()
                {
                    Latitude = personGoTimes[e.Position].Latitude,
                    Longitude = personGoTimes[e.Position].Longitude,
                });
            };

            view.FindViewById<ImageButton>(Resource.Id.imageButtonBack).Click += DisplayPersons_Click;

            view.SetBackgroundColor(Android.Graphics.Color.Argb(200, 195, 207, 219));
        }

        private void DisplayPersons_Click(object sender, EventArgs e)
        {
            displayPersons = !displayPersons;

            DisplayPersonsClick.Invoke(sender, e);
        }
    }
}
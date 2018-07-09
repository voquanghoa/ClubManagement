using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using ClubManagement.Controllers;
using ClubManagement.Models;
using Android.Preferences;
using ClubManagement.Adapters;

namespace ClubManagement.Fragments
{
    public class PersonGoTimesFragment : Fragment
    {
        private View view;

        private UserEventsController userEventsController = UserEventsController.Instance;

        private string eventId;

        public PersonGoTimesFragment()
        {
            
        }

        public PersonGoTimesFragment(string eventId)
        {
            this.eventId = eventId;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.FragmentPersonGoTimes, container, false);

            Init();

            return view;
        }

        private void Init()
        {
            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewPersonsGoTime);

            recyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

            var personGoTimes = userEventsController.Values.Where(x => x.EventId == eventId).Select(x =>
            {
                var personGoTimeModel = new PersonGoTimeModel()
                {
                    //Name = 
                };

                return personGoTimeModel;
            }).ToList();

            recyclerView.SetAdapter(new PersonGoTimesAdapter(personGoTimes));
        }
    }
}
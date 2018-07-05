using System.Collections.Generic;
using Android.Support.V4.App;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace ClubManagement.CustomAdapters
{
    public class PagerAdapter : FragmentPagerAdapter
    {
        private readonly List<Fragment> fragments = new List<Fragment>();

        private readonly List<string> titles = new List<string>();

        public PagerAdapter(FragmentManager fm) : base(fm)
        {
        }

        public override int Count => fragments.Count;

        public override Fragment GetItem(int position)
        {
            return fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(titles[position]);
        }

        public void AddFramgent(Fragment fragment, string title)
        {
            fragments.Add(fragment);
            titles.Add(title);
        }
    }
}
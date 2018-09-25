using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ClubManagement.Models
{
    public class FeeGroupModel
    {
        public string Id { set; get; }

        public int ImageId { set; get; }

        public int TitleId { set; get; }
    }
}
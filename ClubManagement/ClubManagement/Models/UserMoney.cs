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
using Newtonsoft.Json;

namespace ClubManagement.Models
{
    class UserMoney
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("moneyId")]
        public int MoneyId { get; set; }
    }
}
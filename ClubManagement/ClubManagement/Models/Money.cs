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
    class Money
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}
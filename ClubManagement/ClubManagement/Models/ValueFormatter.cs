using System;

using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;
using MikePhil.Charting.Util;

namespace ClubManagement.Models
{
    public class ValueFormatter : Java.Lang.Object, IValueFormatter
    {
        public String GetFormattedValue(float value, Entry entry, int dataSetIndex, ViewPortHandler viewPortHandler)
        {
            return value.ToString();
        }
    }
}
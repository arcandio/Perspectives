using UnityEngine;
using System.Collections;

/// <summary>
/// A less precise cousin of DateTime, with a wider range and negative values.
/// </summary>
public class TimelineDate {
    /// <summary>
    /// number of 1 minute intervals from year zero. Range of about 5 trillion years, +/- 2,500,000,000,000 max/min.
    /// </summary>
	long ticks = 0;
    const float ticksPerYear = 365.25f * 24f * 60f;
    const float ticksPerDay = 24f * 60f;
    const float ticksPerHour = 60f;
    const int[] monthStandardYear = new int[] {31, 28, 31, 30, 31, 31, 30, 31, 30, 31, 30, 31};
    const int[] monthLeapYear = new int[] { 31, 29, 31, 30, 31, 31, 30, 31, 30, 31, 30, 31 };

    public TimelineDate Now ()
    {
        TimelineDate now = new TimelineDate();
        now.ticks = System.DateTime.Now.Ticks / 10000000;
        return now;
    }

    public int Year ()
    {
        return (int)(ticks / ticksPerYear);
    }
    public int MonthInteger ()
    {

    }
    public int DayOfMonth ()
    {
        return (int)(ticks / ticksPerYear);
    }
    public int DayOfYear()
    {

    }
    public int Hour ()
    {

    }
    public int Minute ()
    {

    }

    public string ToString()
    {
        
    }
}

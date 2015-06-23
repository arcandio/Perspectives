using UnityEngine;
using System.Collections;

/// <summary>
/// A less precise cousin of DateTime, with a wider range and negative values.
/// </summary>
[System.Serializable]
public class TimelineDateTicks {
    /// <summary>
    /// number of 1 minute intervals from year zero. Range of about 5 trillion years, +/- 2,500,000,000,000 max/min.
    /// </summary>
	public long ticks = 0;

	const string sep = "/";
    const float ticksPerYear = 365.25f * 24 * 60 * 60;
	const int ticksPerDay = 24 * 60 * 60;
    const int ticksPerHour = 60 * 60;
	const int ticksPerMinute = 60;
    readonly int[] monthStandardYear = {31, 28, 31, 30, 31, 31, 30, 31, 30, 31, 30, 31};
    readonly int[] monthLeapYear = {31, 29, 31, 30, 31, 31, 30, 31, 30, 31, 30, 31 };
	readonly string[] monthNames = {
		"January",
		"February",
		"March",
		"April",
		"May",
		"June",
		"July",
		"August",
		"September",
		"October",
		"November",
		"December"
	};
	int[] monthsCurrent;
	readonly string[] dayNames = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};

	public int year;
	public int dayOfYear;
	public int hour;
	public int minute;
	public int second;

	public int monthInteger;
	public string monthName;
	public int dayOfMonth;
	public string dayName;
	public int weekOfYear;
	public int weekOfMonth;
	public bool isLeapYear;
	public int dayOfWeek;

    static public TimelineDateTicks Now ()
    {
		TimelineDateTicks now = new TimelineDateTicks(System.DateTime.Now.Ticks / 10000000); // 10 million ticks a second
        return now;
    }

	public TimelineDateTicks(int yearInput, int monthInput, int dayOfMonthInput)
	{
		year = yearInput;
		monthInteger = monthInput;
		dayOfMonth = dayOfMonthInput;
		CheckForLeapYear ();
		GetDayOfYear ();
		ticks = (long)(year * ticksPerYear + dayOfYear * ticksPerDay);
		// figure out the month name
		monthName = monthNames [monthInteger-1];
		
		// figure out the day name
		//http://en.wikipedia.org/wiki/Determination_of_the_day_of_the_week#Months_table
		dayOfWeek = (dayOfMonth + monthInteger + (year % 100) + ((year % 100) / 4) + (year / 100) + 2) % 7;

		dayName = dayNames[dayOfWeek];
		
		// figure out the week of the year
		weekOfYear = (int)(dayOfYear / 7);
	}

	public TimelineDateTicks (long initialTicks)
	{
		// Calculate all base values
		ticks = initialTicks;
		long remainder = ticks;

		year = (int)(ticks / ticksPerYear);
		remainder -= (long)(year * ticksPerYear); // Ticks left in this year
		//year += 1; //because the datetime is offset 1 year?

		dayOfYear = (int)(remainder / ticksPerDay);
		remainder -= (long)(dayOfYear * ticksPerDay); // ticks left in this day

		hour = (int)(remainder / ticksPerHour);
		remainder -= (long)(hour * ticksPerHour); // ticks left in this hour

		minute = (int)(remainder / ticksPerMinute);
		remainder -= (long)(minute * ticksPerMinute); // ticks left in this minute

		second = (int)(remainder);

		// is it a leap year?
		CheckForLeapYear ();



		// figure out what month the day is in
		int daysPassed = 0;
		foreach (int daysInCurrentMonth in monthsCurrent) {
			++monthInteger;
			int startOfMonth = daysPassed;
			int endOfMonth = daysPassed + daysInCurrentMonth;
			if (dayOfYear > startOfMonth && dayOfYear < endOfMonth)
			{
				dayOfMonth = dayOfYear - daysPassed;
				break;
			}
			else {
				daysPassed = endOfMonth;
			}
		}

		// figure out the month name
		monthName = monthNames [monthInteger];

		// figure out the day name
		dayOfWeek = (dayOfMonth + monthInteger + year % 100 + (year % 100) / 4 + year / 100) % 7;
		dayName = dayNames[dayOfWeek];

		// figure out the week of the year
		weekOfYear = (int)(dayOfYear / 7);
	}

    public string ToString()
    {
		return "" + year + sep + monthInteger + sep + dayOfMonth + " " + hour + ":" + minute;
    }

	void CheckForLeapYear () {
		if (year % 4 != 0) {
			isLeapYear = false;
		} else if (year % 100 != 0) {
			isLeapYear = true;
		} else if (year % 400 != 0) {
			isLeapYear = false;
		} else {
			isLeapYear = true;
		}

		// which set of months to use?
		if (isLeapYear) {
			monthsCurrent = monthLeapYear;
		} else {
			monthsCurrent = monthStandardYear;
		}
	}
	void GetDayOfYear () {
		// we have a day of the month, whether or not it's a leap year, and the month.
		int daysPassed = 0;
		for (int i = 0; i < monthInteger-1; i++) {
			daysPassed += monthsCurrent[i];
		}
		dayOfYear = daysPassed + dayOfMonth;
	}
}

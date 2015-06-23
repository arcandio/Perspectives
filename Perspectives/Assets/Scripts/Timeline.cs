using UnityEngine;
using System.Collections;

[System.Serializable]
public class TimelineDate {

    public int year;
    public short month;
    public short day;

    public short hour;
    public short minute;
    public short second;

    private TimelineDate()
    {
        year = 0;
        month = 0;
        day = 0;
        hour = 0;
        minute = 0;
        second = 0;
    }

    public TimelineDate(int year, short month, short day)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        Normalize();
    }

    public TimelineDate(int year, int month, int day, int hour, int minute, int second)
    {
        this.year = year;
        this.month = (short)month;
        this.day = (short)day;
        this.hour = (short)hour;
        this.minute = (short)minute;
        this.second = (short)second;
        Normalize();
    }
    public TimelineDate(string s)
    {
        string[] split = s.Split(new char[] { '.' });
        if (split.Length == 7)
        {
            this.year = int.Parse(split[1]);
            if (split[0] == "-")
            {
                this.year *= -1;
            }
            this.month = short.Parse(split[2]);
            this.day = short.Parse(split[3]);
            this.hour = short.Parse(split[4]);
            this.minute = short.Parse(split[5]);
            this.second = short.Parse(split[6]);
            Normalize();
        }
        else
        {
            Normalize();
        }
    }

    public string ToString()
    {
        Normalize();
        string sign = Mathf.Sign(year) == -1 ? "-" : "+";
        string output = "";
        output += sign;
        output += ".";
        output += Mathf.Abs(year).ToString("0000000000");
        output += ".";
        output += month.ToString("00");
        output += ".";
        output += day.ToString("00");
        output += ".";
        output += hour.ToString("00");
        output += ".";
        output += minute.ToString("00");
        output += ".";
        output += second.ToString("00");
        return output;
    }

    public static TimelineDate operator +(TimelineDate a, TimelineDate b)
    {
        return Arithmetic(a, b, 1);
    }
    public static TimelineDate operator -(TimelineDate a, TimelineDate b)
    {
        return Arithmetic(a, b, -1);
    }

    static TimelineDate Arithmetic(TimelineDate a, TimelineDate b, int sign)
    {
        int sec = a.second + b.second * sign;
        int min = a.minute + b.minute * sign;
        int hou = a.hour + b.hour * sign;
        int dow = a.DayOfYear() + b.DayOfYear() * sign;
        int da = 0;
        int mo = a.month + b.month * sign;
        int ye = a.year + b.year * sign;

        sec = MoveDigit(sec, 60, ref min);
        min = MoveDigit(min, 60, ref hou);
        hou = MoveDigit(hou, 24, ref da);
        dow = MoveDigit(dow, 365, ref ye);
        da = Calendar.GetDayOfMonth(ye, dow);
        mo = Calendar.GetMonthOfYear(ye, dow);

        TimelineDate ntd = new TimelineDate(ye, mo, da, hou, min, sec);
        return ntd;
    }

    void Normalize()
    {
        int sec = second;
        int min = minute;
        int hou = hour;
        int dow = DayOfYear();
        int da = 0;
        int mo = month;
        int ye = year;

        sec = MoveDigit(sec, 60, ref min);
        min = MoveDigit(min, 60, ref hou);
        hou = MoveDigit(hou, 24, ref da);
        dow = MoveDigit(dow, 365, ref ye);
        da = Calendar.GetDayOfMonth(ye, dow);
        mo = Calendar.GetMonthOfYear(ye, dow);

        year = ye;
        month = (short)mo;
        day = (short)da;

        hour = (short)hou;
        minute = (short)min;
        second = (short)sec;
    }

    static public int MoveDigit(int current, int mod, ref int higher )
    {
        int starting = current;
        int remainder = current % mod;
        higher += (int)((float)(starting - remainder)/(float)mod);

        if (remainder < 0)
        {
            higher -= 1;
            remainder += mod;
        }

        return remainder;
    }

    public string MonthName()
    {
        return Calendar.monthNames[month - 1];
    }
    public string DayName()
    {
        return Calendar.GetDayName(this);
    }
    public int DayOfYear()
    {
        return Calendar.GetDayOfYear(this);
    }

    static public TimelineDate Zero()
    {
        return new TimelineDate();
    }

    /*
    public long Ticks ()
    {
        long ticks = 0;
        ticks += (long)(year * 365.25 * 24 * 60 * 60);
        for (int i = 0; i <= month; i++)
        {
            if (i > 0)
            {
                ticks += (long)(Calendar.monthStandardYear[i-1] * 24 * 60 * 60);
            }
        }
        ticks += (long)(day * 24 * 60 * 60);
        ticks += (long)(hour * 60 * 60);
        ticks += (long)(minute * 60);
        ticks += (long)(second);
        //ticks *= (long)Mathf.Sign(year);
        //Debug.Log((long)Mathf.Sign(year)*ticks);
        return ticks;
    }
    */
}

static public class Calendar
{
    static readonly int[] monthStandardYear = { 31, 28, 31, 30, 31, 31, 30, 31, 30, 31, 30, 31 };
    static readonly int[] monthLeapYear = { 31, 29, 31, 30, 31, 31, 30, 31, 30, 31, 30, 31 };
    static public readonly string[] monthNames = {
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
    static public readonly string[] dayNames = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    static int[] t = new int[] { 0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4 };
    static bool CheckForLeapYear(int year)
    {
        bool isLeapYear = false;
        if (year % 4 != 0)
        {
            isLeapYear = false;
        }
        else if (year % 100 != 0)
        {
            isLeapYear = true;
        }
        else if (year % 400 != 0)
        {
            isLeapYear = false;
        }
        else
        {
            isLeapYear = true;
        }
        return isLeapYear;
    }
    static public int[] GetMonths(int year)
    {
        if (CheckForLeapYear(year))
        {
            return monthLeapYear;
        }
        else
        {
            return monthStandardYear;
        }
    }
    static public int GetDayOfYear (int year, int month, int day)
    {
        int daysPassed = 0;
        int[] months = GetMonths(year);

        for (int i = 0; i < month-1; i++ )
        {
            //Debug.Log(i);
            daysPassed += months[i];
        }
        daysPassed += day;
        return daysPassed;
    }
    static public int GetDayOfYear(TimelineDate tld)
    {
        return GetDayOfYear(tld.year, tld.month, tld.day);
    }
    static public int GetDayOfMonth(int year, int dayOfYear)
    {
        int daysPassed = 0;
        int dayOfMonth = 0;
        int[] months = GetMonths(year);
        for (int i = 0; i < 12; i++)
        {
            int thisMonth = months[i];
            int nextMonth = months[(i + 1) % 12];
            if (dayOfYear < (daysPassed + nextMonth))
            {
                dayOfMonth = dayOfYear - daysPassed;
                break;
            }
            else
            {
                daysPassed += thisMonth;
            }
        }
        return dayOfMonth;
    }
    static public int GetMonthOfYear(int year, int dayOfYear)
    {
        int daysPassed = 0;
        int month = 0;
        int[] months = GetMonths(year);
        for (int i = 0; i < 12; i++)
        {
            int thisMonth = months[i];
            int nextMonth = months[(i + 1) % 12];
            if (dayOfYear < (daysPassed + nextMonth))
            {
                month = i+1;
                break;
            }
            else
            {
                daysPassed += thisMonth;
            }
        }
        return month;
    }
    static public string GetDayName(int year, int month, int day)
    {
        string dayName = "";
        if (month < 3)
        {
            year--;
        }
        int dayOfWeek = (year + year / 4 - year / 100 + year / 400 + t[month - 1] + day) % 7;
        dayName = dayNames[dayOfWeek];
        return dayName;
    }
    static public string GetDayName(TimelineDate tld)
    {
        return GetDayName(tld.year, tld.month, tld.day);
    }
}
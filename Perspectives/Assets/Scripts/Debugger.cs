using UnityEngine;
using System.Collections;
using System;

//[ExecuteInEditMode]
public class Debugger : MonoBehaviour {
    public long ticks;
    public string startString;
    public TimelineDate start;
    public int dayOfYear;
    public int dayOfMonth;
    public int monthOfYear;
    public string monthName;
    public string dayName;
    public TimelineDate end;
    public TimelineDate age;
    public TimelineDate calculatedEnd;
    public string testPath = "derp.json";
    public FileData testFile;

	// Use this for initialization
	void Start () {
        testFile = FileData.NewFile();
        testFile.InitializeAtPath(testPath);
        testFile.SaveData();
        testFile = FileData.GetFile(testPath);
	}
	
	// Update is called once per frame
	void Update () {

        age = end - start;
        calculatedEnd = start + age;
        startString = start.ToString();

        dayOfYear = start.DayOfYear();
        dayOfMonth = Calendar.GetDayOfMonth(start.year, dayOfYear);
        monthOfYear = Calendar.GetMonthOfYear(start.year, dayOfYear);
        monthName = start.MonthName();
        dayName = start.DayName();

	}
}

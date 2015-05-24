using UnityEngine;
using System.Collections;

public class Debugger : MonoBehaviour {
	public string nowToDate = "";
	public TimelineDate now = TimelineDate.Now ();
	public int year = 2015;
	public int month =  5;
	public int dayOfMonth = 23;
	public TimelineDate today;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		now = TimelineDate.Now ();
		nowToDate = now.ToString ();
		today = new TimelineDate (year, month, dayOfMonth);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MeetingTabs : MonoBehaviour
{

  public Text[] meetingNames;
  public Text[] meetingTimes;
  public MainCode mc;
  public SliderCode sc;
  public int centered = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    // Centers
    public void TabClicked(int tabNum) {

    }

    public bool CheckMeeting(int tabNum) {
      return true;
    }

    public int GetRealIndex(int target) {
      int diff = centered - 2;
      return target + diff;
    }

    public void Center(int meetingNum) {
      centered = meetingNum;

      for(int x=0;x< meetingNames.Length;x++) {
        int realIndex = GetRealIndex(x);
        if(realIndex >= 0 && realIndex < mc.nextMeeting) {
          meetingNames[x].text = mc.meetings[realIndex].date;
          meetingTimes[x].text = mc.meetings[realIndex].time;
          if(realIndex == mc.nextMeeting-1) {
            meetingTimes[x].text = "Current Meeting";
          }
        }
        else {
          meetingNames[x].text = "";
          meetingTimes[x].text = "";
        }

      }
    }
}

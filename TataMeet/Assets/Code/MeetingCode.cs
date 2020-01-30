using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingCode : MonoBehaviour
{

    public int meetingNum = 0;
    public SliderCode sc;
    public List<Texture2D> ideaList = new List<Texture2D>();
    public bool availMeeting = true;
    public string date;
    public string time;
    public bool hasIdeas = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddIdea(Texture2D tex) {
      if(!hasIdeas) {
        hasIdeas = true;
        ideaList = new List<Texture2D>();
      }
      ideaList.Add(tex);


    }

    public List<Texture2D> GetIdeas() {
      return ideaList;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderCode : MonoBehaviour
{

    public MainCode mc;
    public List<Texture2D> images =  new List<Texture2D>();
    public int curImage = 0;
    public GameObject[] quads;
    public GameObject cam;
    public GameObject camParent;
    public float moveSpeed = 1.0f;

    public bool moving = false;
    float destX = 0.0f;

    public GameObject prevQuad;
    bool moveBack = false;

    public bool movingUp = false;
    public bool movingDown = false;
    public int moveStage = 0;

    public Animator ani;

    public GameObject prevMeeting;
    public GameObject nextMeeting;

    public bool firstImage = false;

    public List<int> meetingArr;

    public GameObject[] backQuads;

    public float speed = 0.0f;
    public bool pushing = false;
    bool centering = false;
    float centerSpeed = 2.5f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

          if(movingUp) {
            // //// NEW CODE
            // if(moveStage == 0) {
            //   if(cam.transform.position.y < 3.0f) {
            //     cam.transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
            //   }
            //   else {
            //     mc.LoadNew();
            //     cam.transform.position -= new Vector3(0.0f,2.5f,0.0f);
            //     moveStage = 1;
            //   }
            // }
            // else {
            //   if(!(cam.transform.position.y < 1.85f && cam.transform.position.y > 1.75f)) {
            //     cam.transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
            //   }
            //   else {
            //
            //     moveStage =0;
            //     movingUp = false;
            //   }
            // }
          }
          if(movingDown) {
            //// NEW CODE
            // if(moveStage == 0) {
            //   if(cam.transform.position.y > 0.5f) {
            //     cam.transform.Translate(-Vector3.up * Time.deltaTime * moveSpeed);
            //   }
            //   else {
            //     mc.LoadNew();
            //     cam.transform.position += new Vector3(0.0f,2.5f,0.0f);
            //     moveStage = 1;
            //   }
            // }
            // else {
            //   if(!(cam.transform.position.y < 1.85f && cam.transform.position.y > 1.75f)) {
            //     cam.transform.Translate(-Vector3.up * Time.deltaTime * moveSpeed);
            //   }
            //   else {
            //     moveStage =0;
            //     movingDown = false;
            //   }
            // }
          }

          if(pushing) {
            camParent.transform.position += new Vector3((speed/10.0f) * Time.deltaTime, 0.0f, 0.0f);
            if(camParent.transform.position.x < quads[0].transform.position.x) {
              ForceStop(false);
            }
            else if(camParent.transform.position.x > quads[images.Count -1].transform.position.x) {
              ForceStop(true);
            }
          }
          if(centering) {
            Vector3 destVec = new Vector3(quads[curImage].transform.position.x, camParent.transform.position.y, camParent.transform.position.z);
            camParent.transform.position = Vector3.MoveTowards(camParent.transform.position, destVec, Time.deltaTime * centerSpeed);
          }

    }

    void ResetMoving() {
      moving = false;
    }

    public void MoveTo(float dest) {
      //moving = true;
      destX = dest;
      //camParent.transform.position = new Vector3(destX, camParent.transform.position.y, camParent.transform.position.z);
      centering = true;
      centerSpeed = 10.0f;
      FillBack();
    }

    public void MoveUp() {
      //if(!moving) {
        // movingUp = true;
         moveStage =0;
        //ani.SetTrigger("MoveUp");
        Invoke("LoadNewz", 0.0f);

      //}
    }

    void LoadNewz() {
      mc.LoadNew();
    }

    public void MoveDown() {
      //if(!moving) {
        Debug.Log("SHOULD CALL TRIGGER");
        //movingDown = true;
        moveStage =0;
        //ani.SetTrigger("MoveDown");
        Invoke("LoadNewz", 0.0f);
      //}
    }

    public void PushRight(float sped) {
      pushing = true;
      speed = -sped;
      centering = false;
    }

    public void PushLeft(float sped) {
      pushing = true;
      speed = -sped;
      centering = false;
    }

    public void StopPush() {
      pushing = false;
      speed = 0.0f;
      int stopImage = FindNearestImage();
      curImage = stopImage;
      //MoveTo(quads[stopImage].transform.position.x);
      mc.curMeeting = GetMeetingNum();
      mc.UpdateHighlight();
      FillBack();
      centerSpeed = 2.5f;
      centering = true;
    }

    void ForceStop(bool end) {
      pushing = false;
      speed = 0.0f;
      int stopImage = FindNearestImage();
      if(end) {
        stopImage = images.Count-1;
      }
      curImage = stopImage;
      MoveTo(quads[stopImage].transform.position.x);
      mc.curMeeting = GetMeetingNum();
      mc.UpdateHighlight();
      FillBack();
      //centering = true;
    }

    public int FindNearestImage() {
      float nearest = Mathf.Abs(camParent.transform.position.x - quads[0].transform.position.x);
      int nearestIndex = 0;
      for(int x=0;x< quads.Length;x++) {
        float newDist = Mathf.Abs(camParent.transform.position.x - quads[x].transform.position.x);
        if(newDist < nearest) {
          nearest = newDist;
          nearestIndex = x;
        }
      }
      return nearestIndex;
    }

    public int GetMeetingNum() {
      for(int x=0;x<meetingArr.Count; x++) {
        if(curImage <= meetingArr[x]) {
          return x;
        }
      }
      return -1;
    }

    public int GetMeetingNum(int tar) {
      for(int x=0;x<meetingArr.Count; x++) {
        if(tar <= meetingArr[x]) {
          return x;
        }
      }
      return -1;
    }

    public int GetMeetingPos(int tar) {
      int meetNum = GetMeetingNum(tar);
      if(meetNum > 0) {
        return tar - (meetingArr[meetNum-1] + 1);
      }
      return tar;
    }

    public void FillBack() {
      int curMeet = GetMeetingNum();
      for(int x=0;x<backQuads.Length;x++) {
        backQuads[x].SetActive(false);
        if(GetMeetingNum(x) == curMeet) {
          backQuads[x].SetActive(true);
        }
      }
    }



    public void MoveLeft() {
      if(moving || movingUp || movingDown) {
        return;
      }
      centering = false;
      prevMeeting.SetActive(false);
      nextMeeting.SetActive(false);
      if(curImage > 0) {
        curImage--;
        ani.SetTrigger("MoveLeft");
        moving = true;
        Invoke("ResetMoving", 0.5f);
        mc.UpdateMeeting(GetMeetingNum());
        FillBack();
      }

      destX = quads[curImage].transform.position.x;
      camParent.transform.position = new Vector3(destX, camParent.transform.position.y, camParent.transform.position.z);
      moveBack = true;
      prevQuad =   quads[curImage];

    }

    public void JumpToMeeting(int meetz) {
      curImage =  meetingArr[meetz];
      Debug.Log("GOING TO IMAGE " + curImage);
      MoveTo(quads[curImage].transform.position.x);

    }

    public void MoveRight() {
      if(moving || movingUp || movingDown) {
        return;
      }
      centering = false;
      prevMeeting.SetActive(false);
      nextMeeting.SetActive(false);

      if(images.Count > (curImage + 1)) {
        curImage++;
        ani.SetTrigger("MoveRight");
        moving = true;
        Invoke("ResetMoving", 0.4f);
        mc.UpdateMeeting(GetMeetingNum());
        FillBack();
      }

      destX = quads[curImage].transform.position.x;
      camParent.transform.position = new Vector3(destX, camParent.transform.position.y, camParent.transform.position.z);
      //quads[curImage].transform.position += new Vector3(0.0f, 0.0f, -0.3f);
      moveBack = true;
      prevQuad =  quads[curImage];
      //ani.SetTrigger("MoveRight");
    }

    public void FillImages() {
      foreach(GameObject q in quads) {
        q.GetComponent<Renderer>().enabled = false;
      }
      for(int x =0;x<images.Count;x++) {
        quads[x].GetComponent<Renderer>().enabled = true;
        quads[x].GetComponent<Renderer>().material.mainTexture = images[x];
      }
    }

    public void Disable() {
      foreach(GameObject q in quads) {
        q.GetComponent<Renderer>().enabled = false;
      }
    }

    public void LoadImagesFirst(List<Texture2D> imList) {
      firstImage = true;
      LoadImages(imList);
    }

    public void LoadImages(List<Texture2D> imList) {
      //Check if Prev and Next meeting
      meetingArr.Clear();
      prevMeeting.SetActive(false);
      nextMeeting.SetActive(false);
      images = imList;
      int nextStart =-1;
      for(int x=0;x<mc.meetings.Length;x++) {
        int calcNum = (mc.meetings[x].ideaList.Count + nextStart );
        meetingArr.Add(calcNum);
        nextStart = calcNum;
      }
      FillImages();

      curImage = images.Count-1;
      if(firstImage) {
        curImage = 0;
        if(mc.curMeeting > 0) {
          //prevMeeting.SetActive(true);
        }
      }

      MoveTo(quads[images.Count-1].transform.position.x);
      FillBack();
      firstImage = false;

    }
}

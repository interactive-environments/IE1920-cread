using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainCode : MonoBehaviour
{

    public MeetingCode[] meetings;
    // 0= mainMenu, 1= spinMenu, 2= chooseMeeting, 3= meetingMenu
    public int stage = 0;
    public int curMeeting = 0;
    public int nextMeeting = 0;
    public SliderCode sc;
    public GameObject sliderObj;

    public Texture2D newMeetingIcon;

    // Menu Objects
    public GameObject mainMenu;
    public GameObject spinMenu;
    public GameObject chooseMeeting;
    public GameObject meetingStuff;

    public TimelineCode tl;

    // WhiteBoard Stuff
    public GameObject coverClose;
    public Animator coverAni;
    public GameObject coverOpen;

    public GameObject[] pasteImages;
    public int nextPaste = 0;
    public bool moveMode = false;

    // MovingShit
    public float pasteSpeed = 1.0f;
    public int dir = 0;

    public bool covered = true;
    public int barCount = 0;

    public GameObject whiteBoard;

    public GameObject camPopup;

    public bool moved = false;
    public GameObject arrows;
    public Transform bottomLimit;
    public Transform bottomLimit2;
    public Transform topLimit;
    public Transform sideLimit;
    public Transform sideLimit1;
    public Transform rawStart;

    public GameObject pasteHelp;

    public Animator loginAni;
    public Animator menuAni;

    public GameObject highLightTab;
    public Text[] meetingStrings;
    public Text[] meetingTimes;

    public int hoverMeeting = -1;
    public int hoverImage = -1;

    public int hoverSlide = 0;

    public GameObject popUpMouse;
    public Animator popUpAni;
    bool popping = false;

    bool prevScroll = false;

    public bool removing = false;
    int removingIndex = 0;
    bool removeDanger= false;
    bool removePressed = false;
    bool deleteWait = false;
    public GameObject deleteBar;
    public Animator deleteBarAni;
    bool pasteDanger = false;

    public MeetingTabs mt;

    public GameObject scrollPopup;
    public bool scrolled = false;
    bool doubleDanger = false;

    // Start is called before the first frame update
    void Start()
    {
      //InvokeRepeating("CheckBar", 0.0f, 1.0f);
      InvokeRepeating("CheckSlide", 0.0f, 0.2f);

    }

    // Update is called once per frame
    void Update()
    {
      if(moveMode) {
        if(Input.mousePosition.x < sideLimit.position.x && Input.mousePosition.x > sideLimit1.position.x
        && Input.mousePosition.y < topLimit.position.y && Input.mousePosition.y > bottomLimit.position.y)  {
          pasteImages[nextPaste].transform.position = Input.mousePosition;

        if(!moved) {
          if((Mathf.Abs(Input.GetAxis("Mouse X")) > 0.0f) || (Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.0f)) {
            moved = true;
            arrows.SetActive(false);
            pasteHelp.SetActive(true);
          }
        }

        //Scaling with scrolling
        if(Input.mouseScrollDelta.y > 2.0f) {
          if(pasteImages[nextPaste].transform.localScale.x < 1.5f) {
            pasteImages[nextPaste].transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
          }
        }
        else if(Input.mouseScrollDelta.y < -2.0f) {
            if(pasteImages[nextPaste].transform.localScale.x > 0.6f) {
              pasteImages[nextPaste].transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
            }
          }

        }

      }
      if((Mathf.Abs(Input.GetAxis("Mouse X")) > 0.0f) || (Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.0f)) {
        if(stage == 3 && popping) {
          //ShowBar();
          HideMousePop();
        }
      }
      if(Input.GetMouseButtonDown(0)) {
        Click();
      }
      if(removing) {
        if(Input.mousePosition.x < sideLimit.position.x && Input.mousePosition.x > sideLimit1.position.x
        && Input.mousePosition.y < topLimit.position.y && Input.mousePosition.y > bottomLimit.position.y)  {
          pasteImages[removingIndex].transform.position = Input.mousePosition;
        }
        if(Input.mousePosition.x > sideLimit.position.x && !deleteWait) {
          deleteWait = true;
          //deleteBar.SetActive(true);
          deleteBarAni.SetTrigger("Trigger");
          Invoke("CheckDelete", 1.0f);
        }


      }

    }

    void ClearPasteDanger() {
      pasteDanger = false;
    }

    void ShowMousePop() {
      popUpMouse.SetActive(true);
      //popUpAni.SetTrigger("Spawn");
      popping = true;
    }

    void HideMousePop() {
      popUpAni.SetTrigger("Gone");
      popping = false;
    }

    void CheckDelete() {
      if(deleteWait && Input.mousePosition.x > sideLimit.position.x) {
        removing = false;
        pasteImages[removingIndex].SetActive(false);
        deleteWait = false;
        //deleteBar.SetActive(false);
        deleteBarAni.SetTrigger("Hide");
        removeDanger = false;
      }
      else {
        deleteWait = false;
        deleteBarAni.SetTrigger("Calm");

      }
    }

    public void AddIdea(Texture2D tex) {
      if(stage == 3) {
        meetings[nextMeeting-1].AddIdea(tex);
          mt.Center(curMeeting);
          sc.LoadImages(GetAllImages());
          ShowBar();
      }
    }

    void ClearDouble() {
      doubleDanger = false;
    }

    public void RemoveImage(int rawNum) {
      if(rawNum >= nextPaste || moveMode || stage != 3 || removeDanger) {
        return;
      }
      doubleDanger = true;
      Invoke("ClearDouble", 1.0f);
      removingIndex = rawNum;
      //deleteBar.SetActive(true);
      deleteBarAni.SetTrigger("Show");
      removeDanger = true;
      removing = true;
      HideBar();
    }

    public void RawReleased(int rawNum) {
      if(rawNum == removingIndex) {
        removePressed = false;
      }
    }

    public void Left() {
      barCount = 0;
      if(stage == 3 && !covered) {
        sc.MoveLeft();
      }
    }

    public void Right() {
      barCount = 0;
      if(stage == 3 && !covered) {
        sc.MoveRight();
      }
    }

    public void Up() {
      if(stage == 3) {
        if(moveMode) {
          dir = 2;
        }
        else {
          if(covered) {
            ShowBar();
          }
          else {
            barCount = 0;
            if(sc.movingUp || sc.movingDown || sc.moving)  {
                //Debug.Log("WE GOT HERE");
              return;
            }
            if(nextMeeting > curMeeting + 1) {
              curMeeting++;
              //Debug.Log("WE GOT HERE");
              sc.MoveUp();
              //sc.LoadImages(meetings[curMeeting].GetIdeas());
              tl.SetDates(GetDateList());
            }
          }
        }
        }
    }

    public void RawPressed(int rawNum) {
      if(!covered) {
        return;
      }
      removingIndex = rawNum;
      removePressed = true;
      //Invoke("RemoveImage", 0.0f);
    }

    void CheckSlide() {
      float scrollSpeed = Input.mouseScrollDelta.x;
      if(scrollSpeed < -0.05f) {
        // if(!scrolled) {
        //   scrolled = true;
        //   scrollPopup.SetActive(false);
        // }
          //Right();
          //Debug.Log("GOING LEFT!!!");
          if(stage == 3 && !covered && (sc.curImage < sc.images.Count-1)) {
            sc.PushRight(scrollSpeed * 25.0f);
          }
        // Move camera with start speed
        prevScroll = true;
      }
      else if(scrollSpeed > 0.05f) {
        if(!scrolled) {
          scrolled = true;
          scrollPopup.SetActive(false);
        }
          //Left();
          //Debug.Log("GOING RIGHT!!!");
          if(stage == 3 && !covered) {
            sc.PushLeft(scrollSpeed * 25.0f);
          }
          prevScroll = true;
      }
      else if(prevScroll) {
        sc.StopPush();
        prevScroll = false;
      }
    }

    public bool CheckMeetingPoss() {
      if(nextMeeting > mt.GetRealIndex(hoverMeeting) && mt.GetRealIndex(hoverMeeting) >= 0) {
        return true;
      }
      return false;
    }

    public void LoadNew() {
      sc.LoadImages(meetings[curMeeting].GetIdeas());
    }

    public void LoadNext() {
      curMeeting++;
      sc.LoadImagesFirst(meetings[curMeeting].GetIdeas());
    }

    public void Login() {
      if(stage == 0) {
        loginAni.SetTrigger("Login");
        menuAni.SetTrigger("Login");
        Invoke("LoadStage3", 1.0f);
        //LoadStage3();
        //spinMenu.SetActive(true);
        ShowMousePop();

      }

    }

    public void Logout() {
      if(stage == 3 && !moveMode && !removing) {
        meetingStuff.SetActive(false);
        stage = 0;
        mainMenu.SetActive(true);
        menuAni.SetTrigger("Reset");
        loginAni.SetTrigger("Reset");
        whiteBoard.SetActive(false);
        foreach(GameObject raw in pasteImages) {
          raw.SetActive(false);
          raw.transform.position = rawStart.transform.position;
          raw.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
          nextPaste = 0;
        }
        covered = true;
        coverClose.SetActive(false);
        barCount = 0;
      }
    }

    public void Down() {
      if(stage == 3) {
        if(moveMode) {
          dir = -2;
        }
        else {
          if(covered) {
            ShowBar();
          }
          else {
            barCount =0;
            //if(sc.moving)  {


              //return;
              //Debug.Log("WE GOT HERE");
          //  }
            Debug.Log("WE GOT HERE");
            if(curMeeting > 0) {
              //Debug.Log("WE GOT HERE");
              curMeeting--;
              sc.MoveDown();
              //sc.LoadImages(meetings[curMeeting].GetIdeas());
              tl.SetDates(GetDateList());
            }
          }
        }
      }
    }

    public void LoadMeeting(int numz) {
      curMeeting = numz;
      UpdateHighlight();
      sc.JumpToMeeting(numz);
      //mt.Center(numz);
      //LoadNew();
    }

    public void Center() {
      if(stage == 3) {
        if(moveMode) {
          //dir = 0;
        }
      }
    }

    public void HideBar() {
      //coverClose.SetActive(true);
      coverAni.SetBool("Covered", true);
      //coverOpen.SetActive(false);
      covered = true;
      CancelInvoke("CheckBar");
      hoverImage = -1;
      hoverMeeting = -1;
    }

    public void ShowBar() {
      //coverClose.SetActive(false);
      //coverOpen.SetActive(true);
      coverAni.SetBool("Covered", false);
      covered = false;
      InvokeRepeating("CheckBar", 0.0f, 1.0f);
      hoverMeeting = -1;
    }

    void ClearDanger() {
      removeDanger = false;
    }


    public void Select() {
      Debug.Log("SELECT PRESSED");
    }

    public void Menu() {
      Debug.Log("MENU PRESSED");
      if(stage == 1) {
        stage = 0;
        spinMenu.SetActive(false);
        mainMenu.SetActive(true);
      }
      else if(stage == 2) {
        stage = 1;
        whiteBoard.SetActive(false);
        sliderObj.SetActive(false);
        spinMenu.SetActive(true);
        chooseMeeting.SetActive(false);
        covered = true;
        coverOpen.SetActive(false);
        coverClose.SetActive(false);
        barCount = 0;
      }
      else if(stage == 3 && !moveMode) {
        stage = 0;
        //chooseMeeting.SetActive(true);
        meetingStuff.SetActive(false);
        mainMenu.SetActive(true);
        //sliderObj.SetActive(false);
        //curMeeting = nextMeeting;
        //sc.LoadImages(GetMeetingImages());
        //tl.SetDates(GetDateList());
        whiteBoard.SetActive(false);
        foreach(GameObject raw in pasteImages) {
          raw.SetActive(false);
          raw.transform.position = rawStart.transform.position;
          nextPaste = 0;
        }
        spinMenu.SetActive(true);
        covered = true;
        //coverOpen.SetActive(false);
        coverClose.SetActive(false);
        barCount = 0;
      }
    }

    public void Click() {
      if(stage == 1) {
        LoadStage3();
      }
      else if(stage == 3) {

        if(covered) {
          if(!moveMode) {
            if(!removing) {
              //ShowBar();
            }
            else if(!doubleDanger) {
              removing = false;
              Invoke("ClearDanger", 1.0f);
              deleteBarAni.SetTrigger("Hide");
              //deleteBar.SetActive(false);
              //ShowBar();
            }

          }
          else if(!pasteDanger && nextPaste < pasteImages.Length-1) {
            pasteImages[nextPaste].GetComponent<Animator>().SetTrigger("Paste");
            nextPaste++;
            moveMode = false;
            Invoke("ClearDanger", 1.0f);
            //pasteImages[nextPaste].GetComponent<Animator>().SetTrigger("Paste");
            pasteHelp.SetActive(false);
            moved = true;
            arrows.SetActive(false);
          }

        }
        else {

          if(hoverSlide == 0 && Input.mousePosition.y > bottomLimit2.position.y) {
            HideBar();
          }
          if(!scrolled) {
            scrolled = true;
            scrollPopup.SetActive(false);
          }
        }
      }

    }

    public int GetImagePos(int buttonPos)  {
      return (sc.curImage - 2 ) + buttonPos;
    }

    bool CheckImagePoss() {
      // Check if meeting has hovered image
      if(hoverImage < 2) {
        if(sc.curImage >= (2- hoverImage)) {
          return true;
        }
      }
      else if(hoverImage > 2) {
        if(sc.images.Count > sc.curImage +  (hoverImage - 2)) {
          return true;
        }
      }
      else if(hoverImage ==2) {
        return true;
      }

      return false;

    }

    public void ImageEnter(int numz) {
      hoverImage = numz;
      hoverMeeting = -1;
    }

    public void ImageExit(int numz) {
      if(hoverImage == numz) {
        hoverImage = -1;
      }
    }

    public void MeetingEnter(int numz) {
      hoverMeeting = numz;
      hoverImage = -1;
      if(stage == 3 && covered && !moveMode && !removing) {
        if(!scrolled) {
          scrollPopup.SetActive(true);
        }
        ShowBar();
      }
    }

    public void MeetingExit(int numz) {
      if(hoverMeeting == numz) {
        hoverMeeting = -1;
      }
    }

    public void SlideEnter(int numz) {
      hoverSlide = numz;
    }
    public void SlideExit(int numz) {
        hoverSlide = 0;
    }

    public void ImageClicked(int numz) {
       hoverImage = numz;
      // Click();
      if(stage != 3 || covered || pasteDanger) {
        return;
      }
      if(meetings[curMeeting].hasIdeas) {
        if(!moveMode) {
          // Check if any meetings are hovered
          // Check if any images are hovered
          if(CheckImagePoss()) {
            pasteDanger = true;
            Invoke("ClearPasteDanger", 1.0f);
            MovePaste();
            hoverImage = -1;
          }
        }
      }
    }

    public void MeetingClicked(int numz) {
      hoverMeeting = numz;
      //Click();
      if(stage != 3 || covered) {
        return;
      }
      if(CheckMeetingPoss()) {
        LoadMeeting(mt.GetRealIndex(hoverMeeting));
      }
    }

    void LoadStage3() {
      mainMenu.SetActive(false);
      stage = 3;
      whiteBoard.SetActive(true);
      sliderObj.SetActive(true);
      nextMeeting++;
      curMeeting = nextMeeting - 1;
      meetings[curMeeting].date = System.DateTime.Now.ToString("dd MMM yyyy");
      meetings[curMeeting].time = System.DateTime.Now.ToString("HH:mm");

      mt.Center(curMeeting);


      UpdateHighlight();

      sc.LoadImages(GetAllImages());
      meetingStuff.SetActive(true);
      coverClose.SetActive(true);
      coverAni.SetBool("Covered", true);

      //HideBar();
    }

    public void UpdateHighlight() {
      //highLightTab.transform.position = new Vector3(meetingStrings[curMeeting].gameObject.transform.position.x,
        //highLightTab.transform.position.y, highLightTab.transform.position.z);
        mt.Center(curMeeting);

    }

    void CheckBar() {
      if(!covered) {
        barCount++;
        if(barCount > 5) {
          barCount = 0;
          //HideBar();
        }
      }

    }

    public void UpdateMeeting(int meetingNum) {
      curMeeting = meetingNum;
      UpdateHighlight();
    }

    public void MovePaste() {
      moveMode = true;
      removeDanger = true;
      pasteImages[nextPaste].SetActive(true);
      int imagePos = GetImagePos(hoverImage);
      int meetNum = sc.GetMeetingNum(imagePos);
      int localIndex = sc.GetMeetingPos(imagePos);
      pasteImages[nextPaste].GetComponent<RawImage>().texture = meetings[meetNum].GetIdeas()[localIndex];
      moved= false;
      arrows.SetActive(true);
      arrows.transform.position =  pasteImages[nextPaste].transform.position;
      HideBar();
    }

    public List<Texture2D> GetMeetingImages() {
      List<Texture2D> wowz =  new List<Texture2D>();
      for(int x=0;x< nextMeeting;x++) {
        MeetingCode mt = meetings[x];
        wowz.Add(mt.ideaList[0]);
      }
      wowz.Add(newMeetingIcon);
      return wowz;
    }

    public List<Texture2D> GetAllImages() {
      List<Texture2D> allIdeas = new List<Texture2D>();
      for(int x =0;x< nextMeeting;x++) {
        foreach(Texture2D texas in meetings[x].GetIdeas()) {
          allIdeas.Add(texas);
        }
      }
      return allIdeas;
    }

    public string[] GetDateList() {
      string[] strList = {"", "", ""};
      if(curMeeting < nextMeeting) {
        strList[1] =  "Meeting " + (meetings[curMeeting].meetingNum + 1)  + " (" +  meetings[curMeeting].date + ")";
      }
      if(curMeeting > 0) {
        strList[0] = "Meeting " + (meetings[curMeeting-1].meetingNum + 1);
      }
      if(curMeeting + 1 < nextMeeting) {
        strList[2] = "Meeting " + (meetings[curMeeting+1].meetingNum + 1);
      }
      return strList;
    }






}

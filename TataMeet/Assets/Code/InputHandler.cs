using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{

    public string serialLink = "COM";
   SerialPort sp;
   //public MainControl mc;
   public MainCode mc;
   public bool centered = true;

    // Start is called before the first frame update
    void Start()
    {
      string commie = PlayerPrefs.GetString("Com", "COM5");
      Debug.Log("COM IS " + commie);
      sp = new SerialPort(commie, 9600);
      sp.Open ();
      sp.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
      try{
          string line = sp.ReadLine();
          Debug.Log(line);
          int numLine = int.Parse(line);
          if(numLine == -1 && centered) {
            //Debug.Log("Left it is");
            mc.Left();
            centered = false;
          }
          else if(numLine == 1 && centered) {
            mc.Right();
            centered = false;
          }
          else if(numLine == 2 && centered) {
            mc.Up();
            centered = false;
          }
          else if(numLine == -2 && centered) {
            mc.Down();
            centered = false;
          }
          else if(numLine == 8) {
            mc.Click();
          }
          else if(numLine == 7) {
            mc.Menu();
          }
          else if (numLine == 101) {
            mc.Login();
          }
          else if(numLine == 100) {
            mc.Logout();
          }
          else if(numLine == 0) {
            mc.Center();
            centered = true;
          }
      }
      catch(System.Exception){
      }

      if(Input.GetKeyDown("space")) {
        mc.Click();
      }
      if(Input.GetKeyDown("right")) {
        mc.Right();
      }
      if(Input.GetKeyDown("left")) {
        mc.Left();
      }
      if(Input.GetKeyDown("up")) {
        mc.Up();
      }
      if(Input.GetKeyDown("down")) {
        mc.Down();
      }
      if(Input.GetKeyDown(KeyCode.R)) {
        SceneManager.LoadScene("TataScreen",  LoadSceneMode.Single);
      }
      if(Input.GetKeyDown(KeyCode.Tab)) {
        mc.Login();
      }

      if(Input.GetKeyDown(KeyCode.Backspace)) {
        mc.Menu();
      }

    }
}

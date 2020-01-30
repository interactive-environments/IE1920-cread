using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class COMCode : MonoBehaviour
{

   public InputField inputz;
   public InputField inputz2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Proceed() {
      string comString = inputz.text;
      string driveString = inputz2.text;
      PlayerPrefs.SetString("Com", comString);
      PlayerPrefs.SetString("OneDrive", driveString);
      SceneManager.LoadScene("TataScreen");
    }
}

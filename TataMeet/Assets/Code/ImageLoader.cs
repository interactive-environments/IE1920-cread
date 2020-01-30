using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class ImageLoader : MonoBehaviour
{

//@"/Users/willieoverman/OneDrive/Pictures/Camera Roll"
///Users/willieoverman/Documents/Interactive Environments/TataMeetImages
//@"/Users/willieoverman/OneDrive/Pictures/Camera Roll"
// @"C:\Users\Tiger Lei\Documents\CREAD\OneDrive\Pictures\Camera Roll"
  public string filesLocation = @"/Users/willieoverman/Documents/Interactive Environments/TataMeetImages";

  public List<Texture2D> images = new List<Texture2D>();
  public Renderer image1;
  public int imageCount = 0;
  public MainCode mc;
  public List<string> imageNames =  new List<string>();

    // Start is called before the first frame update
    void Start()
    {
      string drivie = PlayerPrefs.GetString("OneDrive", "");
      filesLocation = drivie;
        //LoadImages();
        InvokeRepeating("CheckImages",0.0f,1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CheckImages() {
      LoadImages();
    }

    public void LoadImages() {
      string[] filePaths = Directory.GetFiles(filesLocation, "*.jpg", SearchOption.AllDirectories);
      int newCount = filePaths.Length;
      //Debug.Log( newCount + " Images Found");
        //Debug.Log("ALRIGHTY");
        foreach(string filePath in filePaths) {
          if(!imageNames.Contains(filePath)) {
          //string filePath = filePaths[x];
          WWW load = new WWW("file:///"+filePath);
           if (string.IsNullOrEmpty(load.error)) {
                  //images.Add(load.texture);
                  //Debug.Log("New Image Gonna be processed");
                  mc.AddIdea(load.texture);
                  Debug.Log(filePath + " ADDED to images");
                  imageNames.Add(filePath);
           }
           imageCount++;
         }
        }





    }
}

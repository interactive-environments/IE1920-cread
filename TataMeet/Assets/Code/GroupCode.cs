using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupCode : MonoBehaviour
{

    // Index of group
    public int groupNum = 0;
    // Four quads with images
    public GameObject[] images;
    // Any image added to group
    public bool used = false;
    // Group is next in line
    public bool newGroup = false;
    // Counter of amount of images in group
    public int curImages = 0;
    // New Group Icon
    public GameObject addGroup;
    public GameObject groupGraphics;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Adds image to group
    public void AddImage() {

    }

    // Pops up group as New Group Button
    public void SpawnGroup() {
      groupGraphics.SetActive(true);
      addGroup.SetActive(true);
      newGroup = true;

    }

    public void AddIdea(Texture2D tex) {
      if(newGroup) {
        newGroup = false;
        addGroup.SetActive(false);
      }
      images[curImages].SetActive(true);
      images[curImages].GetComponent<Renderer>().material.mainTexture = tex;
      curImages++;

    }
}

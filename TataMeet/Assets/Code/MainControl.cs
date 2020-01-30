using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl : MonoBehaviour
{

    public GameObject camz;
    public int curGroup = 0;

    // List of group gameobjects
    public GameObject[] groups;

    public int finalGroup = 0;

    bool moving = false;
    public Vector3 dest;

    // 0: Zoom out, 1: Move, 2: Zoom in
    int moveStage = 0;

    public float moveSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if(moving) {
        bool stillMoving = false;
        Vector3 destHigh = new Vector3(dest.x, dest.y, camz.transform.position.z);
        float dist = Mathf.Abs(Vector3.Distance(camz.transform.position, destHigh));
        if(dist > 0.1f) {
          float step =  moveSpeed * Time.deltaTime; // calculate distance to move
          camz.transform.position = Vector3.MoveTowards(camz.transform.position, destHigh, step);
          stillMoving = true;
        }

        if(moveStage == 0) {
          if(camz.transform.position.z > -4.0f) {
            camz.transform.Translate(-Vector3.forward * Time.deltaTime * moveSpeed);
            stillMoving = true;
          }
          else {
            moveStage = 1;
          }

        }
        else if(moveStage == 1) {
          if(camz.transform.position.z < 0.0f) {
            camz.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            stillMoving = true;
          }
        }

        if(!stillMoving) {
          //moving = false;
        }

      }

      if(Input.GetKeyDown("right")) {
        MoveNext();
      }
      if(Input.GetKeyDown("left")) {
        MoveBack();
      }
    }

    public void MoveNext() {
      if(curGroup < finalGroup) {
        curGroup++;
        MoveCam(groups[curGroup].transform.position);
      }
    }

    public void MoveCam(Vector3 location) {
      dest = location;
      moving = true;
      moveStage = 0;
    }

    public void MoveBack() {
      if(curGroup > 0) {
        curGroup--;
        MoveCam(groups[curGroup].transform.position);
      }
    }

    public void AddIdea(Texture2D tex) {
      groups[curGroup].GetComponent<GroupCode>().AddIdea(tex);
      Debug.Log("CUR GROUP IS " + curGroup);
      Debug.Log("FINAL GROUP IS " + finalGroup);
      if(curGroup == finalGroup) {

        groups[curGroup + 1].GetComponent<GroupCode>().SpawnGroup();
        finalGroup++;
      }
    }


}

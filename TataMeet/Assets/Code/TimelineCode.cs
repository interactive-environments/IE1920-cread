using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineCode : MonoBehaviour
{
    public Text[] dates;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDates(string[] ds) {
      for(int x =0;x< ds.Length;x++) {
        dates[x].text = ds[x];
      }
    }

}

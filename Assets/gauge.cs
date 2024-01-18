using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gauge : MonoBehaviour
{
    public float gaugeValue = 0f;
    public float goalValue = 0.5f;
    private float gaugeStartRotation = -45f;
    private float gaugeEndRotation = -315f;
    private Transform needle = null;
    private Transform goal = null;

    private void Start()
    {
        needle = transform.Find("Pointer");
        goal = transform.Find("Goal");
    }


    // Update is called once per frame
    void Update()
    {
        float rotation = Mathf.Lerp(gaugeStartRotation, gaugeEndRotation, gaugeValue);
        needle.localRotation = Quaternion.Euler(0f, rotation, 0 );
        goal.localRotation = Quaternion.Euler(0f, Mathf.Lerp(gaugeStartRotation, gaugeEndRotation, goalValue), 0 );
        
        //always look at the camera by rotating on y axis, x and z are locked to default
        
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(-90.0f, transform.rotation.eulerAngles.y, 0f);

        
        



    }
}

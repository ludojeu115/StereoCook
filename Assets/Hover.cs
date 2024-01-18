using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    float originalY;
    public float floatStrength = 1;
    public float floatOffset = 0;

    private void Start()
    {
        floatOffset = UnityEngine.Random.Range(0, 2 * (float)Math.PI);
        originalY = this.transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,
            originalY + ((float)Math.Sin(Time.time+floatOffset) * floatStrength),
            transform.position.z);
    }
}

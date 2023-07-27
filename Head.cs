using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    float time;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 2f)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        }
    }
}

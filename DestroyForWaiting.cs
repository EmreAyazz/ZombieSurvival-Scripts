using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyForWaiting : MonoBehaviour
{
    float time;
    public float maxTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > maxTime)
            Destroy(gameObject);
    }
}

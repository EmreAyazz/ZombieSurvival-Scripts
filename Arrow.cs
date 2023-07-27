using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 30);
        }
        else
        {
            if (hit.transform.GetComponent<Collider>().isTrigger)
                transform.Translate(Vector3.forward * Time.deltaTime * 30);
        }
    }

}

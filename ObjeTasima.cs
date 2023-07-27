using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjeTasima : MonoBehaviour
{
    RaycastHit hit, hit2;

    public GameObject myobj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (Input.GetMouseButtonDown(0))
            {
                myobj = hit.transform.gameObject;
                Destroy(myobj.GetComponent<Rigidbody>());
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            myobj.AddComponent<Rigidbody>();
            myobj = null;
        }

        if (myobj)
        {
            if (Physics.Raycast(ray, out hit2, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                myobj.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
            }
        }
    }
}

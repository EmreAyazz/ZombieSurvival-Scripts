using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeriFirlama : MonoBehaviour
{
    public float time;
    bool okay;
    public float backpower, upPower;
    // Start is called before the first frame update
    void Start()
    {
        SetRagdoll(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.truckComing)
            time -= Time.deltaTime;

        if (time <= 0 && !okay)
        {
            SetRagdoll(true);
            okay = true;
        }
    }

    public void SetRagdoll(bool active)
    {
        Rigidbody[] rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = !active;

            if (active)
            {
                rigidbody.AddForce(-transform.forward * backpower);
                rigidbody.AddForce(transform.up * upPower);
            }
        }

        Collider[] colliders = transform.GetChild(0).GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = active;
        }

    }
}

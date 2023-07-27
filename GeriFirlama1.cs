using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeriFirlama1 : MonoBehaviour
{
    public float time;
    public float backpower, upPower;
    // Start is called before the first frame update
    void Start()
    {
        SetRagdoll(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bus"))
        {
            GameObject money = Instantiate(Resources.Load<GameObject>("Money 1"), transform.position + new Vector3(0, 1, 0), transform.rotation);
            money.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 10f), Random.Range(-10f, 10f)), 50f, 7.5f);
            money.GetComponent<Money>().money = 20;
            SetRagdoll(true);
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

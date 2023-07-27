using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyCamera : MonoBehaviour
{
    public Vector3 distance;
    public Vector3 lookingDistance;
    public GameObject player;

    MainCamera mainCamera;

    private void Awake()
    {
        mainCamera = GameObject.FindObjectOfType<MainCamera>();
        mainCamera.target = gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindObjectOfType<MainCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.target = gameObject;
    }

    private void FixedUpdate()
    {
        if (player)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + distance.y, player.transform.position.z + distance.z);
            transform.LookAt(player.transform.position + lookingDistance);
        }
    }
}

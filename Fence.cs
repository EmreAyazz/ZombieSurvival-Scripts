using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    public bool opened;
    public GameObject area;
    public int money;
    public GameObject openingObject;

    public ZombieManager zombieArea;

    public GameObject companion;

    private void Start()
    {
        if (openingObject && openingObject.name == "Doctor")
            TutorialManager.Instance.ilkFence = gameObject;
    }
}

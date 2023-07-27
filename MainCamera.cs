using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainCamera : MonoBehaviour
{
    public float time;
    public GameObject target;
    GameObject lastTarget;
    public bool targetChanged;
    public bool following;

    private void Start()
    {
        following = true;
        lastTarget = target;
    }

    void LateUpdate()
    {
        if (target != lastTarget)
        {
            targetChanged = true;
            lastTarget = target;
        }
        if (targetChanged)
        {
            following = false;
            transform.DOMove(target.transform.position, time).OnComplete(() => 
            {
                following = true;
            });
            transform.DORotate(target.transform.eulerAngles, time);
            targetChanged = false;
        }
        else
        {
            if (following)
            {
                transform.position = target.transform.position;
                transform.rotation = target.transform.rotation;
            }
        }
    }
}

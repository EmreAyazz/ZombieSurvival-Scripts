using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100;
    public float totalHealth = 100;

    GameObject healthBar;

    private void Start()
    {
        healthBar = transform.Find("Health").gameObject;
    }

    private void Update()
    {
        healthBar.transform.GetChild(0).localScale = new Vector3(health / totalHealth, 1, 1);    
    }
}

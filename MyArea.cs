using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyArea : MonoBehaviour
{
    Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Enemy":
                if (!player.enemies.Contains(other.gameObject))
                {
                    if (other.GetComponent<NavMeshAgent>().isOnNavMesh)
                    {
                        transform.parent.GetComponent<Player>().enemies.Add(other.gameObject);
                        transform.parent.GetComponent<Player>().EnemiesKontrol();
                    }
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Enemy":
                if (transform.parent.GetComponent<Player>().enemies.Contains(other.gameObject))
                {
                    transform.parent.GetComponent<Player>().enemies.Remove(other.gameObject);
                    transform.parent.GetComponent<Player>().EnemiesKontrol();
                }
                break;
        }
    }
}

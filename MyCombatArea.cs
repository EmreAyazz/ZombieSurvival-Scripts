using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCombatArea : MonoBehaviour
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
        if (!player.myWeapon.bullet)
        {
            GetComponent<Collider>().enabled = true;
            transform.localScale = new Vector3(player.myWeapon.range * 100, player.myWeapon.range * 100, transform.localScale.z);
        }
        else
        {
            GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!player.combatEnemies.Contains(other.gameObject))
                player.combatEnemies.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (player.combatEnemies.Contains(other.gameObject))
                player.combatEnemies.Remove(other.gameObject);
        }
    }
}

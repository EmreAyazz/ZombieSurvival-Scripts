using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    List<GameObject> enemies = new List<GameObject>();
    Player player;

    public float damage;
    bool closed;

    float time = 1f;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        time = 1f;
    }

    private void Update()
    {
        if (!closed)
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (player.x2Damage)
                        damage *= 2;

                    if (enemies.Count > 0)
                    {
                        if (enemies[i] != null)
                        {
                            if (enemies[i].GetComponent<Enemy>())
                                enemies[i].GetComponent<Enemy>().health -= damage;
                            if (enemies[i].GetComponent<BossEnemy>())
                                enemies[i].GetComponent<BossEnemy>().health -= damage;
                            if (enemies[i].GetComponent<Skeleton>())
                                enemies[i].GetComponent<Skeleton>().health -= damage;
                        }
                    }

                    closed = true;

                    Invoke("DestroyThis", 3f);
                }
            }
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //public varaibles
    public float speed = 3.0f;
    public float spawnInterval;
    public int miniEnemySpawnCount;
    public bool isBoss = false;

    //private variables   
    private float nextSpawn;
    private Rigidbody enemyRb;
    private GameObject player;
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        //if there is a boss enemy get spawn manager script
        if (isBoss)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //move with normalized force based on player and enemy positions
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        //enemay movement following player
        enemyRb.AddForce(lookDirection * speed);

        //if there is a boss
        if (isBoss)
        {
            //if game time is greater than next spawn time
            if(Time.time > nextSpawn)
            {
                //next spawn time is equal to game time plus length of spawnINterval
                nextSpawn = Time.time + spawnInterval;
                //calls spawn manger to spawn a certain amonut of mini enemies
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }

        //destroys enemies when they fall off platform
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}

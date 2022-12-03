using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //private variables
    private float speed = 3.0f;
    private Rigidbody enemyRb;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //move with normalized force based on player and enemy positions
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        //enemay movement following player
        enemyRb.AddForce(lookDirection * speed);

        //destroys enemies when they fall off platform
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}

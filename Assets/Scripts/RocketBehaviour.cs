using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    private Transform target;
    private bool homing;
    private float speed = 15.0f;
    private float rocketStrength = 15.0f;
    private float aliveTimer = 5.0f;

    // Update is called once per frame
    void Update()
    {
        if(homing && target != null)
        {
            //missiles reducing distance between them and tartget normalized
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            //missile movement speed
            transform.position += moveDirection * speed * Time.deltaTime;
            //orientes missile front to aim at target
            transform.LookAt(target);
        }
    }

    public void Fire(Transform newTarget)
    {
        //
        target = newTarget;
        homing = true; 
        //destroy missiles after set time
        Destroy(gameObject, aliveTimer);
    }

    //missile collision and rebound
    private void OnCollisionEnter(Collision col)
    {
        //if target exists
        if (target != null)
        {
            //if tag of missile and target tag match
            if (col.gameObject.CompareTag(target.tag))
            {
                //get rigidbody of target
                Rigidbody targetRigidbody = col.gameObject.GetComponent<Rigidbody>();
                //use normal of of collision contact to determine direction to push
                Vector3 away = -col.contacts[0].normal;
                //add force to collision for movement of target
                targetRigidbody.AddForce(away * rocketStrength, ForceMode.Impulse);
                //destroy missile
                Destroy(gameObject);
            }
        }
    }
}

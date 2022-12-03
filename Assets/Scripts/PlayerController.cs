using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public variables
    public float speed = 5.0f;
    public bool hasPowerup;
    public GameObject powerupIndicator;

    //private variables
    private float powerupStrength = 15.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;

    // Start is called before the first frame update
    void Start()
    {
        
        playerRb= GetComponent<Rigidbody>(); //finds rigidbody component on player
        focalPoint = GameObject.Find("Focal Point"); //finds focal point game object

    }

    // Update is called once per frame
    void Update()
    {
        //player input forward/backward movement
        float forwardInput = Input.GetAxis("Vertical");
        //player movement on z-axis
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        //attaches powerup indicator to player while they move
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    //on collision destroy powerup object and set has powerup to true
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true); //turns on powerup indicator effect
            StartCoroutine(PowerupCountdownRoutine()); //starts loop outside of update calling method
        }
    }

    //loop that takes place outside of update loop
    IEnumerator PowerupCountdownRoutine()
    {
        //starts a countdown of 7 seconds
        yield return new WaitForSeconds(7);
        //reutrns powerup to false
        hasPowerup= false;
        powerupIndicator.gameObject.SetActive(false); //turns off powerup indicator effect
    }

    //collision with enemy while powered up
    private void OnCollisionEnter(Collision collision)
    {
        //if colliding with enemy and player has the powerup
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            //get rigidbody compoenent when colliding with enemy
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            //distance collision will create
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            //impulse force push enemy away from player on collision
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            Debug.Log("Collided");
        }
    }
}

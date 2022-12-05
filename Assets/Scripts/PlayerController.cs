using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public variables
    public float speed = 5.0f;
    public float hangTime;
    public float smashSpeed;
    public float explosionForce;
    public float explosionRadius;
    public bool hasPowerup;
    public bool smashing = false;
    public GameObject powerupIndicator;
    public GameObject rocketPrefab;
    public PowerUpType currentPowerup = PowerUpType.None; //sets initial powerup to no powerups

    //private variables
    private float floorY;
    private float powerupStrength = 15.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private GameObject tmpRocket; //spawning rockets
    private Coroutine powerupCountdown;

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
        //if powerup type is rockets and player pushes F key run LaunchROckets method
        if (currentPowerup == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }
        //if powerup type is smash and player pushes space bar run smash coroutine
        if (currentPowerup == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space)) 
        {
            smashing = true;
            StartCoroutine(Smash());
        }

    }

    //on collision destroy powerup object and set has powerup to true
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            currentPowerup = other.gameObject.GetComponent<Powerup>().powerUpType; //sets current powerup to one of the powerup types defined in Powerup script
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true); //turns on powerup indicator effect

            //if have powerup already and pick up new one, stop first powerup and replace with second
            if (powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }
            powerupCountdown = StartCoroutine(PowerupCountdownRoutine()); //starts loop outside of update calling method

        }
    }

    //loop that takes place outside of update loop
    IEnumerator PowerupCountdownRoutine()
    {
        //starts a countdown of 7 seconds
        yield return new WaitForSeconds(7);
        //reutrns powerup to false
        hasPowerup= false;
        currentPowerup = PowerUpType.None; //returns powerup type to none
        powerupIndicator.gameObject.SetActive(false); //turns off powerup indicator effect
    }

    //coroutine for smash powerup effects
    IEnumerator Smash()
    {
        var enemies = FindObjectsOfType<Enemy>();

        //store y position before taking off
        floorY = transform.position.y;

        //Calculate amount of time player goes up
        float jumpTime = Time.time + hangTime;

        //while gametime is less than jumptime
        while (Time.time < jumpTime)
        {
            //move player up while keeping x velocity
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed * 2);
        }

        //move player down
        while (transform.position.y > floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }

        //cycle through all enemies
        for (int i = 0; i < enemies.Length; i++)
        {
            //apply explosion force that originates from our position
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
            }

            //return smashing bool to false
            smashing = false;
        }
    }

    //collision with enemy while powered up
    private void OnCollisionEnter(Collision collision)
    {
        //if colliding with enemy and powerup picked up is a pushback type
        if (collision.gameObject.CompareTag("Enemy") && currentPowerup == PowerUpType.Pushback)
        {
            //get rigidbody compoenent when colliding with enemy
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            //distance collision will create
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            //impulse force push enemy away from player on collision
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            Debug.Log("Player collided with: " + collision.gameObject.name + " with powerup set to " + currentPowerup.ToString()); //console logs which powerup picked up
        }
    }

    void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>()) 
        {
            //spawns rockets, launches in y-axis to not pushback player, no quaternion rotation
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            //calls Fire method from RocketBehaviour script and targets enemy position
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //public variables
    public int enemyCount;
    public int waveNumber = 1;
    public GameObject[] enemyPrefab; //gameobject array
    public GameObject[] powerupPrefab; //gameobject array

    //private variables
    private float spawnRange = 9.0f;

    // Start is called before the first frame update
    void Start()
    {
        //initial enemy wave
        SpawnEnemyWave(waveNumber);
        //initial powerup
        SpawnPowerup();
    }

    // Update is called once per frame
    void Update()
    {
        //sets enemy count to length of array of enemies active
        enemyCount = FindObjectsOfType<Enemy>().Length;

        //if enemies fall to 0 spaw more enemies
        if (enemyCount == 0)
        {
            waveNumber++; //increases wave number by 1 on each iteration
            SpawnEnemyWave(waveNumber); //spawn more enemies with each iteration
            SpawnPowerup();
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        //keep looping until condition met 
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            //random enemy from array
            int randomEnemy = Random.Range(0, enemyPrefab.Length);

            //spawns random enemy from array with proper rotation in random position
            Instantiate(enemyPrefab[randomEnemy], GenerateSpawnPosition(), enemyPrefab[randomEnemy].transform.rotation);
        }
    }

    void SpawnPowerup()
    {
        //random array index
        int randomPowerup = Random.Range(0,powerupPrefab.Length);
        //spawns random powerup with proper rotation in random position
        Instantiate(powerupPrefab[randomPowerup], GenerateSpawnPosition(), powerupPrefab[randomPowerup].transform.rotation);
    }

    //method that generates the random vector3 position for spawn range
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomPos;
    }
}

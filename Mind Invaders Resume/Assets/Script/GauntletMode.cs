using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefenderSpawn
{
    public Vector3 TopSpawn;
    public Vector3 BottomSpawn;
}

public class GauntletMode : MonoBehaviour {

    public GameObject[] powerUps;
    public GameObject defenderEnemy;
    public GameObject invaderEnemy;
    public GameObject rammerEnemy;
    public GameObject dodgerEnemy;
    public GameObject bossEnemy;
    public DefenderSpawn defenderSpawn;
    public Vector3 powerUpSpawn;
    public Vector3 dodgerSpawn;
    public Vector2 defMovementTimeRange;
    public GUIText enemyText;
    public int R1EnemyMax;
    public int R1DefenderMax;
    public float powerUpSpawnTime;

    private GameController gameController;
    private int enemiesRemaining;
    private int defendersRemaining;
    private int dodgersRemaining;
    private int invadersRemaining;
    private int rammersRemaining;

    private bool nextRound;

    void Start()
    {
        nextRound = false;
        enemyText.text = "";

        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Cannot Find 'GameController' script");
        }

        StartCoroutine(SpawnPowerUps());
        StartCoroutine(RoundOne());
    }

    IEnumerator RoundOne()
    {
        yield return new WaitForSeconds(gameController.introWait1 + gameController.introWait2 + gameController.introWait3);

        enemiesRemaining = R1EnemyMax;
        defendersRemaining = R1DefenderMax;
        UpdateEnemyText();

        for (int i = 0; i < R1DefenderMax; i++)
        {
            SpawnDefenders();
        }

        while (!nextRound)
        {
            if (defendersRemaining < R1DefenderMax)
            {
                SpawnDefenders();
                defendersRemaining++;
            }

            if (enemiesRemaining == 0)
            {
                nextRound = true;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator SpawnPowerUps()
    {
        yield return new WaitForSeconds(gameController.introWait1 + gameController.introWait2 + gameController.introWait3);
        Quaternion spawnRotation;
        Vector3 spawnLocation;

        while (true)
        {
            yield return new WaitForSeconds(powerUpSpawnTime);
            spawnLocation = new Vector3(Random.Range(powerUpSpawn.x, powerUpSpawn.y),
            0.0f, powerUpSpawn.z);

            GameObject powerUp = powerUps[Random.Range(0, powerUps.Length)];
            if (powerUp.tag == "OneUp")
            {
                spawnRotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
            }
            else
            {
                spawnRotation = Quaternion.identity;
            }

            Instantiate(powerUp, spawnLocation, spawnRotation);
        }
    }

    void UpdateEnemyText()
    {
        enemyText.text = "Enemies Remaining: " + enemiesRemaining;
    }

    void SpawnDefenders()
    {
        Vector3 spawnLocation;
        Quaternion spawnRotation = Quaternion.identity;

        if (Random.value < 0.5f)
        {
            spawnLocation = new Vector3(Random.Range(defenderSpawn.TopSpawn.x, 
                defenderSpawn.TopSpawn.y),
            0.0f, defenderSpawn.TopSpawn.z);
        }
        else
        {
            spawnLocation = new Vector3(Random.Range(defenderSpawn.BottomSpawn.x,
                defenderSpawn.BottomSpawn.y),
            0.0f, defenderSpawn.BottomSpawn.z);
        }

        Instantiate(defenderEnemy, spawnLocation, spawnRotation);

    }

    void SpawnDodgers()
    {
        Vector3 spawnLocation;
        Quaternion spawnRotation = Quaternion.identity;

        spawnLocation = new Vector3(Random.Range(dodgerSpawn.x, dodgerSpawn.y), 0.0f,
                dodgerSpawn.z);
        Instantiate(dodgerEnemy, spawnLocation, spawnRotation);
    }

    void EnemyDefeated()
    {
        enemiesRemaining--;
        UpdateEnemyText();
    }

    public void DefenderDefeated()
    {
        defendersRemaining--;
        EnemyDefeated();
    }
	
}

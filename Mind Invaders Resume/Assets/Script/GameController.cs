using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TextLimit
{
    public int answerLimit;
    public int questionLimit;
    public int maxAnswerSize;
    public int maxQuestionSize;
};

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public GameObject[] choiceHazards;
    public GameObject enemyShip;
    public GameObject kamikaze;
    public GameObject rammer;
    public GameObject playerExplosion;
    public GameObject extraLife;
    public GameObject shieldIcon;
    public GameObject shield;
    public GameObject slowDownIcon;
    public Vector3 spawnEnemyLeft;
    public Vector3 spawnEnemyRight;
    public Vector3 spawnChoiceLocation;
    public Vector2 spawnKamiLeft;
    public Vector2 spawnKamiRight;
    public Vector2 spawnKamiZRange;
    public Vector2 spawnLifeXRange;
    public float spawnLifeZ;
    public float spawnOffset; // Space between answer asteroids
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public float spawnKamiWait;
    public float startQuestionWait;
    public float introWait1;
    public float introWait2;
    public float introWait3;
    public float extraLifeWait;
    public float shieldTime;
    public float invinsibilityFrames;
    public float timeLimit;
    public float slowDownTimeLength;
    public int hazardCount;
    public int lifeCount;
    public int maxEnemyNumber;
    public int enemySpawnFrequency;
    public int roundToSendKamikaze;

    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;
    public GUIText questionText;
    public GUIText lifeText;
    public GUIText introText;
    public GUIText roundText;
    public GUIText timeText;
    public Text[] answer;
    public TextLimit textLimit;

    private bool gameOver;
    private bool restart;
    private bool nextWave;
    private bool correctAnswer;
    private bool createShield;
    private bool slowDown;
    private int score;
    private int prevScore;
    private int enemyNumber;
    private int currentRound;
    private float damageTime;
    private Vector3 startPlayerPosition;
    private GameObject player;
    private GameObject[] newChoiceHazards;
    private FileReader fileReader;

    void Start()
    {
        gameOver = false;
        restart = false;
        correctAnswer = false;
        createShield = false;
        slowDown = false;
        GlobalVariables.slowDownTime = false;
        restartText.text = "";
        gameOverText.text = "";
        if(questionText != null)
        {
            questionText.text = "";
        }
        lifeText.text = "";
        introText.text = "";
        roundText.text = "";
        timeText.text = "";
        score = 0;
        enemyNumber = 0;
        currentRound = 1;
        damageTime = 0.0f;
        newChoiceHazards = new GameObject[choiceHazards.Length];

        GameObject fileReaderObject = GameObject.FindGameObjectWithTag("FileReader");
        if (fileReaderObject != null)
        {
            fileReader = fileReaderObject.GetComponent<FileReader>();
        }

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            startPlayerPosition = player.transform.position;

        }

        UpdateScore();
        UpdateLife();
        UpdateRound();

        StartCoroutine(Intro());

        if(!GlobalVariables.gauntletMode)
        {
            StartCoroutine(SpawnKamikaze());
            StartCoroutine(SpawnQuestions());
            StartCoroutine(SpawnEnemies());
            StartCoroutine(SpawnExtraLives());
            StartCoroutine(SpawnShieldIcon());
            StartCoroutine(SpawnSlowDownIcon());
        }

        if (GlobalVariables.timeAttack)
        {
            UpdateTime((int)timeLimit);
            StartCoroutine(TimeAttack());
        }

    }

    private void Update()
    {
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        damageTime += Time.deltaTime;

        if (restart)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(1);
            }
            else if(Input.GetKeyDown(KeyCode.M))
            {
                SceneManager.LoadScene(0);
            }
        }

        if (createShield)
        {
            StartCoroutine(CreateShield());
        }

        if(GlobalVariables.slowDownTime)
        {
            StartCoroutine(SlowDownTime());
        }
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(introWait1);
        introText.text = "Ready?!";
        yield return new WaitForSeconds(introWait2);
        introText.text = "GO!!";
        yield return new WaitForSeconds(introWait3);
        introText.text = "";
    }

    IEnumerator SpawnQuestions()
    {
        yield return new WaitForSeconds(introWait1 + introWait2 + introWait3);
        yield return new WaitForSeconds(startQuestionWait);

        Quaternion spawnRotation = Quaternion.identity;
        float initialSpawnX = spawnChoiceLocation.x;
        int questionLocater;
        int questionSet;
        string correctAnswerText = "";

        while (true)
        {
            nextWave = false;
            questionLocater = Random.Range(0, fileReader.stringList.Count / 7);
            questionSet = 7 * questionLocater;
            questionText.fontSize = textLimit.maxQuestionSize;
            questionText.text = fileReader.stringList[questionSet];
            if (questionText.text.Length > textLimit.questionLimit)
            {
                questionText.fontSize -= (int)((questionText.text.Length - textLimit.questionLimit) * 0.75f);
            }

            for (int i = 0; i < answer.Length; i++)
            {
                answer[i].fontSize = textLimit.maxAnswerSize;
                answer[i].text = fileReader.stringList[questionSet + (i + 1)];
                if(answer[i].text.Length > textLimit.answerLimit)
                {
                    answer[i].fontSize -= (int)((answer[i].text.Length - textLimit.answerLimit) * 0.5f);
                }

                if(answer[i].text == fileReader.stringList[questionSet + 5])
                {
                    correctAnswerText = answer[i].text;
                }
            }

            //Question goes here/////////////////////////////////////
            for (int i = 0; i < choiceHazards.Length; ++i)
            {
                newChoiceHazards[i] = Instantiate(choiceHazards[i], spawnChoiceLocation, spawnRotation);
                newChoiceHazards[i].GetComponentInChildren<ClampAnswer>().answer = answer[i];
                choiceHazards[i].tag = "AnswerChoice";
                spawnChoiceLocation.x += spawnOffset;

                if(newChoiceHazards[i].GetComponentInChildren<ClampAnswer>().answer.text == correctAnswerText)
                {
                    GameObject correctHazard = newChoiceHazards[i];
                    correctHazard.tag = "CorrectAnswer";
                }
            }

            while (!nextWave)
            {
                if (correctAnswer) 
                {
                    for (int i = 0; i < choiceHazards.Length; ++i)
                    {
                        if (newChoiceHazards[i] != null)
                        {
                            Instantiate(newChoiceHazards[i].GetComponent<DestroyByContact>().explosion,
                                newChoiceHazards[i].transform.position, newChoiceHazards[i].transform.rotation);
                            newChoiceHazards[i].GetComponentInChildren<ClampAnswer>().answer.text = "";
                            Destroy(newChoiceHazards[i]);
                        }
                    }
                    spawnChoiceLocation.x = initialSpawnX;
                    nextWave = true;
                    correctAnswer = false;
                    TrackScore();
                }
                yield return new WaitForSeconds(waveWait);
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(introWait1);
        yield return new WaitForSeconds(introWait2);
        yield return new WaitForSeconds(introWait3);

        yield return new WaitForSeconds(startWait);
        int totalEnemies;

        while (true)
        {
            if(((currentRound - 1) / GlobalVariables.enemyFreq + GlobalVariables.startEnemyVal) < GlobalVariables.maxEnemyVal)
            {
                totalEnemies = (currentRound / GlobalVariables.enemyFreq) + GlobalVariables.startEnemyVal;
            }
            else
            {
                totalEnemies = GlobalVariables.maxEnemyVal;
            }

            if (enemyNumber < totalEnemies)
            {
                Vector3 spawnLocation;
                Quaternion spawnRotation = Quaternion.identity;
                if (Random.value > 0.5f)
                {
                    spawnLocation = new Vector3(spawnEnemyLeft.x, spawnEnemyLeft.y, spawnEnemyLeft.z);
                }
                else
                {
                    spawnLocation = new Vector3(spawnEnemyRight.x, spawnEnemyRight.y, spawnEnemyRight.z);
                }

                Instantiate(enemyShip, spawnLocation, spawnRotation);
                enemyNumber++;
            }

            if (gameOver)
            {
                restartText.text = "Press 'R' to Restart\n" + "Press 'M' to return to Main Menu";
                restart = true;
                break;
            }
            yield return new WaitForSeconds(spawnWait);
        }
    }

    IEnumerator SpawnKamikaze()
    {
        yield return new WaitForSeconds(introWait1);
        yield return new WaitForSeconds(introWait2);
        yield return new WaitForSeconds(introWait3);

        yield return new WaitForSeconds(spawnKamiWait);

        Vector3 spawnLocation;
        Quaternion spawnRotation = Quaternion.identity;
        while (true)
        {
            if (currentRound >= GlobalVariables.kamiFirstRound && player != null)
            {
                if (Random.value > 0.5f)
                {
                    spawnLocation = new Vector3(spawnKamiLeft.x, spawnKamiLeft.y, Random.Range(spawnKamiZRange.x, spawnKamiZRange.y));
                }
                else
                {
                    spawnLocation = new Vector3(spawnKamiRight.x, spawnKamiRight.y, Random.Range(spawnKamiZRange.x, spawnKamiZRange.y));
                }

                Instantiate(kamikaze, spawnLocation, spawnRotation);
            }
            yield return new WaitForSeconds(GlobalVariables.kamiFreq);
        }
    }

    IEnumerator TimeAttack()
    {
        yield return new WaitForSeconds(introWait1 + introWait2 + introWait3);
        float timeRemaining = timeLimit;

        while (timeRemaining > 0.9f)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTime((int)timeRemaining);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        GameOver("Time's Up!");
        Instantiate(playerExplosion, player.transform.position, player.transform.rotation);
        Destroy(player);
    }

    IEnumerator SpawnExtraLives()
    {
        yield return new WaitForSeconds(introWait1 + introWait2 + introWait3);

        Quaternion spawnRotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
        bool oneUp = false;
        bool checkRound = true;

        while (true)
        {
            if(currentRound % 5 != 0)
            {
                checkRound = true;
            }

            if (checkRound && currentRound % 5 == 0)
            {
                oneUp = true;
                checkRound = false;
            }

            if(oneUp && Random.value > 0.5f && Random.value > 0.5f)
            {
                Vector3 spawnLocation = new Vector3(Random.Range(spawnLifeXRange.x, spawnLifeXRange.y), 0.0f, spawnLifeZ);
                Instantiate(extraLife, spawnLocation, spawnRotation);
                yield return new WaitForSeconds(extraLifeWait);
                oneUp = false;
            }
            yield return new WaitForSeconds(1f);
        }

       /* while (true)
        {
      
             Vector3 spawnLocation = new Vector3(Random.Range(spawnLifeXRange.x, spawnLifeXRange.y), 0.0f, spawnLifeZ);
             Instantiate(extraLife, spawnLocation, spawnRotation);
             yield return new WaitForSeconds(1f);
           
        }*/
    }

    IEnumerator SpawnShieldIcon()
    {
        yield return new WaitForSeconds(introWait1 + introWait2 + introWait3);

        Quaternion spawnRotation = Quaternion.identity;
        bool oneUp = false;
        bool checkRound = true;

        while (true)
        {
            if (currentRound % 4 != 0)
            {
                checkRound = true;
            }

            if (checkRound && currentRound % 4 == 0)
            {
                oneUp = true;
                checkRound = false;
            }

            if (oneUp && Random.value > 0.5f && Random.value > 0.5f)
            {
                Vector3 spawnLocation = new Vector3(Random.Range(spawnLifeXRange.x, spawnLifeXRange.y), 0.0f, spawnLifeZ);
                Instantiate(shieldIcon, spawnLocation, spawnRotation);
                yield return new WaitForSeconds(extraLifeWait);
                oneUp = false;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator SpawnSlowDownIcon()
    {
        yield return new WaitForSeconds(introWait1 + introWait2 + introWait3);

        Quaternion spawnRotation = Quaternion.identity;
        bool oneUp = false;
        bool checkRound = true;

        while (true)
        {
            if (currentRound % 7 != 0)
            {
                checkRound = true;
            }

            if (checkRound && currentRound % 7 == 0)
            {
                oneUp = true;
                checkRound = false;
            }

            if (oneUp && Random.value > 0.5f && Random.value > 0.5f)
            {
                Vector3 spawnLocation = new Vector3(Random.Range(spawnLifeXRange.x, spawnLifeXRange.y), 0.0f, spawnLifeZ);
                Instantiate(slowDownIcon, spawnLocation, spawnRotation);
                yield return new WaitForSeconds(extraLifeWait);
                oneUp = false;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator CreateShield()
    {
        shield.SetActive(true);
        yield return new WaitForSeconds(shieldTime);
        shield.SetActive(false);
        createShield = false;
    }

    IEnumerator SlowDownTime()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(slowDownTimeLength);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02F;
        GlobalVariables.slowDownTime = false;
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void EnemyDestroyed(int enemyValue, string tag)
    {
        score += enemyValue;
        if(tag == "Defender" && !GlobalVariables.gauntletMode)
        {
            enemyNumber--;
        }
        UpdateScore();
    }

    public void CorrectAnswer(int correctAnswerValue)
    {
        score += correctAnswerValue;
        UpdateScore();
    }

    public void WrongAnswer(int wrongAnswerValue)
    {
        score -= wrongAnswerValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void TrackScore()
    {
        prevScore = score;
    }

    void UpdateLife()
    {
        lifeText.text = "Life: " + lifeCount;
    }
    void UpdateRound()
    {
        roundText.text = "Round: " + currentRound;
    }

    void UpdateTime(int timeRemaining)
    {
        timeText.text = "Time Remaining: " + timeRemaining;
    }

    public void GameOver(string goodbye)
    {
        gameOverText.text = goodbye;
        gameOver = true;
    }

    public int Death()
    {
        lifeCount--;
        UpdateLife();
        return lifeCount;
    }

    public void SetPlayerRespawnPosition()
    {
        player.transform.position = startPlayerPosition;
    }

    public float InvinsibilityFrames()
    {
        return invinsibilityFrames;
    }

    public bool CheckForInvinsibility()
    {
        if(damageTime > invinsibilityFrames)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetDamageTime()
    {
        damageTime = 0.0f;
    }

    public void NextRound()
    {
        currentRound++;
        UpdateRound();
    }

    public void CorrectAnswer()
    {
        correctAnswer = true;
    }

    public void ExtraLife()
    {
        lifeCount++;
        UpdateLife();
    }

    public void EnableShield()
    {
        createShield = true;
    }

    public void EnableTimeSlowing()
    {
        slowDown = true;
    }





    IEnumerator GauntletMode()
    {
        yield return new WaitForSeconds(introWait1 + introWait2 + introWait3);
    }

}

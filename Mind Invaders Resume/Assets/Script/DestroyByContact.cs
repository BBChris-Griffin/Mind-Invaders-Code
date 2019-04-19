using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    public int enemyValue;

    private int correctAnswerValue;
    private int wrongAnswerValue;
    private bool positionFound = false;
    private GameController gameController;
    private GauntletMode gauntletMode;
    private Vector3 startPlayerPosition;

    void Start()
    {
        correctAnswerValue = 1000;
        wrongAnswerValue = 100;

        if (GlobalVariables.gauntletMode)
        {
            GameObject gauntletModeObject = GameObject.FindGameObjectWithTag("Gauntlet");
            if (gauntletModeObject != null)
            {
                gauntletMode = gauntletModeObject.GetComponent<GauntletMode>();
            }
            else
            {
                Debug.Log("Cannot Find 'GauntletMode' script");
            }
        }

        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Cannot Find 'GameController' script");
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy Projectile") ||
            other.CompareTag("Defender") || other.CompareTag("Invader") || other.CompareTag("Rammer") || 
            other.CompareTag("Dodger") || other.CompareTag("ExtraLife") || other.CompareTag("AnswerChoice") || 
            other.CompareTag("CorrectAnswer") || other.CompareTag("ShieldIcon") || other.CompareTag("SlowTimeIcon"))
        {
            return;
        }
        if ((tag == "ExtraLife" || tag == "ShieldIcon" || tag == "Shield" || tag == "SlowTimeIcon") 
            && other.CompareTag("Projectile"))
        {
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if(other.tag == "Player" && tag == "ShieldIcon")
        {
            gameController.EnableShield();
        }

        if((other.tag == "Player" || other.tag == "Shield") && tag == "SlowTimeIcon")
        {
            GlobalVariables.slowDownTime = true;
        }

        if (other.tag == "Player" && tag != "Shield" && tag != "SlowTimeIcon" && tag != "ShieldIcon")
        {
            if (other.tag == "Player" && tag != "ExtraLife"
                && gameController.lifeCount > 1 && gameController.CheckForInvinsibility())
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.ResetDamageTime();
                gameController.SetPlayerRespawnPosition();
                gameController.Death();
            }
            else if (other.tag == "Player" && tag != "ExtraLife"
                && gameController.lifeCount == 1 && gameController.CheckForInvinsibility())
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.ResetDamageTime();
                gameController.Death();
                gameController.GameOver("Game Over!");
            }
        }

        if (tag == "Defender" || tag == "Invader")
        {
            gameController.EnemyDestroyed(enemyValue, tag);
        }

        if (tag == "AnswerChoice" && GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            GetComponentInChildren<ClampAnswer>().answer.text = "";
            gameController.WrongAnswer(wrongAnswerValue);
        }

        if (tag == "CorrectAnswer" && GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            gameController.CorrectAnswer();
            gameController.CorrectAnswer(correctAnswerValue);
            gameController.NextRound();
        }

        if (tag == "ExtraLife" && (other.tag == "Player" || other.tag == "Shield"))
        {
            gameController.ExtraLife();
        }


        if(GlobalVariables.gauntletMode)
        {
            /*if(other.tag == "Projectile" && (tag == "Defender" || tag == "Invader" 
                || tag == "Rammer") || tag == "Dodger")
            {
                gauntletMode.EnemyDefeated();
            }*/
            if(other.tag == "Projectile" && tag == "Defender")
            {
                gauntletMode.DefenderDefeated();
            }
        }

        if (!(other.tag == "Player" && (tag == "ExtraLife" || tag == "ShieldIcon" || tag == "SlowTimeIcon" ||  
            gameController.lifeCount != 0 || gameController.CheckForInvinsibility())))
        {
            if(other.tag != "Shield")
            {
                Destroy(other.gameObject);
            }
        }

        if(tag == "Shield")
        {
            return;
        }

        if(!((tag == "CorrectAnswer" || tag == "AnswerChoice") && GetComponent<Rigidbody>().velocity != Vector3.zero))
        {
            Destroy(this.gameObject);
        }
    }
}

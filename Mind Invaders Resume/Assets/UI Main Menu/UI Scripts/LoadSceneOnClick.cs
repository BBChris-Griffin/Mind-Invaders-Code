using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    public GameObject ship;
    public GameObject mainPanel;
    public float flyingTime;
    public float flySpeed;
    public GUIText dpad;
    public AudioSource flightAudio;
    public AudioSource takeOffAudio;

    private GameObject background;
    private bool load;
    private void Start()
    {
        background = GameObject.FindGameObjectWithTag("Background");
        load = false;
    }

    public void LoadByIndex(int sceneIndex)
	{
        if(ship != null)
        {
            StartCoroutine(FlyAway(sceneIndex));
        }
        else
        {
            if(GlobalVariables.gauntletMode)
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                SceneManager.LoadScene(sceneIndex);
            }
        }
    }

    public void StudyMode()
    {
        GlobalVariables.startEnemyVal = 0;
        GlobalVariables.maxEnemyVal = 0;
        GlobalVariables.kamiFirstRound = 100000000;
    }

    public void TimeAttack()
    {
        GlobalVariables.timeAttack = true;
    }

    public void GauntletMode()
    {
        GlobalVariables.gauntletMode = true;
    }

    IEnumerator FlyAway(int sceneIndex)
    {
        dpad.text = "";
        flightAudio.Pause();
        takeOffAudio.Play();
        mainPanel.GetComponent<RectTransform>().localScale = new Vector3(0.0f, 0.0f, 1.0f);
        background.GetComponent<BGScroller>().scrollSpeed = -50f;
        ship.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, flySpeed);

        yield return new WaitForSeconds(flyingTime);
        SceneManager.LoadScene(sceneIndex);
    }
		
}

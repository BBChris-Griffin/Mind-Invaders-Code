using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveNewData : MonoBehaviour {

    public Dropdown startEnemyDrop;
    public Dropdown maxEnemyDrop;
    public Dropdown enemyFreqDrop;
    public Dropdown kamiAppearDrop;
    public Dropdown kamiFreqDrop;

    public void SaveData()
    {
        GlobalVariables.startEnemyVal = startEnemyDrop.value;
        GlobalVariables.maxEnemyVal = maxEnemyDrop.value;
        GlobalVariables.enemyFreq = enemyFreqDrop.value + 1;

        if(kamiAppearDrop.value < 10)
        {
            GlobalVariables.kamiFirstRound = kamiAppearDrop.value + 1;
        }
        else
        {
            GlobalVariables.kamiFirstRound = 100000000;
        }

        GlobalVariables.kamiFreq = kamiFreqDrop.value + 1;
    }
}

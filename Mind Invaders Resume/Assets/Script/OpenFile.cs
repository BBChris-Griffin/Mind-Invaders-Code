using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class OpenFile : MonoBehaviour {

    public InputField inputField;

    public void File()
    {
        GlobalVariables.databaseFile = inputField.text;
        try
        {
            StreamReader sReader = new StreamReader(GlobalVariables.databaseFile);
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("File Not Found");
            GlobalVariables.databaseFile = "";
        }
    }
}

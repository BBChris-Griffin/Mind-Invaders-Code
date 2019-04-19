using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDatabase : MonoBehaviour {

	public void RetrieveFile(string fileName)
    {/*
#if UNITY_EDITOR
        GlobalVariables.databaseFile = UnityEditor.EditorUtility.OpenFilePanel("Retrieve Database", "", "txt");
#endif*/
        GlobalVariables.databaseFile = fileName;
    }

    public void ShowExplorer()
    {
        string itemPath = "file://[dir]";
        itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
        System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
    }

    public void UseOGDatabse()
    {
        GlobalVariables.databaseFile = "";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;


public class FileReader : MonoBehaviour {

    public List<string> stringList = new List<string>();

	void Start ()
    {

       string readPath = GlobalVariables.databaseFile; 

        ReadFile(readPath);
    }

    void ReadFile(string filePath)
    {
        StreamReader sReader;
        if (filePath != "")
        {
            sReader = new StreamReader(filePath);
            while (!sReader.EndOfStream)
            {
                string line = sReader.ReadLine();
                stringList.Add(line);
            }
            sReader.Close();
        }
        else
        {
            TextAsset database = (TextAsset)Resources.Load("questions");
            string file = database.text;
            string[] fLines = Regex.Split(file, "\n");

            for(int i = 0; i < fLines.Length; i++)
            {
                stringList.Add(fLines[i]);
            }
        }

    }

}

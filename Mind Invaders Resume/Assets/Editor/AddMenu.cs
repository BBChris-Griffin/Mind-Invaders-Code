using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMenu : EditorWindow {

    [MenuItem("Edit/Reset Playerprefs")]

	public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}

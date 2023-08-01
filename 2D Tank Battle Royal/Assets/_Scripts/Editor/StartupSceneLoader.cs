using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

[InitializeOnLoad]
public static class StartupSceneLoader
{
    static StartupSceneLoader()
    {
        EditorApplication.playModeStateChanged += LoadStartupScene;
    }

    private static void LoadStartupScene(PlayModeStateChange stateChange)
    {
        if (stateChange == PlayModeStateChange.ExitingEditMode)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        if (stateChange == PlayModeStateChange.EnteredPlayMode)
        {
            if (EditorSceneManager.GetActiveScene().buildIndex != 0)
            {
                EditorSceneManager.LoadScene(0);
            }
        }
    }
}

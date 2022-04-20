using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class DatabaseTab
{
    private string _databaseQuery;

    public void DisplayDatabaseTab()
    {

    }

    public void DisplayDatabaseDebugTab()
    {
        _databaseQuery = EditorGUILayout.TextArea(_databaseQuery, GUILayout.Height(150f));

        if(GUILayout.Button("Input"))
        {
            Debug.Log("INPUT");
            Debug.Log(DataBase.ExecuteQuery(_databaseQuery));
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate database"))
        {
            DataBase.GenerateDatabase();
        }
    }
}

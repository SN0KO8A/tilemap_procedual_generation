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
    private string _databaseAnswer;

    public void DisplayDatabaseTab()
    {

    }

    public void DisplayDatabaseDebugTab()
    {
        _databaseQuery = EditorGUILayout.TextArea(_databaseQuery, GUILayout.Height(150f));

        if (GUILayout.Button("Input"))
        {
            _databaseAnswer = DataBase.ExecuteQuery(_databaseQuery);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Answer: " + _databaseAnswer);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate database"))
        {
            DataBase.GenerateDatabase();
        }
    }
}

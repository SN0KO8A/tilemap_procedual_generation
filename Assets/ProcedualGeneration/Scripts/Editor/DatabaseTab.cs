using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        if(GUILayout.Button("Load"))
        {
            //DataTable decorationTable = Database.GetTable("SELECT * FROM DecorationSettings;");
            //DecorationSettings decorationSettings = DecorationSettings.FromDataTable(decorationTable, 0);

            //GeneratorWindow.Instance.GeneratorTab.Init(new GlobalSettings(), new GraphicSettings(), new TerrainSettings(), decorationSettings);
        }
    }

    public void DisplayDatabaseDebugTab()
    {
        _databaseQuery = EditorGUILayout.TextArea(_databaseQuery, GUILayout.Height(150f));

        if (GUILayout.Button("Input"))
        {
            _databaseAnswer = Database.ExecuteQuery(_databaseQuery);

            if (string.IsNullOrEmpty(_databaseAnswer))
            {
                Debug.Log($"SQL answer: Done!");
            }
            else
            {
                Debug.Log($"SQL answer: {_databaseAnswer}");
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate database"))
        {
            Database.GenerateDatabase();
        }
    }

    public void LoadDataFromDatabase()
    {

    }

    public void DrawListElement(MapData data)
    {

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class DatabaseTab
{
    private string _databaseQuery;
    private string _databaseAnswer;

    private Vector2 _scroll = Vector2.zero;

    private List<MapData> _mapData = new List<MapData>();

    public void DisplayDatabaseTab()
    {
        DisplayHeader();

        _scroll = EditorGUILayout.BeginScrollView(_scroll, EditorStyles.helpBox, GUILayout.Height(500f));

        foreach (var item in _mapData.ToArray())
        {
            DisplayItem(item);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        if(GUILayout.Button("Update data"))
        {
            RefreshMapData();
        }
    }

    public void RefreshMapData()
    {
        _mapData = new List<MapData>();
        DataRowCollection data = Database.GetTable("SELECT * FROM Map;").Rows;

        foreach (DataRow row in data)
        {
            _mapData.Add(MapData.GetFromDataBase(row));
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

    private void DisplayHeader()
    {
        EditorGUILayout.Space();
        GUIStyle headerLabelStyle = new GUIStyle(GUI.skin.label);
        headerLabelStyle.fontSize = 14;
        headerLabelStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("DATABASE", headerLabelStyle);
        EditorGUILayout.Space();
    }

    public void DisplayItem(MapData mapData)
    {
        float labelWidth = EditorGUIUtility.labelWidth = 75f;
        EditorGUILayout.IntField("ID: ", mapData.ID);

        EditorGUILayout.BeginHorizontal();

        EditorGUIUtility.wideMode = true;
        EditorGUILayout.FloatField("Seed: ", mapData.Seed);

        Vector2 size = new Vector2(mapData.Settings.GlobalSettings.Width, mapData.Settings.GlobalSettings.Height);
        EditorGUILayout.Vector2Field("Size: ", size);
        EditorGUILayout.FloatField("Fill amount: ", mapData.Settings.TerrainSettings.FillAmount);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        string terrainTileAssetPath = AssetDatabase.GUIDToAssetPath(mapData.GraphicSettings.TerrainTileGUID);
        string backgroundTileAssetPath = AssetDatabase.GUIDToAssetPath(mapData.GraphicSettings.BackgroundTileGUID);
        string decorationTileAssetPath = AssetDatabase.GUIDToAssetPath(mapData.GraphicSettings.DecorationTileGUID);

        TileBase terrainTile = AssetDatabase.LoadAssetAtPath<TileBase>(terrainTileAssetPath);
        TileBase backgroundTile = AssetDatabase.LoadAssetAtPath<TileBase>(backgroundTileAssetPath);
        TileBase decorationTile = AssetDatabase.LoadAssetAtPath<TileBase>(decorationTileAssetPath);

        EditorGUILayout.ObjectField("Terrain: ", terrainTile, typeof(TileBase), false);
        EditorGUILayout.ObjectField("Background: ", backgroundTile, typeof(TileBase), false);
        EditorGUILayout.ObjectField("Decoration: ", decorationTile, typeof(TileBase), false);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if(GUILayout.Button("Delete"))
        {
            DeleteItem(mapData);
        }

        if (GUILayout.Button("Load"))
        {
            Load(mapData);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUIUtility.labelWidth = labelWidth;
    }

    public void DeleteItem(MapData mapData)
    {
        mapData.DeleteFromDatabase();
        _mapData.Remove(mapData);
    }

    public void Load(MapData mapData)
    {
        if (GeneratorWindow.Instance == null) return;

        GeneratorWindow.Instance.GeneratorTab.Init(mapData);
    }
}

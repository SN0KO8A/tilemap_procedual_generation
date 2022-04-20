using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneratorWindow : EditorWindow
{
    [SerializeField] private GeneratorTab _generatorTab;
    [SerializeField] private DatabaseTab _databaseTab;
    [SerializeField] private TabState _tabState;

    private string[] _tabs = { "Generator", "Database", "Database DEBUG" };
    private int _tabSelected = -1;

    [MenuItem("Tools/Tilemap Generator", false, 45)]
    public static void OpenWindow()
    {
        GeneratorWindow window = GetWindow<GeneratorWindow>();
        window.Init();
        window.Show();
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        _tabSelected = GUILayout.Toolbar(_tabSelected, _tabs);
        _tabState = (TabState)_tabSelected;
        //if (_tabState != TabState.GENERATOR)
        //{
        //    if (GUILayout.Button("Generator tab"))
        //    {
        //        _tabState = TabState.GENERATOR;
        //    }
        //}

        //else
        //{
        //    EditorGUILayout.LabelField("Generator tab", EditorStyles.);
        //}

        //if(GUILayout.Button("Database tab"))
        //{
        //    _tabState = TabState.DATABASE;
        //}

        //if (GUILayout.Button("Database DEBUG tab"))
        //{
        //    _tabState = TabState.DATABASE_DEBUG;
        //}
        EditorGUILayout.EndHorizontal();

        switch (_tabState)
        {
            case TabState.GENERATOR:
                _generatorTab.DrawGeneratorWindow();
                break;

            case TabState.DATABASE:
                _databaseTab.DisplayDatabaseTab();
                break;

            case TabState.DATABASE_DEBUG:
                _databaseTab.DisplayDatabaseDebugTab();
                break;
        }

    }

    public void Init()
    {
        _generatorTab = new GeneratorTab();
        _generatorTab.Init();
    }

    private enum TabState
    {
        GENERATOR,
        DATABASE,
        DATABASE_DEBUG
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneratorWindow : EditorWindow
{
    public static GeneratorWindow Instance;

    [SerializeField] private GeneratorTab _generatorTab;
    [SerializeField] private DatabaseTab _databaseTab;
    [SerializeField] private TabState _tabState;

    private string[] _tabs = { "Generator", "Database", "Database DEBUG" };
    private int _tabSelected = -1;

    public GeneratorTab GeneratorTab { get => _generatorTab; set => _generatorTab = value; }
    public DatabaseTab DatabaseTab { get => _databaseTab; set => _databaseTab = value; }

    [MenuItem("Window/2D/Tilemap Generator", false, 1)]
    public static void OpenWindow()
    {
        GeneratorWindow window = GetWindow<GeneratorWindow>();
        window.Init();
        window.Show();

        Instance = window;
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        _tabSelected = GUILayout.Toolbar(_tabSelected, _tabs);
        _tabState = (TabState)_tabSelected;

        EditorGUILayout.EndHorizontal();

        switch (_tabState)
        {
            case TabState.GENERATOR:
                _databaseTab.Initialized = false;
                _generatorTab.DisplayGeneratorTab();
                break;

            case TabState.DATABASE:
                _databaseTab.DisplayDatabaseTab();
                break;

            case TabState.DATABASE_DEBUG:
                _databaseTab.Initialized = false;
                _databaseTab.DisplayDatabaseDebugTab();
                break;
        }

    }

    public void Init()
    {
        _generatorTab = new GeneratorTab();
        _databaseTab = new DatabaseTab();
        _generatorTab.Init();
    }

    private enum TabState
    {
        GENERATOR,
        DATABASE,
        DATABASE_DEBUG
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class GeneratorTab
{
    [SerializeField] private Tilemap _terrainTilemap;
    [SerializeField] private Tilemap _backgroundTilemap;

    //Graphic settings
    [SerializeField] private TileBase _terrainTile;
    [SerializeField] private TileBase _backgroundTile;
    [SerializeField] private TileBase _decorationTile;

    [SerializeField] private GenerationCore _generationCore;
    [SerializeField] private TerrainGenerationCore _terrainGenerator;
    [SerializeField] private DecorationGenerator _decorationGenerator;

    [SerializeField] private MapData _map;

    [SerializeField] private GlobalSettings _globalSettings;
    [SerializeField] private GraphicSettings _graphicSettings;
    [SerializeField] private TerrainSettings _terrainSettings;
    [SerializeField] private DecorationSettings _decorationSettings;

    [SerializeField] private bool _debugMode;
    private Vector2 _scrollViewPos;
    private float _defaultLabelWidth;

    public void Init()
    {
        _map = new MapData();

        _globalSettings = new GlobalSettings();
        _graphicSettings = new GraphicSettings();
        _terrainSettings = new TerrainSettings();
        _decorationSettings = new DecorationSettings();

        _map.Settings.GlobalSettings = _globalSettings;
        _map.Settings.TerrainSettings = _terrainSettings;
        _map.Settings.DecorationSettings = _decorationSettings;
        _map.GraphicSettings = _graphicSettings;
    }

    public void Init(MapData mapData)
    {
        _map = mapData;

        _globalSettings = mapData.Settings.GlobalSettings;
        _graphicSettings = mapData.GraphicSettings;
        _terrainSettings = mapData.Settings.TerrainSettings;

        string terrainTileAssetPath = AssetDatabase.GUIDToAssetPath(_graphicSettings.TerrainTileGUID);
        string backgroundTileAssetPath = AssetDatabase.GUIDToAssetPath(_graphicSettings.BackgroundTileGUID);
        string decorationTileAssetPath = AssetDatabase.GUIDToAssetPath(_graphicSettings.DecorationTileGUID);

        _terrainTile = AssetDatabase.LoadAssetAtPath<TileBase>(terrainTileAssetPath);
        _backgroundTile = AssetDatabase.LoadAssetAtPath<TileBase>(backgroundTileAssetPath);
        _decorationTile = AssetDatabase.LoadAssetAtPath<TileBase>(decorationTileAssetPath);

        _decorationSettings = mapData.Settings.DecorationSettings;
    }

    public void DrawGeneratorWindow()
    {
        DisplayHeader();

        _scrollViewPos = EditorGUILayout.BeginScrollView(_scrollViewPos);
        _defaultLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.wideMode = true;

        _terrainTilemap = (Tilemap)EditorGUILayout.ObjectField("Terrain tilemap", _terrainTilemap, typeof(Tilemap), true);
        _backgroundTilemap = (Tilemap)EditorGUILayout.ObjectField("Background tilemap", _backgroundTilemap, typeof(Tilemap), true);
        _debugMode = EditorGUILayout.Toggle("DEBUG MODE", _debugMode);

        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(!_terrainTilemap);

        DisplayGraphicSettings();
        DisplayGlobalSettings();
        DisplayTerrainSettings();
        DisplayDecorationSettings();
        DisplayGenerateButton();
        DisplaySaveToDataBaseButton();

        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndScrollView();
    }

    public void InitGenerators()
    {
        if (_generationCore == null)
        {
            _generationCore = new GenerationCore(_globalSettings.Width, _globalSettings.Height);
            _generationCore.terrainTilemap = _terrainTilemap;
            _generationCore.backgroundTilemap = _backgroundTilemap;

            _generationCore.terrainTile = _terrainTile;
            _generationCore.backgroundTile = _backgroundTile;
            _generationCore.decorTile = _decorationTile;
        }

        if (_terrainGenerator == null)
        {
            _terrainGenerator = new TerrainGenerationCore(
                _terrainSettings.TerrainOffset,
                _terrainSettings.FillAmount,
                _terrainSettings.Iterations,
                _terrainSettings.EdgesAreWalls,
                _terrainSettings.HasWay,
                _terrainSettings.HeightOfWay
                );
        }

        if (_decorationGenerator == null)
        {
            _decorationGenerator = new DecorationGenerator(_decorationSettings.DecorationSetArea, _decorationSettings.ChanceToSpawn);
        }

        _generationCore.generators.Add(_terrainGenerator);
        _generationCore.generators.Add(_decorationGenerator);
    }

    private void DisplayGraphicSettings()
    {
        EditorGUILayout.LabelField("Graphic settings", EditorStyles.boldLabel);

        _terrainTile = (TileBase)EditorGUILayout.ObjectField("Terrain tile", _terrainTile, typeof(TileBase), false);
        _backgroundTile = (TileBase)EditorGUILayout.ObjectField("Background tile", _backgroundTile, typeof(TileBase), false);
        _decorationTile = (TileBase)EditorGUILayout.ObjectField("Decoration tile", _decorationTile, typeof(TileBase), false);

        if (_terrainTile != null)
            _graphicSettings.TerrainTileGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_terrainTile));

        if (_backgroundTile != null)
            _graphicSettings.BackgroundTileGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_backgroundTile));

        if (_decorationTile != null)
            _graphicSettings.DecorationTileGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_decorationTile));

        if (_debugMode)
        {
            EditorGUIUtility.labelWidth = 180f;

            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(true);
            _graphicSettings.TerrainTileGUID = EditorGUILayout.TextField("DEBUG: Terrain tile GUID", _graphicSettings.TerrainTileGUID, GUILayout.MinWidth(100f));
            _graphicSettings.BackgroundTileGUID = EditorGUILayout.TextField("DEBUG: Background tile GUID", _graphicSettings.BackgroundTileGUID, GUILayout.MinWidth(100f));
            _graphicSettings.DecorationTileGUID = EditorGUILayout.TextField("DEBUG: Decoration tile GUID", _graphicSettings.DecorationTileGUID, GUILayout.MinWidth(100f));
            EditorGUI.EndDisabledGroup();

            EditorGUIUtility.labelWidth = _defaultLabelWidth;
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DisplayGlobalSettings()
    {
        EditorGUILayout.LabelField("Global settings", EditorStyles.boldLabel);

        _globalSettings.Width = EditorGUILayout.IntField("Width", _globalSettings.Width);
        _globalSettings.Height = EditorGUILayout.IntField("Height", _globalSettings.Height);

        if (_debugMode)
        {
            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(true);
            _map.Seed = EditorGUILayout.FloatField("DEBUG: Random seed", _map.Seed);
            EditorGUI.EndDisabledGroup();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DisplayTerrainSettings()
    {
        EditorGUILayout.LabelField("Terrain settings", EditorStyles.boldLabel);

        _terrainSettings.EdgesAreWalls = EditorGUILayout.Toggle("Edges are walls", _terrainSettings.EdgesAreWalls);
        _terrainSettings.HasWay = EditorGUILayout.Toggle("Map has a way", _terrainSettings.HasWay);
        EditorGUILayout.Space();

        _terrainSettings.TerrainOffset = EditorGUILayout.Vector2IntField("Terrain offset", _terrainSettings.TerrainOffset);
        _terrainSettings.FillAmount = EditorGUILayout.Slider("Fill amount map", _terrainSettings.FillAmount, 0f, 100f);

        GUIContent iteraionFieldContent = new GUIContent("Iterations of algorithm", "The more of iteration of algroithm, the better quality of generation");
        _terrainSettings.Iterations = EditorGUILayout.IntField(iteraionFieldContent, _terrainSettings.Iterations);

        if (_terrainSettings.HasWay)
            _terrainSettings.HeightOfWay = EditorGUILayout.IntField("Height of the way", _terrainSettings.Iterations);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DisplayDecorationSettings()
    {
        EditorGUILayout.LabelField("Decoration settings", EditorStyles.boldLabel);

        _decorationSettings.DecorationSetArea = EditorGUILayout.Vector2IntField("Decoration set area", _decorationSettings.DecorationSetArea);
        _decorationSettings.ChanceToSpawn = EditorGUILayout.Slider("Chance to spawn", _decorationSettings.ChanceToSpawn, 0f, 100f);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DisplayGenerateButton()
    {
        if (GUILayout.Button("Generate"))
        {
            InitGenerators();
            _generationCore.Generate();
            _map.Seed = _generationCore.randomSeed;

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
    }

    private void DisplaySaveToDataBaseButton()
    {
        if (GUILayout.Button("Save to database"))
        {
            _map.ToDatabase();
            Debug.Log($"Map saved into databse with ID: {_map.ID}");
            Init();
        }

        //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
    }

    private void DisplayHeader()
    {
        EditorGUILayout.Space();
        GUIStyle headerLabelStyle = new GUIStyle(GUI.skin.label);
        headerLabelStyle.fontSize = 14;
        headerLabelStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("MAP GENERATOR", headerLabelStyle);
        EditorGUILayout.Space();
    }
}

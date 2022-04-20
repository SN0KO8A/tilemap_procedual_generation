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

    [SerializeField] private GlobalSettings _globalSettings;
    [SerializeField] private GraphicSettings _graphicSettings;
    [SerializeField] private TerrainSettings _terrainSettings;
    [SerializeField] private DecorationSettings _decorationSettings;

    [SerializeField] private bool _debugMode;
    private Vector2 _scrollViewPos;
    private float _defaultLabelWidth;

    public void Init()
    {
        if (_globalSettings == null)
            _globalSettings = new GlobalSettings();

        if (_graphicSettings == null)
            _graphicSettings = new GraphicSettings();

        if (_terrainSettings == null)
            _terrainSettings = new TerrainSettings();

        if (_decorationSettings == null)
            _decorationSettings = new DecorationSettings();
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

        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndScrollView();
    }

    public void InitGenerators()
    {
        _generationCore = new GenerationCore(_globalSettings.width, _globalSettings.height);
        _generationCore.terrainTilemap = _terrainTilemap;
        _generationCore.backgroundTilemap = _backgroundTilemap;

        _generationCore.terrainTile = _terrainTile;
        _generationCore.backgroundTile = _backgroundTile;
        _generationCore.decorTile = _decorationTile;

        _terrainGenerator = new TerrainGenerationCore(
            _terrainSettings.terrainOffset,
            _terrainSettings.fillAmount,
            _terrainSettings.iterations,
            _terrainSettings.edgesAreWalls,
            _terrainSettings.hasWay,
            _terrainSettings.heightOfWay
            );

        _decorationGenerator = new DecorationGenerator(_decorationSettings.decorationSetArea, _decorationSettings.chanceToSpawn);

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
            _graphicSettings.terrainTileGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_terrainTile));

        if (_backgroundTile != null)
            _graphicSettings.backgroundTileGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_backgroundTile));

        if (_decorationTile != null)
            _graphicSettings.decorationTileGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_decorationTile));

        if (_debugMode)
        {
            EditorGUIUtility.labelWidth = 180f;

            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(true);
            _graphicSettings.terrainTileGUID = EditorGUILayout.TextField("DEBUG: Terrain tile GUID", _graphicSettings.terrainTileGUID, GUILayout.MinWidth(100f));
            _graphicSettings.backgroundTileGUID = EditorGUILayout.TextField("DEBUG: Background tile GUID", _graphicSettings.backgroundTileGUID, GUILayout.MinWidth(100f));
            _graphicSettings.decorationTileGUID = EditorGUILayout.TextField("DEBUG: Decoration tile GUID", _graphicSettings.decorationTileGUID, GUILayout.MinWidth(100f));
            EditorGUI.EndDisabledGroup();

            EditorGUIUtility.labelWidth = _defaultLabelWidth;
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DisplayGlobalSettings()
    {
        EditorGUILayout.LabelField("Global settings", EditorStyles.boldLabel);

        _globalSettings.width = EditorGUILayout.IntField("Width", _globalSettings.width);
        _globalSettings.height = EditorGUILayout.IntField("Height", _globalSettings.height);

        if (_debugMode)
        {
            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(true);
            _globalSettings.seed = EditorGUILayout.FloatField("DEBUG: Random seed", _globalSettings.seed);
            EditorGUI.EndDisabledGroup();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DisplayTerrainSettings()
    {
        EditorGUILayout.LabelField("Terrain settings", EditorStyles.boldLabel);

        _terrainSettings.edgesAreWalls = EditorGUILayout.Toggle("Edges are walls", _terrainSettings.edgesAreWalls);
        _terrainSettings.hasWay = EditorGUILayout.Toggle("Map has a way", _terrainSettings.hasWay);
        EditorGUILayout.Space();

        _terrainSettings.terrainOffset = EditorGUILayout.Vector2IntField("Terrain offset", _terrainSettings.terrainOffset);
        _terrainSettings.fillAmount = EditorGUILayout.Slider("Fill amount map", _terrainSettings.fillAmount, 0f, 100f);

        GUIContent iteraionFieldContent = new GUIContent("Iterations of algorithm", "The more of iteration of algroithm, the better quality of generation");
        _terrainSettings.iterations = EditorGUILayout.IntField(iteraionFieldContent, _terrainSettings.iterations);

        if (_terrainSettings.hasWay)
            _terrainSettings.heightOfWay = EditorGUILayout.IntField("Height of the way", _terrainSettings.iterations);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DisplayDecorationSettings()
    {
        EditorGUILayout.LabelField("Decoration settings", EditorStyles.boldLabel);

        _decorationSettings.decorationSetArea = EditorGUILayout.Vector2IntField("Decoration set area", _decorationSettings.decorationSetArea);
        _decorationSettings.chanceToSpawn = EditorGUILayout.Slider("Chance to spawn", _decorationSettings.chanceToSpawn, 0f, 100f);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DisplayGenerateButton()
    {
        if (GUILayout.Button("Generate"))
        {
            InitGenerators();
            _generationCore.Generate();
            _globalSettings.seed = _generationCore.randomSeed;

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
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

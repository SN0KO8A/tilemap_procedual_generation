using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerationCore : MonoBehaviour
{
    public static GenerationCore Instance;

    [Header("Generators")]
    public List<Generator> generators;

    [Header("Input stuff")]
    public Tilemap terrainTilemap;
    public Tilemap backgroundTilemap;
    public TileBase terrainTile;
    public TileBase decorTile;
    public TileBase backgroundTile;

    [Header("Map Settings")]
    public float randomSeed = 0f;
    public int width;
    public int height;

    private System.Random _rand;
    private int[,] _map;

    public GenerationCore()
    {
        generators = new List<Generator>();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        randomSeed = Random.Range(0f, 100f);
        _rand = new System.Random(randomSeed.GetHashCode());
        _map = new int[width, height];
    }

    public void GenerateTerrain()
    {
        Init();

        Clear();
        foreach (var generator in generators)
        {
            generator.Generate(ref _map, _rand);
        }
        RenderTilemap();
    }

    public void Clear()
    {
        terrainTilemap.ClearAllTiles();
        backgroundTilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _map[x, y] = 0;
            }
        }
    }

    private void RenderTilemap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                if (_map[x, y] == 1)
                {
                    terrainTilemap.SetTile(tilePos, terrainTile);
                }

                else if (_map[x, y] == 2)
                {
                    terrainTilemap.SetTile(tilePos, decorTile);
                }

                if(backgroundTilemap)
                    backgroundTilemap.SetTile(tilePos, backgroundTile);
            }
        }
    }
}

[CustomEditor(typeof(GenerationCore))]
public class GenerationCoreEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GenerationCore generationCore = (GenerationCore)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("DEBUG BUTTONS", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate Terrain"))
        {
            generationCore.GenerateTerrain();
        }

        if (GUILayout.Button("Clear"))
        {
            generationCore.Clear();
        }
    }
}

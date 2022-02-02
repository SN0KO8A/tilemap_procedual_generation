using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData : IDatabaseEntity
{
    public GraphicSettings graphicSettings;
    public Settings settings;

    public MapData()
    {

    }

    public MapData(GraphicSettings graphicSettings, Settings settings)
    {
        this.graphicSettings = graphicSettings;
        this.settings = settings;
    }

    public int ID { get; set; }

    public void PushToDatabase(int dataBase)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class GraphicSettings : IDatabaseEntity
{
    public string terrainTileGUID;
    public string backgroundTileGUID;
    public string decorationTileGUID;

    public GraphicSettings()
    {

    }

    public GraphicSettings(string terrainTileGUID, string backgroundTileGUID, string decorationTileGUID)
    {
        this.terrainTileGUID = terrainTileGUID;
        this.backgroundTileGUID = backgroundTileGUID;
        this.decorationTileGUID = decorationTileGUID;
    }

    public int ID { get; set; }

    public void PushToDatabase(int dataBase)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class Settings : IDatabaseEntity
{
    public GlobalSettings globalSettings;
    public TerrainSettings terrainSettings;
    public DecorationSettings decorationSettings;

    public Settings()
    {
    }

    public Settings(GlobalSettings globalSettings, TerrainSettings terrainSettings, DecorationSettings decorationSettings)
    {
        this.globalSettings = globalSettings;
        this.terrainSettings = terrainSettings;
        this.decorationSettings = decorationSettings;
    }

    public int ID { get; set; }

    public void PushToDatabase(int dataBase)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class GlobalSettings : IDatabaseEntity
{
    public float seed;
    public int width;
    public int height;

    public GlobalSettings()
    {
    }

    public GlobalSettings(float seed, int width, int height)
    {
        this.seed = seed;
        this.width = width;
        this.height = height;
    }

    public int ID { get; set; }

    public void PushToDatabase(int dataBase)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class TerrainSettings : IDatabaseEntity
{
    public Vector2Int terrainOffset;
    public float fillAmount;
    public int iterations;
    public bool edgesAreWalls;
    public bool hasWay;
    public int heightOfWay;

    public TerrainSettings()
    {

    }

    public TerrainSettings(Vector2Int terrainOffset, float fillAmount, int iterations, bool edgesAreWalls, bool hasWay, int heightOfWay)
    {
        this.terrainOffset = terrainOffset;
        this.fillAmount = fillAmount;
        this.iterations = iterations;
        this.edgesAreWalls = edgesAreWalls;
        this.hasWay = hasWay;
        this.heightOfWay = heightOfWay;
    }

    public int ID { get; set; }

    public void PushToDatabase(int dataBase)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class DecorationSettings : IDatabaseEntity
{
    public Vector2Int decorationSetArea;
    public float chanceToSpawn;

    public DecorationSettings()
    {

    }

    public DecorationSettings(Vector2Int decorationSetArea, float chanceToSpawn)
    {
        this.decorationSetArea = decorationSetArea;
        this.chanceToSpawn = chanceToSpawn;
    }

    public int ID { get; set; }

    public void PushToDatabase(int dataBase)
    {
        throw new NotImplementedException();
    }
}

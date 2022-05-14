using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[Serializable]
public class MapData : IDatabaseEntity
{
    private int _id;
    private float _seed;
    private GraphicSettings _graphicSettings;
    private Settings _settings;

    public MapData()
    {
        this._id = -1;
        _settings = new Settings();
        _graphicSettings = new GraphicSettings();

        _settings.TerrainSettings = new TerrainSettings();
        _settings.GlobalSettings = new GlobalSettings();
        _settings.DecorationSettings = new DecorationSettings();
    }

    public MapData(float seed, GraphicSettings graphicSettings, Settings settings, int ID = -1)
    {
        this._id = ID;
        this._seed = seed;
        this._graphicSettings = graphicSettings;
        this._settings = settings;

        _settings = new Settings();
        _graphicSettings = new GraphicSettings();

        _settings.TerrainSettings = new TerrainSettings();
        _settings.GlobalSettings = new GlobalSettings();
        _settings.DecorationSettings = new DecorationSettings();
    }

    public int ID { get => _id; set => _id = value; }
    public float Seed { get => _seed; set => _seed = value; }
    public GraphicSettings GraphicSettings { get => _graphicSettings; set => _graphicSettings = value; }
    public Settings Settings { get => _settings; set => _settings = value; }

    public void DeleteFromDatabase()
    {
        Database.OpenConnection();
        string result = string.Empty;

        result = Database.ExecuteQuerryWithoutConnection($"DELETE FROM Map WHERE ID='{_id}'");

        Database.CloseConnection();

        _graphicSettings.DeleteFromDatabase();
        _settings.DeleteFromDatabase();
    }

    public static MapData GetFromDataBase(DataRow dataRow)
    {
        MapData mapData = new MapData();

        mapData.ID = int.Parse(dataRow["ID"].ToString());
        mapData.Seed = int.Parse(dataRow["seed"].ToString());

        int graphicID = int.Parse(dataRow["graphic"].ToString());
        int settingsID = int.Parse(dataRow["settings"].ToString());

        DataRow graphicDataRow = Database.GetTable($"SELECT * FROM Graphic WHERE ID='{graphicID}'").Rows[0];
        DataRow settingsDataRow = Database.GetTable($"SELECT * FROM Settings WHERE ID='{settingsID}'").Rows[0];

        mapData.GraphicSettings = GraphicSettings.FromDatabase(graphicDataRow);
        mapData.Settings = Settings.FromDatabase(settingsDataRow);

        return mapData;
    }

    public void ToDatabase()
    {
        string result = string.Empty;

        if(_graphicSettings.ID == -1)
            _graphicSettings.ToDatabase();

        if (_settings.ID == -1)
            _settings.ToDatabase();

        Database.OpenConnection();

        if (_id == -1)
        {
            result = Database.ExecuteQuerryWithoutConnection($"INSERT INTO Map (seed, graphic, settings) " +
                $"VALUES({_seed}, {_graphicSettings.ID}, {_settings.ID}); ");
            _id = int.Parse(Database.ExecuteQuerryWithoutConnection("SELECT LAST_INSERT_ROWID()"));
        }

        Database.CloseConnection();
    }
}

[Serializable]
public class GraphicSettings : IDatabaseEntity
{
    private int _id;
    private string _terrainTileGUID;
    private string _backgroundTileGUID;
    private string _decorationTileGUID;

    public GraphicSettings()
    {
        this._id = -1;
    }

    public GraphicSettings(string terrainTileGUID, string backgroundTileGUID, string decorationTileGUID, int ID = -1)
    {
        this._id = ID;
        this._terrainTileGUID = terrainTileGUID;
        this._backgroundTileGUID = backgroundTileGUID;
        this._decorationTileGUID = decorationTileGUID;
    }

    public int ID { get => _id; set => _id = value; }
    public string TerrainTileGUID { get => _terrainTileGUID; set => _terrainTileGUID = value; }
    public string BackgroundTileGUID { get => _backgroundTileGUID; set => _backgroundTileGUID = value; }
    public string DecorationTileGUID { get => _decorationTileGUID; set => _decorationTileGUID = value; }

    public void DeleteFromDatabase()
    {
        Database.OpenConnection();
        string result = string.Empty;

        result = Database.ExecuteQuerryWithoutConnection($"DELETE FROM Graphic WHERE ID='{_id}'");

        Database.CloseConnection();
    }

    public static GraphicSettings FromDatabase(DataRow dataRow)
    {
        GraphicSettings globalSettings = new GraphicSettings();

        globalSettings.ID = int.Parse(dataRow["ID"].ToString());
        globalSettings.TerrainTileGUID = dataRow["rule_tile_id"].ToString();
        globalSettings.BackgroundTileGUID = dataRow["background_tile_id"].ToString();
        globalSettings.DecorationTileGUID = dataRow["decoration_tile_id"].ToString();

        return globalSettings;
    }

    public void ToDatabase()
    {
        Database.OpenConnection();
        string result = string.Empty;

        if (_id == -1)
        {
            result = Database.ExecuteQuerryWithoutConnection($"INSERT INTO Graphic (rule_tile_id, background_tile_id, decoration_tile_id) " +
                $"VALUES('{_terrainTileGUID}', '{_backgroundTileGUID}', '{_decorationTileGUID}'); ");
            _id = int.Parse(Database.ExecuteQuerryWithoutConnection("SELECT LAST_INSERT_ROWID()"));
        }

        Database.CloseConnection();
    }
}

[Serializable]
public class Settings : IDatabaseEntity
{
    private int _id;
    private GlobalSettings _globalSettings;
    private TerrainSettings _terrainSettings;
    private DecorationSettings _decorationSettings;

    public Settings()
    {
        this._id = -1;
    }

    public Settings(GlobalSettings globalSettings, TerrainSettings terrainSettings, DecorationSettings decorationSettings, int ID = -1)
    {
        this._id = ID;
        this._globalSettings = globalSettings;
        this._terrainSettings = terrainSettings;
        this._decorationSettings = decorationSettings;
    }

    public int ID { get => _id; set => _id = value; }
    public GlobalSettings GlobalSettings { get => _globalSettings; set => _globalSettings = value; }
    public TerrainSettings TerrainSettings { get => _terrainSettings; set => _terrainSettings = value; }
    public DecorationSettings DecorationSettings { get => _decorationSettings; set => _decorationSettings = value; }

    public void DeleteFromDatabase()
    {
        Database.OpenConnection();
        string result = string.Empty;

        result = Database.ExecuteQuerryWithoutConnection($"DELETE FROM Settings WHERE ID='{_id}'");

        Database.CloseConnection();

        _globalSettings.DeleteFromDatabase();
        _terrainSettings.DeleteFromDatabase();
        _decorationSettings.DeleteFromDatabase();
    }

    public static Settings FromDatabase(DataRow dataRow)
    {
        Settings settings = new Settings();

        settings.ID = int.Parse(dataRow["ID"].ToString());

        int globalSettingsID = int.Parse(dataRow["global_settings"].ToString());
        int terrainSettingsID = int.Parse(dataRow["terrain_settings"].ToString());
        int decorationSettingsID = int.Parse(dataRow["decoration_settings"].ToString());

        DataRow globalSettingsDataRow = Database.GetTable($"SELECT * FROM GlobalSettings WHERE ID='{globalSettingsID}'").Rows[0];
        DataRow terrainSettingsDataRow = Database.GetTable($"SELECT * FROM TerrainSettings WHERE ID='{terrainSettingsID}'").Rows[0];
        DataRow decoratoinSettingsDataRow = Database.GetTable($"SELECT * FROM DecorationSettings WHERE ID='{decorationSettingsID}'").Rows[0];

        settings.GlobalSettings = GlobalSettings.FromDataBase(globalSettingsDataRow);
        settings.TerrainSettings = TerrainSettings.FromDatabase(terrainSettingsDataRow);
        settings.DecorationSettings = DecorationSettings.FromDatabase(decoratoinSettingsDataRow);

        return settings;
    }

    public void ToDatabase()
    {
        string result = string.Empty;

        if(_globalSettings.ID == -1)
            _globalSettings.ToDatabase();

        if (_terrainSettings.ID == -1)
            _terrainSettings.ToDatabase();

        if(_decorationSettings.ID == -1)
            _decorationSettings.ToDatabase();

        Database.OpenConnection();

        if (_id == -1)
        {
            result = Database.ExecuteQuerryWithoutConnection($"INSERT INTO Settings (global_settings, terrain_settings, decoration_settings) " +
                $"VALUES({_globalSettings.ID}, {_terrainSettings.ID}, {_decorationSettings.ID}); ");
            _id = int.Parse(Database.ExecuteQuerryWithoutConnection("SELECT LAST_INSERT_ROWID()"));
        }

        Database.CloseConnection();
    }
}

[Serializable]
public class GlobalSettings : IDatabaseEntity
{
    private int _id;
    private int _width;
    private int _height;

    public GlobalSettings()
    {
        this._id = -1;
    }

    public GlobalSettings(int width, int height, int ID = -1)
    {
        this._id = ID;
        this._width = width;
        this._height = height;
    }

    public int ID { get => _id; set => _id = value; }
    public int Width { get => _width; set => _width = value; }
    public int Height { get => _height; set => _height = value; }

    public void DeleteFromDatabase()
    {
        Database.OpenConnection();
        string result = string.Empty;

        result = Database.ExecuteQuerryWithoutConnection($"DELETE FROM GlobalSettings WHERE ID='{_id}'");

        Database.CloseConnection();
    }

    public static GlobalSettings FromDataBase(DataRow dataRow)
    {
        GlobalSettings globalSettings = new GlobalSettings();

        globalSettings.ID = int.Parse(dataRow["ID"].ToString());
        globalSettings.Width = int.Parse(dataRow["width"].ToString());
        globalSettings.Height = int.Parse(dataRow["height"].ToString());

        return globalSettings;
    }

    public void ToDatabase()
    {
        Database.OpenConnection();
        string result = string.Empty;

        if (_id == -1)
        {
            result = Database.ExecuteQuerryWithoutConnection($"INSERT INTO GlobalSettings (width, height) " +
                $"VALUES({_width}, {_height}); ");
            _id = int.Parse(Database.ExecuteQuerryWithoutConnection("SELECT LAST_INSERT_ROWID()"));
        }

        Database.CloseConnection();
    }
}

[Serializable]
public class TerrainSettings : IDatabaseEntity
{
    private int _id;
    private Vector2Int _terrainOffset;
    private float _fillAmount;
    private int _iterations;
    private bool _edgesAreWalls;
    private bool _hasWay;
    private int _heightOfWay;

    public TerrainSettings()
    {
        this._id = -1;
    }

    public TerrainSettings(Vector2Int terrainOffset, float fillAmount, int iterations, bool edgesAreWalls, bool hasWay, int heightOfWay, int ID = -1)
    {
        this._id = ID;
        this._terrainOffset = terrainOffset;
        this._fillAmount = fillAmount;
        this._iterations = iterations;
        this._edgesAreWalls = edgesAreWalls;
        this._hasWay = hasWay;
        this._heightOfWay = heightOfWay;
    }

    public Vector2Int TerrainOffset { get => _terrainOffset; set => _terrainOffset = value; }
    public float FillAmount { get => _fillAmount; set => _fillAmount = value; }
    public int Iterations { get => _iterations; set => _iterations = value; }
    public int ID { get => _id; set => _id = value; }
    public bool HasWay { get => _hasWay; set => _hasWay = value; }
    public int HeightOfWay { get => _heightOfWay; set => _heightOfWay = value; }
    public bool EdgesAreWalls { get => _edgesAreWalls; set => _edgesAreWalls = value; }

    public void DeleteFromDatabase()
    {
        Database.OpenConnection();
        string result = string.Empty;

        result = Database.ExecuteQuerryWithoutConnection($"DELETE FROM TerrainSettings WHERE ID='{_id}'");

        Database.CloseConnection();
    }

    public static TerrainSettings FromDatabase(DataRow dataRow)
    {
        TerrainSettings terrainSettings = new TerrainSettings();

        terrainSettings.ID = int.Parse(dataRow["ID"].ToString());
        Vector2Int terrainOffset = new Vector2Int(
            int.Parse(dataRow["terrain_offset_x"].ToString()),
            int.Parse(dataRow["terrain_offset_y"].ToString())
            );

        terrainSettings.TerrainOffset = terrainOffset;
        terrainSettings.FillAmount = float.Parse(dataRow["fill_amount"].ToString());
        terrainSettings.Iterations = int.Parse(dataRow["moor_iterations"].ToString());
        terrainSettings.HasWay = bool.Parse(dataRow["has_way"].ToString());
        terrainSettings.HeightOfWay = int.Parse(dataRow["height_of_way"].ToString());
        terrainSettings.EdgesAreWalls = bool.Parse(dataRow["edges_are_walls"].ToString());

        return terrainSettings;
    }

    public void ToDatabase()
    {
        Database.OpenConnection();
        string result = string.Empty;

        if (_id == -1)
        {
            result = Database.ExecuteQuerryWithoutConnection($"INSERT INTO TerrainSettings (terrain_offset_x, terrain_offset_y, fill_amount," +
                $" moor_iterations, edges_are_walls, has_way, height_of_way) " +
                $"VALUES({_terrainOffset.x}, {_terrainOffset.y},{_fillAmount}, {_iterations}, {_edgesAreWalls}, {_hasWay}, {_heightOfWay}); ");
            _id = int.Parse(Database.ExecuteQuerryWithoutConnection("SELECT LAST_INSERT_ROWID()"));
        }

        Database.CloseConnection();
    }
}

[Serializable]
public class DecorationSettings : IDatabaseEntity
{
    private Vector2Int _decorationSetArea;
    private float _chanceToSpawn;
    private int _id;

    public DecorationSettings()
    {
        _id = -1;
    }

    public DecorationSettings(Vector2Int decorationSetArea, float chanceToSpawn, int ID = -1)
    {
        this._id = ID;
        this._decorationSetArea = decorationSetArea;
        this._chanceToSpawn = chanceToSpawn;
    }

    public Vector2Int DecorationSetArea { get => _decorationSetArea; set => _decorationSetArea = value; }
    public float ChanceToSpawn { get => _chanceToSpawn; set => _chanceToSpawn = value; }
    public int ID { get => _id; set => _id = value; }

    public static DecorationSettings FromDatabase(DataRow dataRow)
    {
        DecorationSettings decorationSettings = new DecorationSettings();

        decorationSettings.ID = int.Parse(dataRow["ID"].ToString());
        decorationSettings.ChanceToSpawn = float.Parse(dataRow["chance_to_spawn"].ToString());
        decorationSettings.DecorationSetArea = new Vector2Int(int.Parse(dataRow["width_to_spawn"].ToString()), 
            int.Parse(dataRow["height_to_spawn"].ToString()));

        return decorationSettings;
    }

    public void DeleteFromDatabase()
    {
        Database.OpenConnection();
        string result = string.Empty;

        result = Database.ExecuteQuerryWithoutConnection($"DELETE FROM DecorationSettings WHERE ID='{_id}'");

        Database.CloseConnection();
    }

    public void ToDatabase()
    {
        Database.OpenConnection();
        string result = string.Empty;

        if (_id == -1)
        {
            result = Database.ExecuteQuerryWithoutConnection($"INSERT INTO DecorationSettings (chance_to_spawn, height_to_spawn, width_to_spawn) " +
                $"VALUES({ChanceToSpawn}, {DecorationSetArea.y},{DecorationSetArea.x}); ");
            _id = int.Parse(Database.ExecuteQuerryWithoutConnection("SELECT LAST_INSERT_ROWID()"));
        }

        Database.CloseConnection();
    } 
}

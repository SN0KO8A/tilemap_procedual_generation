using UnityEngine;
using System.IO;
using Mono.Data.Sqlite;
using System.Data;
using UnityEditor;
using System.Threading;

public class Database
{
    private const string fileName = "db.bytes";
    private static string DBPath;
    private static SqliteConnection connection;
    private static SqliteCommand command;

    public bool HasConnection => connection != null;


    static Database()
    {
        DBPath = GetDatabasePath();
        //UnpackDatabase(DBPath);
    }

    private static string GetDatabasePath()
    {
        return Path.Combine("Assets\\ProcedualGeneration\\Database", fileName);
    }

    private static void UnpackDatabase(string toPath)
    {
        WWW reader = new WWW(toPath);
        while (!reader.isDone) { }

        File.WriteAllBytes(toPath, reader.bytes);
    }

    public static void OpenConnection()
    {
        connection = new SqliteConnection("Data Source=" + DBPath);
        command = new SqliteCommand(connection);
        connection.Open();
    }

    public static void GenerateDatabase()
    {
        EditorUtility.DisplayProgressBar("Creating DB...", "Creating database file", 0f);
        UnpackDatabase(DBPath);

        EditorUtility.DisplayProgressBar("Creating DB...", "Creating tables in the database", 1f);
        GenerateTables();
        EditorUtility.ClearProgressBar();
    }

    public static void GenerateTables()
    {
        ExecuteQuery("CREATE TABLE IF NOT EXISTS \"DecorationSettings\" " +
            "(\"ID\" INTEGER NOT NULL UNIQUE, \"chance_to_spawn\"" +
            "    FLOAT NOT NULL, \"height_to_spawn\"" +
            "   INTEGER NOT NULL, \"width_to_spawn\"" +
            "    INTEGER NOT NULL, PRIMARY KEY(\"ID\"" +
            " AUTOINCREMENT))");

        ExecuteQuery("CREATE TABLE IF NOT EXISTS \"GlobalSettings\" (" +
            "   \"ID\"    INTEGER NOT NULL UNIQUE," +
            "   \"width\" FLOAT NOT NULL," +
            "   \"height\"    FLOAT NOT NULL," +
            "   PRIMARY KEY(\"ID\" AUTOINCREMENT))");

        ExecuteQuery("CREATE TABLE IF NOT EXISTS \"TerrainSettings\" " +
            "(\"ID\"    INTEGER NOT NULL UNIQUE," +
            " \"terrain_offset_x\"  INTEGER NOT NULL," +
            " \"terrain_offset_y\"  INTEGER NOT NULL," +
            " \"fill_amount\"   INTEGER NOT NULL," +
            " \"moor_iterations\"   INTEGER NOT NULL," +
            " \"edges_are_walls\"   BOOL NOT NULL," +
            " \"has_way\"   BOOL NOT NULL," +
            " \"height_of_way\" INTEGER," +
            " PRIMARY KEY(\"ID\" AUTOINCREMENT))");

        ExecuteQuery("CREATE TABLE IF NOT EXISTS \"Settings\" " +
            "(\"ID\"    INTEGER NOT NULL UNIQUE," +
            "\"global_settings\"   INTEGER NOT NULL," +
            "\"terrain_settings\"  INTEGER NOT NULL," +
            "\"decoration_settings\"   INTEGER NOT NULL," +
            "FOREIGN KEY(\"global_settings\") REFERENCES \"GlobalSettings\"(\"ID\")," +
            "FOREIGN KEY(\"terrain_settings\") REFERENCES \"TerrainSettings\"(\"ID\")," +
            "FOREIGN KEY(\"decoration_settings\") REFERENCES \"DecorationSettings\"(\"ID\")," +
            "PRIMARY KEY(\"ID\" AUTOINCREMENT))");

        ExecuteQuery("CREATE TABLE IF NOT EXISTS \"Graphic\" " +
            "(\"ID\"    INTEGER NOT NULL UNIQUE," +
            "\"rule_tile_id\"  CHAR(32) NOT NULL," +
            "\"background_tile_id\"    CHAR(32) NOT NULL," +
            "\"decoration_tile_id\"    CHAR(32) NOT NULL," +
            "PRIMARY KEY(\"ID\" AUTOINCREMENT))");

        ExecuteQuery("CREATE TABLE IF NOT EXISTS \"Map\" " +
            "(\"ID\"    INTEGER NOT NULL UNIQUE," +
            "\"seed\"  FLOAT NOT NULL," +
            "\"graphic\"   INTEGER NOT NULL," +
            "\"settings\"  INTEGER NOT NULL," +
            "FOREIGN KEY(\"graphic\") REFERENCES \"Graphic\"(\"ID\")," +
            "FOREIGN KEY(\"settings\") REFERENCES \"Settings\"(\"ID\")," +
            "PRIMARY KEY(\"ID\" AUTOINCREMENT))");
    }

    public static void CloseConnection()
    {
        connection.Close();
        command.Dispose();
    }

    public static string ExecuteQuery(string query)
    {
        OpenConnection();

        command.CommandText = query;
        //command.ExecuteNonQuery();
        object answer = command.ExecuteScalar();
        CloseConnection();

        if (answer != null)
            return answer.ToString();
        else
            return null;
    }

    public static string ExecuteQuerryWithoutConnection(string query)
    {
        command.CommandText = query;
        //command.ExecuteNonQuery();
        object answer = command.ExecuteScalar();

        if (answer != null)
            return answer.ToString();
        else
            return null;
    }

    public static DataTable GetTable(string query)
    {
        OpenConnection();

        SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);

        DataSet DS = new DataSet();
        adapter.Fill(DS);
        adapter.Dispose();

        CloseConnection();

        return DS.Tables[0];
    }
}

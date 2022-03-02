using UnityEngine;
using System.IO;
using Mono.Data.Sqlite;

public class DataBase
{
    private const string fileName = "db.bytes";
    private static string DBPath;
    private static SqliteConnection connection;
    private static SqliteCommand command;

    static DataBase()
    {
        DBPath = GetDatabasePath();
    }

    private static string GetDatabasePath()
    {
        return Path.Combine(Application.streamingAssetsPath, fileName);
    }

    private static void UnpackDatabase(string toPath)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

        WWW reader = new WWW(fromPath);
        while (!reader.isDone) { }

        File.WriteAllBytes(toPath, reader.bytes);
    }
}

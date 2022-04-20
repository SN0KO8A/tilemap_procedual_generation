using UnityEngine;
using System.IO;
using Mono.Data.Sqlite;
using System.Data;

public class DataBase
{
    private const string fileName = "db.bytes";
    private static string DBPath;
    private static SqliteConnection connection;
    private static SqliteCommand command;

    static DataBase()
    {
        DBPath = GetDatabasePath();
        UnpackDatabase(DBPath);
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

    private static void OpenConnection()
    {
        connection = new SqliteConnection("Data Source=" + DBPath);
        command = new SqliteCommand(connection);
        connection.Open();
    }

    public static void GenerateDatabase()
    {
        UnpackDatabase(DBPath);
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
        command.ExecuteNonQuery();
        object answer = command.ExecuteScalar();
        CloseConnection();

        if(answer != null)
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

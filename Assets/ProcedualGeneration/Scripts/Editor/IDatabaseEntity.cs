using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDatabaseEntity
{
    public int ID
    {
        get; set;
    }

    public void PushToDatabase(int dataBase);
}

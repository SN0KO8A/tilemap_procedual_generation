using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public interface IDatabaseEntity
{
    public void ToDatabase();
    public void DeleteFromDatabase();
}

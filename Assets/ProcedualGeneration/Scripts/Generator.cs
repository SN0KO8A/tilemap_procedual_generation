using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator : MonoBehaviour
{   
    //For generate
    protected System.Random rand;
    protected int[,] map;

    protected int _width;
    protected int _height;

    public virtual void Generate(ref int[,] map, System.Random rand)
    {
        this.map = map;
        this.rand = rand;

        _width = map.GetUpperBound(0);
        _height = map.GetUpperBound(1);
    }
}

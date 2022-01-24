using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DecorationGenerator : Generator
{
    public static int decorationID = 2;
    [SerializeField] private Vector2 _decorationSetDistance;
    [SerializeField, Range(0f,100f)] private float _chanceToSpawn;

    public override void Generate(ref int[,] map, System.Random rand)
    {
        base.Generate(ref map, rand);
        SetDecorations();
    }

    private void SetDecorations()
    {
        for (int i = 0; i < map.GetUpperBound(0); i++)
        {
            for (int j = 0; j < map.GetUpperBound(1); j++)
            {
                if (CheckForPlace(i, j) && Random.Range(0f, 100f) <= _chanceToSpawn)
                {
                    map[i, j] = 2;
                }
            }
        }
    }

    private bool CheckForPlace(int x, int y)
    {
        if(x - _decorationSetDistance.x < 0 || x + _decorationSetDistance.x > map.GetUpperBound(0) ||
            y - _decorationSetDistance.y < 0 || y + _decorationSetDistance.y > map.GetUpperBound(1))
        {
            return false;
        }

        for (int i = x - (int)_decorationSetDistance.x; i < x + _decorationSetDistance.x; i++)
        {
            for (int j = y - (int)_decorationSetDistance.y; j < y; j++)
            {
                if(map[i,j] == 0)
                {
                    return false;
                }
            }

            for (int j = y; j < y + _decorationSetDistance.y; j++)
            {
                if (map[i, j] != 0)
                {
                    return false;
                }
            }
        }

        return true;
    }
}

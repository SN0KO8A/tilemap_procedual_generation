using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerationCore : Generator
{
    [Header("Terrain Settings")]
    [SerializeField] private Vector2Int _terrainOffset;
    [SerializeField, Range(35, 100)] float _fillAmount;
    [Tooltip("More iterations - more quality, but more time for generation")]
    [SerializeField] private int _iterations = 3;
    [SerializeField] private bool _edgesAreWalls = true;
    [SerializeField] private int _heightOfWay = 3;

    private int startY;
    private int newY;

    public override void Generate(ref int [,] map, System.Random rand)
    {
        base.Generate(ref map, rand);

        InitRandomMap();
        CreateWay();
        MooreAlgorithm();
        SetStartAndEndPosition();
    }

    private void InitRandomMap()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if ((x >= _terrainOffset.x && x < _width - _terrainOffset.x) &&
                    (y >= _terrainOffset.y && y < _height - _terrainOffset.y))
                {
                    map[x, y] = rand.Next(0, 100) < _fillAmount ? 1 : 0;
                }

                else
                {
                    map[x, y] = 1;
                }
            }
        }
    }

    private void CreateWay()
    {
        startY = rand.Next(_terrainOffset.y, _height - _heightOfWay - _terrainOffset.y);
        newY = startY;

        for (int x = _terrainOffset.x; x < _width - _terrainOffset.x; x++)
        {
            for (int y = newY; y < newY +  _heightOfWay; y++)
            {
                map[x, y] = 0;
            }

            int randomY = rand.Next(-1, 2);

            if(newY + randomY >= _height - _terrainOffset.y - _heightOfWay)
            {
                randomY--;
            }

            else if(newY + randomY < _terrainOffset.y + _heightOfWay)
            {
                randomY++;
            }

            newY += randomY;
        }
    }

    private int GetMooreSurroundingsTiles(int x, int y)
    {
        int tileCount = 0;

        for(int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for(int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                bool inBounds = neighbourX >= 0 && neighbourX < _width && neighbourY >= 0 && neighbourY < _height;
                if (inBounds && (neighbourX != x || neighbourY != x))
                {
                    tileCount += map[neighbourX, neighbourY];
                }
            }
        }

        return tileCount;
    }

    private void MooreAlgorithm()
    {
        for (int i = 0; i < _iterations; i++)
        {
            for (int x = _terrainOffset.x; x < _width - _terrainOffset.x; x++)
            {
                for (int y = _terrainOffset.y; y < _height - _terrainOffset.y; y++)
                {
                    int surroundingsTiles = GetMooreSurroundingsTiles(x, y);

                    if(_edgesAreWalls && (x == 0 || x == _width - 1 || y == 0 || y == _height -1))
                    {
                        map[x, y] = 1;
                    }
                    else if(surroundingsTiles == 4)
                    {
                        continue;
                    }
                    else if(surroundingsTiles > 4)
                    {
                        map[x, y] = 1;
                    }
                    else if(surroundingsTiles < 4)
                    {
                        map[x, y] = 0;
                    }
                }
            }
        }
    }

    private void SetStartAndEndPosition()
    {
        //Debug.Log($"Start in {_terrainOffset.x + 5} and {startY + _heightOfWay / 2}");
        //Debug.Log($"End in {_width - _terrainOffset.x - 5} and {newY + _heightOfWay / 2}");
        map[_terrainOffset.x + 5, startY + _heightOfWay / 2] = GenerationCore.Instance.startPointCode;
        map[_width - _terrainOffset.x - 5, newY + _heightOfWay / 2] = GenerationCore.Instance.endPointCode;
    }
}

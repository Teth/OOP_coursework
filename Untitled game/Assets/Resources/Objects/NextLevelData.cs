using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NextData", menuName = "ScriptableObjects/NextData", order = 2)]
public class NextLevelData : ScriptableObject
{

    Vector2Int minSize = new Vector2Int(30, 30);

    Locations locations;

    MapType type;
    
    Vector2Int mapsize;

    public void GenerateNextLevelData()
    {
        locations = (Locations)Random.Range(0, 3);
        type = (MapType)Random.Range(0, 3);
        mapsize = new Vector2Int(minSize.x + (int)(PlayerPrefs.GetInt("LevelCleared") * 1.3), minSize.y + (int)(PlayerPrefs.GetInt("LevelCleared") * 1.3));
    }

    public Locations GetLocation()
    {
        return locations;
    }

    public MapType GetMapType()
    {
        return type;
    }

    public Vector2Int GetMapSize()
    {
        return mapsize;
    }
}


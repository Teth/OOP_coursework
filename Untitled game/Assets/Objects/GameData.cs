using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [SerializeField]
    int levelCleared;
    //List<Items>

    public void IncrementLevel()
    {
        levelCleared++;
    }

    public void Reset()
    {
        levelCleared = 0;
    }

    public int LevelCleared()
    {
        return levelCleared; 
    }

    public float GetSpawnRate()
    {
        return 0.0001f * Mathf.Pow(levelCleared,2);
    }
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NextLevelData", order = 1)]
public class NextLevelData : ScriptableObject
{
    [SerializeField]
    GameData gameData;

    [SerializeField]
    Vector2Int minSize;

    Locations locations;

    MapType type;

    Vector2Int mapsize;
    
    public void GenerateNextLevelData()
    {
        locations = (Locations)Random.Range(0, 3);
        type = (MapType)Random.Range(0, 3);
        mapsize = new Vector2Int(minSize.x + (int)(gameData.LevelCleared() * 1.3), minSize.y + (int)(gameData.LevelCleared() * 1.3));
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

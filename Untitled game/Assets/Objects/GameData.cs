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

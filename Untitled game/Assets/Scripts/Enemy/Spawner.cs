using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner
{
    public void Spawn(GameMap map, Tileset tileset)
    {
        Locations mapType = StaticTestSettings.getLocation();
        List<Vector2Int> spawnPositions = GetSpawnPositions(map, tileset);
        GameObject enemyObject;
        AbstractEnemyFactory enemyFactory;
        foreach (var spawnPosition in spawnPositions)
        {
            enemyFactory = GetRandomFactoryOnLocation(mapType);
            enemyObject = IsEnemyRangedOrMelee() ? enemyFactory.CreateRangedEnemy() : enemyFactory.CreateMeleeEnemy();
            enemyObject.transform.position = new Vector2(spawnPosition.x + 0.5f, spawnPosition.y + 0.5f);
        }

    }
    List<Vector2Int> GetSpawnPositions(GameMap map, Tileset tileset)
    {
        List<Vector2Int> spawnPositions = new List<Vector2Int>();
        MapModifier mapModifier = new MapModifier(map);
        for (int x = -map.sizeX / 2; x < map.sizeX / 2; x++)
        {
            for (int y = -map.sizeY / 2; y < map.sizeY / 2; y++)
            {
                if (tileset.GetIndoorTiles().Contains(mapModifier.GetGroundTile(new Vector3Int(x, y, 0))))
                {
                    //spawn
                    if (Random.value < 0.005)
                    {
                        Debug.Log(new Vector2Int(x, y));                        
                        spawnPositions.Add(new Vector2Int(x, y));
                    }
                }
            }
        }
        return spawnPositions;
    }
    /// <summary>
    /// If true enemy will be ranged, otherwise it will be false
    /// </summary>
    /// <returns></returns>
    bool IsEnemyRangedOrMelee()
    {
        return Random.value < 0.5;
    }
    AbstractEnemyFactory GetRandomFactoryOnLocation(Locations location)
    {
        var randomFactor = Random.value;
        switch (location)
        {
            case Locations.Forest:
                return (randomFactor < 0.5) ? new RatFactory() : (AbstractEnemyFactory)new GnollFactory();
            case Locations.Desert:
                return (randomFactor < 0.5) ? new DemonFactory() : (AbstractEnemyFactory)new GnollFactory();
            case Locations.Village:
                return (randomFactor < 0.5) ? new SkeletonFactory() : (AbstractEnemyFactory)new RatFactory();
        }
        return new RatFactory();
    }
    //IEnemyController GetCustomEnemyController()
    //{
    //    throw
    //}
}
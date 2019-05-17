using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner
{
    GameObject playerPrefab;
    public Spawner()
    {
        AssetProxy prefabLoader = new AssetProxy(typeof(GameObject));
        playerPrefab = prefabLoader.LoadAsset("Assets/Objects/Player/Player.prefab");
    }

    public void Spawn(GameMap map, Tileset tileset)
    {
        Locations mapType = StaticTestSettings.getLocation();        

        //now spawning player
        GameObject player = Object.Instantiate(playerPrefab);
        player.transform.position = GetPlayerSpawnPosition(map, tileset);

        //now spawning enemies
        List<Vector2Int> EnemiesSpawnPositions = GetEnemiesSpawnPositions(map, tileset);
        GameObject enemyObject;
        AbstractEnemyFactory enemyFactory;
        foreach (var spawnPosition in EnemiesSpawnPositions)
        {
            enemyFactory = GetRandomFactoryOnLocation(mapType);
            enemyObject = IsEnemyRangedOrMelee() ? enemyFactory.CreateRangedEnemy() : enemyFactory.CreateMeleeEnemy();
            enemyObject.transform.position = new Vector2(spawnPosition.x + 0.5f, spawnPosition.y + 0.5f);
        }

    }
    List<Vector2Int> GetEnemiesSpawnPositions(GameMap map, Tileset tileset)
    {
        List<Vector2Int> spawnPositions = new List<Vector2Int>();
        MapModifier mapModifier = new MapModifier(map);
        GameData gameData = new AssetProxy(typeof(GameData)).LoadAsset("Assets/Objects/Data.asset");
        for (int x = -map.sizeX / 2; x < map.sizeX / 2; x++)
        {
            for (int y = -map.sizeY / 2; y < map.sizeY / 2; y++)
            {
                if (tileset.GetIndoorTiles().Contains(mapModifier.GetGroundTile(new Vector3Int(x, y, 0))) && !tileset.GetStructureTiles().Contains(mapModifier.GetStructureTile(new Vector3Int(x, y, 0))))
                {
                    //spawn
                    if (Random.value < gameData.GetSpawnRate())
                    {                       
                        spawnPositions.Add(new Vector2Int(x, y));
                    }
                }
            }
        }
        Debug.Log("Spawn rate:" + gameData.GetSpawnRate());
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
    Vector2 GetPlayerSpawnPosition(GameMap map, Tileset tileset)
    {
        Rect spawnArea = new Rect(new Vector2(-map.sizeX / 2, -map.sizeY / 2), 
            new Vector2(map.sizeX, map.sizeY));
        spawnArea.xMin += map.sizeX / 10;   //  map.sizeX / 10 is offsetX
        spawnArea.yMin += map.sizeY / 10;   //  map.sizeY / 10 is offsetY
        spawnArea.width -= map.sizeX / 5;
        spawnArea.height -= map.sizeY / 5;
        List<Vector2> spawnCorners = new List<Vector2>()
        {
            new Vector2(spawnArea.min.x, spawnArea.min.y),
            new Vector2(spawnArea.min.x, spawnArea.max.y),
            new Vector2(spawnArea.max.x, spawnArea.min.y),
            new Vector2(spawnArea.max.x, spawnArea.max.y)
        };
        MapModifier mapModifier = new MapModifier(map);
        foreach (var corner in spawnCorners)
        {
            if (!tileset.GetStructureTiles().Contains(mapModifier.
                GetStructureTile(new Vector3Int((int)corner.x, (int)corner.y, 0))))
            {
                Debug.Log(new Vector2((int)corner.x + 0.5f, (int)corner.y + 0.5f));
                return new Vector2((int)corner.x + 0.5f, (int)corner.y + 0.5f);
            }
        }
        return Vector2Int.zero;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class mapGen : MonoBehaviour
{
    NextLevelData nextLevelData;

    EdgeCollider2D edgeCollider;

    void Start()
    {
        nextLevelData = new AssetProxy(typeof(NextLevelData)).LoadAsset("Assets/Objects/NextData.asset");
        nextLevelData.GenerateNextLevelData();
        Vector2Int ms = nextLevelData.GetMapSize();
        int MapSizeX = ms.x;
        int MapSizeY = ms.y;

        System.DateTime start = System.DateTime.Now;

        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.points = new Vector2[] { new Vector2(-MapSizeX / 2, -MapSizeY / 2), new Vector2(-MapSizeX / 2, MapSizeY / 2), new Vector2(MapSizeX / 2, MapSizeY / 2), new Vector2(MapSizeX / 2, -MapSizeY / 2), new Vector2(-MapSizeX / 2, -MapSizeY / 2) };
        GameMap map = new GameMap(MapSizeX, MapSizeY);

        map.ground = GetComponentsInChildren<Tilemap>()[0];
        map.decorations = GetComponentsInChildren<Tilemap>()[1];
        map.structures = GetComponentsInChildren<Tilemap>()[2];

        TilesetFactory tilesetFactory = new TilesetFactory();
        Tileset ts = tilesetFactory.GetTileset(nextLevelData.GetLocation());

        MapBuilder builder;
        MapType type = nextLevelData.GetMapType();
        switch (type)
        {
            case MapType.Dungeon:
                builder = new DungeonMapBuilder(new Rect(-MapSizeX / 2, -MapSizeY / 2, MapSizeX, MapSizeY), map, ts);
                break;
            case MapType.Ruins:
                builder = new RuinsMapBuilder(new Rect(-MapSizeX / 2, -MapSizeY / 2, MapSizeX, MapSizeY), map, ts);
                break;
            case MapType.Village:
                builder = new VillageMapBuilder(new Rect(-MapSizeX / 2, -MapSizeY / 2, MapSizeX, MapSizeY), map);
                break;
            default:
                builder = new DungeonMapBuilder(new Rect(-MapSizeX / 2, -MapSizeY / 2, MapSizeX, MapSizeY), map, ts);
                break;
        }
        //MapBuilder builder = new VillageMapBuilder(new Rect(-MapSizeX / 2, -MapSizeY / 2, MapSizeX, MapSizeY), map);

        MapGenerator mapGen = new MapGenerator(builder);
        mapGen.Generate();
        builder.GetMap();
        Spawner spawner = new Spawner();
        spawner.Spawn(map, ts);
        System.TimeSpan timeItTook = System.DateTime.Now - start;
        Debug.Log(string.Format("Map generated, {0}",timeItTook));
    }
}
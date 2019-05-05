using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mapGen : MonoBehaviour
{
    // Start is called before the first frame update
    int MapSizeX = 60;
    int MapSizeY = 100;
    EdgeCollider2D edgeCollider;
    void Start()
    {
        DateTime start = DateTime.Now;

        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.points = new Vector2[] { new Vector2(-MapSizeX / 2, -MapSizeY / 2), new Vector2(-MapSizeX / 2, MapSizeY / 2), new Vector2(MapSizeX / 2, MapSizeY / 2), new Vector2(MapSizeX / 2, -MapSizeY / 2), new Vector2(-MapSizeX / 2, -MapSizeY / 2) };
        GameMap map = new GameMap(MapSizeX, MapSizeY);

        map.ground = GetComponentsInChildren<Tilemap>()[0];
        map.decorations = GetComponentsInChildren<Tilemap>()[1];
        map.structures = GetComponentsInChildren<Tilemap>()[2];

        TilesetFactory tilesetFactory = new TilesetFactory();
        ITileset ftileset = tilesetFactory.GetTileset(Locations.Forest);
        //ITileset vtileset = tilesetFactory.GetTileset(Locations.Village);
        //ITileset dtileset = tilesetFactory.GetTileset(Locations.Desert);

        //MapBuilder builder = new DungonMapBuilder(new Rect(-MapSizeX / 2, -MapSizeY / 2, MapSizeX, MapSizeY), map, ftileset);
        MapBuilder builder = new DungonMapBuilder(new Rect(-MapSizeX / 2, -MapSizeY / 2, MapSizeX, MapSizeY), map, ftileset);

        MapGenerator mapGen = new MapGenerator(builder);
        mapGen.Generate();

        TimeSpan timeItTook = DateTime.Now - start;
        Debug.Log(String.Format("Map generated, {0}",timeItTook));
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    

    

   
}



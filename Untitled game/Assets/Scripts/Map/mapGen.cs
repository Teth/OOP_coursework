﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mapGen : MonoBehaviour
{
    // Start is called before the first frame update
    public int MapSize;
    EdgeCollider2D edgeCollider;
    void Start()
    {
        DateTime start = DateTime.Now;

        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.points = new Vector2[] { new Vector2(-MapSize / 2, -MapSize / 2), new Vector2(-MapSize / 2, MapSize / 2), new Vector2(MapSize / 2, MapSize / 2), new Vector2(MapSize / 2, -MapSize / 2), new Vector2(-MapSize / 2, -MapSize / 2) };
        GameMap map = new GameMap();

        map.ground = GetComponentsInChildren<Tilemap>()[0];
        map.decorations = GetComponentsInChildren<Tilemap>()[1];
        map.structures = GetComponentsInChildren<Tilemap>()[2];

        TilesetFactory tilesetFactory = new TilesetFactory();
        ITileset ftileset = tilesetFactory.GetTileset(Locations.Forest);

        MapBuilder builder = new DungonMapBuilder(MapSize, map, ftileset);
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



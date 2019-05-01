using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mapGen : MonoBehaviour
{
    // Start is called before the first frame update
    public int MapSize;
    void Start()
    {
        DateTime start = DateTime.Now;
        GameMap map = new GameMap();
        map.ground = GetComponentsInChildren<Tilemap>()[0];
        map.decorations = GetComponentsInChildren<Tilemap>()[1];
        map.structures = GetComponentsInChildren<Tilemap>()[2];
        MapBuilder builder = new ForestMapBuilder(MapSize, map);
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



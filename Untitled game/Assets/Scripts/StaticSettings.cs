using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    Dungeon,
    Ruins,
    Village
}

public static class StaticTestSettings
{
    static Locations location = Locations.Forest;
    
    public static void setLocation(Locations loc)
    {
        location = loc;
    }

    public static Locations getLocation()
    {
        return location;
    }

    static Vector2Int mapSize = new Vector2Int(50, 50);

    public static void setMapSize(Vector2Int mapsize)
    {
        mapSize = mapsize;
    }

    public static Vector2Int getMapSize()
    {
        return mapSize;
    }

    static MapType mapType = MapType.Dungeon;

    public static void SetMapType(MapType maptype)
    {
        mapType = maptype;
    }

    public static MapType GetMapType()
    {
        return mapType;
    }

}
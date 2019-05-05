using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesetFactory
{
    Dictionary<Locations, ITileset> tilesets = new Dictionary<Locations, ITileset>();

    public TilesetFactory()
    {
        tilesets.Add(Locations.Forest, new ForestTileset());
        tilesets.Add(Locations.Desert, new DesertTileset());
        tilesets.Add(Locations.Village, new VillageTileset());
    }

    public ITileset GetTileset(Locations key)
    {
        if (tilesets.ContainsKey(key))
            return tilesets[key];
        else
            return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// FACTORY pattern

public class TilesetFactory
{
    Dictionary<Locations, Tileset> tilesets = new Dictionary<Locations, Tileset>();

    public TilesetFactory()
    {
        tilesets.Add(Locations.Forest, new Tileset(new ForestTileset()));
        tilesets.Add(Locations.Desert, new Tileset(new DesertTileset()));
        tilesets.Add(Locations.Village, new Tileset(new VillageTileset()));
    }

    public Tileset GetTileset(Locations key)
    {
        if (tilesets.ContainsKey(key))
            return tilesets[key];
        else
            return null;
    }
}

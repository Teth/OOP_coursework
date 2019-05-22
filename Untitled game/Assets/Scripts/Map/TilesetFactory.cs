using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// FACTORY pattern


public abstract class TilesetCreator
{
    public abstract ITileset GetITileset();

    public Tileset CreateTileset()
    {
        ITileset ts = GetITileset();
        return new Tileset(ts);
    }
}

public class ForestTilesetCreator : TilesetCreator
{
    public override ITileset GetITileset()
    {
        return new ForestTileset();
    }
}

public class DesertTilesetCreator : TilesetCreator
{
    public override ITileset GetITileset()
    {
        return new DesertTileset();
    }
}


public class VillageTilesetCreator : TilesetCreator
{
    public override ITileset GetITileset()
    {
        return new VillageTileset();
    }
}


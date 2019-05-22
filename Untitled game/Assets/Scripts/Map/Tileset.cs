using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
// Bridge pattern

public enum Locations
{
    Forest,
    Desert,
    Village
}

//Interface
public interface ITileset
{
    TileBase GetGroundTile();
    TileBase GetIndoorTile();
    TileBase GetStructureTile();
    TileBase GetDecorationTile(bool isIndoors);
    List<TileBase> GetGroundTiles();
    List<TileBase> GetIndoorTiles();
    List<TileBase> GetStructureTiles();
    List<TileBase> GetDecorationTiles(bool isIndoors);
}

//Abstraction
public class Tileset
{
    private ITileset _tileset;

    public Tileset(ITileset tileset)
    {
        this._tileset = tileset;
    }

    public void SetTileset(ITileset tileset)
    {
        this._tileset = tileset;
    }

    public TileBase GetGroundTile()
    {
        return _tileset.GetGroundTile();
    }

    public TileBase GetIndoorTile()
    {
        return _tileset.GetIndoorTile();
    }

    public TileBase GetStructureTile()
    {
        return _tileset.GetStructureTile();
    }

    public TileBase GetDecorationTile(bool isIndoors)
    {
        return _tileset.GetDecorationTile(isIndoors);
    }

    public List<TileBase> GetIndoorTiles()
    {
        return _tileset.GetIndoorTiles();
    }

    public List<TileBase> GetGroundTiles()
    {
        return _tileset.GetGroundTiles();
    }

    public List<TileBase> GetStructureTiles()
    {
        return _tileset.GetStructureTiles();
    }

}

//Implimentation
public class ForestTileset : ITileset
{
    protected List<TileBase> ground;
    protected List<TileBase> indoorGround;
    protected List<TileBase> structures;
    protected List<TileBase> decorationsOutside;
    protected List<TileBase> decorationsInside;

    public ForestTileset()
    {
        ground = new List<TileBase>();
        indoorGround = new List<TileBase>();
        structures = new List<TileBase>();
        decorationsInside = new List<TileBase>();
        decorationsOutside = new List<TileBase>();
        AssetProxy tileProxy = new AssetProxy(typeof(TileBase));
        decorationsInside.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/decoration_dungeon.asset"));
        decorationsInside.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/decoration_dungeon1.asset"));
        decorationsInside.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/decoration_dungeon2.asset"));
        decorationsInside.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/decoration_dungeon3.asset"));
        decorationsOutside.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/decoration_outdoor.asset"));
        ground.Add(tileProxy.LoadAsset("Objects/Tiles/Shared/ground.asset"));
        ground.Add(tileProxy.LoadAsset("Objects/Tiles/Shared/ground1.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_floor.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_deco1.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_deco2.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_deco3.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_deco4.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_deco5.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_deco6.asset"));
        structures.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone.asset"));
    }

    public TileBase GetDecorationTile(bool isIndoors)
    {
        if(isIndoors)
        {
            return Tiles.GetRandomTile(decorationsInside);
        }else
        {
            return Tiles.GetRandomTile(decorationsOutside);
        }
    }

    public List<TileBase> GetDecorationTiles(bool isIndoors)
    {
        if (isIndoors)
        {
            return decorationsInside;
        }
        else
        {
            return decorationsOutside;
        }
    }

    public TileBase GetGroundTile()
    {
        return Tiles.GetRandomTile(ground);
    }

    public List<TileBase> GetGroundTiles()
    {
        return ground;
    }

    public TileBase GetIndoorTile()
    {
        return Tiles.GetRandomTile(indoorGround, 0.7f);
    }

    public List<TileBase> GetIndoorTiles()
    {
        return indoorGround;
    }

    public TileBase GetStructureTile()
    {
        return Tiles.GetRandomTile(structures);
    }

    public List<TileBase> GetStructureTiles()
    {
        return structures;
    }
}

//Implimentation
public class DesertTileset : ITileset
{
    protected List<TileBase> ground;
    protected List<TileBase> indoorGround;
    protected List<TileBase> structures;
    protected List<TileBase> decorationsOutside;
    protected List<TileBase> decorationsInside;

    public DesertTileset()
    {
        ground = new List<TileBase>();
        indoorGround = new List<TileBase>();
        structures = new List<TileBase>();
        decorationsOutside = new List<TileBase>();
        decorationsInside = new List<TileBase>();
        AssetProxy tileProxy = new AssetProxy(typeof(TileBase));
        ground.Add(tileProxy.LoadAsset("Objects/Tiles/Desert/desert.asset"));
        ground.Add(tileProxy.LoadAsset("Objects/Tiles/Desert/desert2.asset"));
        ground.Add(tileProxy.LoadAsset("Objects/Tiles/Desert/desert3.asset"));
        decorationsInside.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/decoration_dungeon3.asset"));
        decorationsOutside.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/decoration_outdoor.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_floor.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_deco1.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_deco2.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_deco3.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_deco4.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_deco5.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone_deco6.asset"));
        structures.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/stone.asset"));

    }

    public TileBase GetDecorationTile(bool isIndoors)
    {
        if (isIndoors)
        {
            return Tiles.GetRandomTile(decorationsInside);
        }
        else
        {
            return Tiles.GetRandomTile(decorationsOutside);
        }
    }

    public List<TileBase> GetDecorationTiles(bool isIndoors)
    {
        if (isIndoors)
        {
            return decorationsInside;
        }
        else
        {
            return decorationsOutside;
        }
    }
    public TileBase GetGroundTile()
    {
        return Tiles.GetRandomTile(ground);
    }

    public List<TileBase> GetGroundTiles()
    {
        return ground;
    }

    public TileBase GetIndoorTile()
    {
        return Tiles.GetRandomTile(indoorGround, 0.7f);
    }

    public List<TileBase> GetIndoorTiles()
    {
        return indoorGround;
    }

    public TileBase GetStructureTile()
    {
        return Tiles.GetRandomTile(structures);
    }

    public List<TileBase> GetStructureTiles()
    {
        return structures;
    }
}

//Implimentation
public class VillageTileset : ITileset
{
    protected List<TileBase> ground;
    protected List<TileBase> indoorGround;
    protected List<TileBase> structures;
    protected List<TileBase> decorationsOutside;
    protected List<TileBase> decorationsInside;


    public VillageTileset()
    {
        ground = new List<TileBase>();
        indoorGround = new List<TileBase>();
        structures = new List<TileBase>();
        decorationsOutside = new List<TileBase>();
        decorationsInside = new List<TileBase>();
        AssetProxy tileProxy = new AssetProxy(typeof(TileBase));
        ground.Add(tileProxy.LoadAsset("Objects/Tiles/Shared/ground.asset"));
        ground.Add(tileProxy.LoadAsset("Objects/Tiles/Shared/ground1.asset"));
        decorationsInside.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/decoration_dungeon3.asset"));
        decorationsOutside.Add(tileProxy.LoadAsset("Objects/Tiles/Forest/decoration_outdoor.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Objects/Tiles/Village/village_floor.asset"));
        structures.Add(tileProxy.LoadAsset("Objects/Tiles/Village/village_wall.asset"));
    }


    public TileBase GetDecorationTile(bool isIndoors)
    {
        if (isIndoors)
        {
            return Tiles.GetRandomTile(decorationsInside);
        }
        else
        {
            return Tiles.GetRandomTile(decorationsOutside);
        }
    }

    public List<TileBase> GetDecorationTiles(bool isIndoors)
    {
        if (isIndoors)
        {
            return decorationsInside;
        }
        else
        {
            return decorationsOutside;
        }
    }

    public TileBase GetGroundTile()
    {
        return Tiles.GetRandomTile(ground);
    }

    public List<TileBase> GetGroundTiles()
    {
        return ground;
    }

    public TileBase GetIndoorTile()
    {
        return Tiles.GetRandomTile(indoorGround);
    }

    public List<TileBase> GetIndoorTiles()
    {
        return indoorGround;
    }

    public TileBase GetStructureTile()
    {
        return Tiles.GetRandomTile(structures);
    }

    public List<TileBase> GetStructureTiles()
    {
        return structures;
    }
}
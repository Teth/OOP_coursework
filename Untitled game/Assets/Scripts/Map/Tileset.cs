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
    List<TileBase> GetGroundTiles();
    List<TileBase> GetIndoorTiles();
    List<TileBase> GetStructureTiles();
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

    public ForestTileset()
    {
        ground = new List<TileBase>();
        indoorGround = new List<TileBase>();
        structures = new List<TileBase>();
        AssetProxy tileProxy = new AssetProxy(typeof(TileBase));
        ground.Add(tileProxy.LoadAsset("Assets/Tiles/Shared/ground.asset"));
        ground.Add(tileProxy.LoadAsset("Assets/Tiles/Shared/ground1.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_floor.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_deco1.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_deco2.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_deco3.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_deco4.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_deco5.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_deco6.asset"));
        structures.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone.asset"));
    }

    public TileBase GetGroundTile()
    {
        return MapHelper.GetRandomTile(ground);
    }

    public List<TileBase> GetGroundTiles()
    {
        return ground;
    }

    public TileBase GetIndoorTile()
    {
        return MapHelper.GetRandomTile(indoorGround, 0.7f);
    }

    public List<TileBase> GetIndoorTiles()
    {
        return indoorGround;
    }

    public TileBase GetStructureTile()
    {
        return MapHelper.GetRandomTile(structures);
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

    public DesertTileset()
    {
        ground = new List<TileBase>();
        indoorGround = new List<TileBase>();
        structures = new List<TileBase>();
        AssetProxy tileProxy = new AssetProxy(typeof(TileBase));
        ground.Add(tileProxy.LoadAsset("Assets/Tiles/Desert/desert.asset"));
        ground.Add(tileProxy.LoadAsset("Assets/Tiles/Desert/desert2.asset"));
        ground.Add(tileProxy.LoadAsset("Assets/Tiles/Desert/desert3.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_floor.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_deco1.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_deco2.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_deco3.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_deco4.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_deco5.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone_deco6.asset"));
        structures.Add(tileProxy.LoadAsset("Assets/Tiles/Forest/stone.asset"));

    }

    public TileBase GetGroundTile()
    {
        return MapHelper.GetRandomTile(ground);
    }

    public List<TileBase> GetGroundTiles()
    {
        return ground;
    }

    public TileBase GetIndoorTile()
    {
        return MapHelper.GetRandomTile(indoorGround, 0.7f);
    }

    public List<TileBase> GetIndoorTiles()
    {
        return indoorGround;
    }

    public TileBase GetStructureTile()
    {
        return MapHelper.GetRandomTile(structures);
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

    public VillageTileset()
    {
        ground = new List<TileBase>();
        indoorGround = new List<TileBase>();
        structures = new List<TileBase>();
        AssetProxy tileProxy = new AssetProxy(typeof(TileBase));
        ground.Add(tileProxy.LoadAsset("Assets/Tiles/Shared/ground.asset"));
        ground.Add(tileProxy.LoadAsset("Assets/Tiles/Shared/ground1.asset"));
        indoorGround.Add(tileProxy.LoadAsset("Assets/Tiles/Village/village_floor.asset"));
        structures.Add(tileProxy.LoadAsset("Assets/Tiles/Village/village_wall.asset"));
    }

    public TileBase GetGroundTile()
    {
        return MapHelper.GetRandomTile(ground);
    }

    public List<TileBase> GetGroundTiles()
    {
        throw new System.NotImplementedException();
    }

    public TileBase GetIndoorTile()
    {
        return MapHelper.GetRandomTile(indoorGround);
    }

    public List<TileBase> GetIndoorTiles()
    {
        throw new System.NotImplementedException();
    }

    public TileBase GetStructureTile()
    {
        return MapHelper.GetRandomTile(structures);
    }

    public List<TileBase> GetStructureTiles()
    {
        throw new System.NotImplementedException();
    }
}

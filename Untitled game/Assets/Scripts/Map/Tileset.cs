using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Locations
{
    Forest,
    Desert
}

public interface ITileset
{
    TileBase GetGroundTile();
    TileBase GetIndoorTile();
    TileBase GetStructureTile();
    List<TileBase> GetGroundTiles();
    List<TileBase> GetIndoorTiles();
    List<TileBase> GetStructureTiles();
}

public class Tileset
{
    protected List<TileBase> ground;
    protected List<TileBase> indoorGround;
    protected List<TileBase> structures;

    public TileBase GetRandomTile(List<TileBase> tiles, float defectChance = 0)
    {
        if(Random.value > defectChance)
        {
            int c = Mathf.FloorToInt(Random.value * tiles.Count);
            return tiles[c];
        }
        return tiles[0];
    }

    public Tileset()
    {
        ground = new List<TileBase>();
        indoorGround = new List<TileBase>();
        structures = new List<TileBase>();
    }
}

public class ForestTileset : Tileset, ITileset
{
    public ForestTileset()
    {
        ground.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/ground.asset", typeof(TileBase)));
        ground.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/ground1.asset", typeof(TileBase)));
        indoorGround.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone_floor.asset", typeof(TileBase)));
        indoorGround.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone_deco1.asset", typeof(TileBase)));
        indoorGround.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone_deco2.asset", typeof(TileBase)));
        indoorGround.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone_deco3.asset", typeof(TileBase)));
        indoorGround.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone_deco4.asset", typeof(TileBase)));
        indoorGround.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone_deco5.asset", typeof(TileBase)));
        indoorGround.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone_deco6.asset", typeof(TileBase)));
        structures.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone.asset", typeof(TileBase)));
    }

    public TileBase GetGroundTile()
    {
        return GetRandomTile(ground);
    }

    public List<TileBase> GetGroundTiles()
    {
        return ground;
    }

    public TileBase GetIndoorTile()
    {
        return GetRandomTile(indoorGround, 0.7f);
    }

    public List<TileBase> GetIndoorTiles()
    {
        return indoorGround;
    }

    public TileBase GetStructureTile()
    {
        return GetRandomTile(structures);
    }

    public List<TileBase> GetStructureTiles()
    {
        return structures;
    }
}

public class DesertTileset : Tileset, ITileset
{
    public DesertTileset()
    {
        ground.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/ground.asset", typeof(TileBase)));
        ground.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/ground1.asset", typeof(TileBase)));
        indoorGround.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone_deco1.asset", typeof(TileBase)));
        indoorGround.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone_deco2.asset", typeof(TileBase)));
        indoorGround.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone_deco3.asset", typeof(TileBase)));
        indoorGround.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone_floor.asset", typeof(TileBase)));
        structures.Add((TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone.asset", typeof(TileBase)));
    }

    public TileBase GetGroundTile()
    {
        return GetRandomTile(ground);
    }

    public List<TileBase> GetGroundTiles()
    {
        throw new System.NotImplementedException();
    }

    public TileBase GetIndoorTile()
    {
        return GetRandomTile(ground);
    }

    public List<TileBase> GetIndoorTiles()
    {
        throw new System.NotImplementedException();
    }

    public TileBase GetStructureTile()
    {
        return GetRandomTile(ground);
    }

    public List<TileBase> GetStructureTiles()
    {
        throw new System.NotImplementedException();
    }
}

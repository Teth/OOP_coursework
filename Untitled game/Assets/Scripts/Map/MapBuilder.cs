using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;

public class MapGenerator
{
    private MapBuilder b;
    public MapGenerator(MapBuilder builder)
    {
        this.b = builder;
    }

    public void Generate()
    {
        b.BuildGround();
        b.BuildStructures();
        //b.BuildDecorations();
    }

}

public class DungonMapBuilder : MapBuilder
{
    public DungonMapBuilder(int size, GameMap map, ITileset tileset) : base(size, map, tileset)
    {
        
    }

    public override void BuildDecorations()
    {
        throw new NotImplementedException();
    }

    public override void BuildGround()
    {
        Rect mapArea = new Rect(-size / 2, -size / 2, size, size);
        Func<ITileset, TileBase> getGround = tileset => tileset.GetGroundTile();
        MapHelper.FillRect(map.ground, mapArea, getGround, tileset);
    }

    public override void BuildStructures()
    {
        StructureGenerator gen = new StructureGenerator(map, tileset);
        gen.CreateDungeon(new Rect(-size / 4, -size / 4, size/2, size/2));
    }
}

public abstract class MapBuilder
{
    protected TileBase[] tiles;
    protected ITileset tileset;
    protected GameMap map;
    protected int size;

    protected MapBuilder(int size, GameMap map, ITileset tileset)
    {
        this.size = size;
        this.map = map;
        this.tileset = tileset;
    }
    public void setTiles(TileBase[] tiles)
    {
        this.tiles = tiles;
    }
    
    public abstract void BuildGround();
    public abstract void BuildDecorations();
    public abstract void BuildStructures();
}


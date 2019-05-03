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
    }

}

public class ForestMapBuilder : MapBuilder
{
    public ForestMapBuilder(int size, GameMap map) : base(size, map)
    {
        setTiles(MapHelper.GetForestTileset());
    }
    public override void BuildDecorations(float fossilRate)
    {
        throw new System.NotImplementedException();
    }

    public override void BuildGround()
    {
        Rect mapArea = new Rect(-size / 2, -size / 2, size, size);
        MapHelper.FillRect(map.ground, mapArea, tiles[0]);
    }

    public override void BuildStructures()
    {
        StructureGenerator gen = new StructureGenerator(map, tiles);
        gen.CreateDungeon(new Rect(-size / 4, -size / 4, size/2, size/2));
    }
}

public abstract class MapBuilder
{
    protected TileBase[] tiles;
    protected GameMap map;
    protected int size;

    protected MapBuilder(int size, GameMap map)
    {
        this.size = size;
        this.map = map;
        
    }
    public void setTiles(TileBase[] tiles)
    {
        this.tiles = tiles;
    }
    
    public abstract void BuildGround();
    public abstract void BuildDecorations(float fossilRate);
    public abstract void BuildStructures();
}


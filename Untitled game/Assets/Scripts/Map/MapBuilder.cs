using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
// BUILDER pattern

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
        b.BuildDecorations();
        b.SetExit();
    }

}

public class DungeonMapBuilder : MapBuilder
{
    int MAX_DUNGEONS = 2;
    MapFacade generator;

    public DungeonMapBuilder(Rect area, GameMap map, Tileset tileset) : base(area, map, tileset)
    {
        generator = new MapFacade(map, tileset);
    }

    public override void BuildDecorations()
    {
        generator.GenerateDecorations(area, 0.01f);
    }

    public override void BuildGround()
    {
        generator.CreateGround(area);
    }

    public override void BuildStructures()
    {
        generator.CreateDungeonInArea(area);
    }

    public override void SetExit()
    {
        throw new System.NotImplementedException();
    }
}

public class VillageMapBuilder : MapBuilder
{
    MapFacade generator;
    public VillageMapBuilder(Rect area, GameMap map) : base(area, map, null)
    {
        tileset = new TilesetFactory().GetTileset(Locations.Village);
        generator = new MapFacade(map, tileset);
    }

    public override void BuildDecorations()
    {
        generator.GenerateDecorations(area);
    }

    public override void BuildGround()
    {
        generator.CreateGround(area);
    }

    public override void BuildStructures()
    {
        generator.CreateVillageInArea(area);
    }

    public override void SetExit()
    {
        throw new System.NotImplementedException();
    }
}

public class RuinsMapBuilder : MapBuilder
{
    int MAX_DUNGEONS = 2;
    MapFacade generator;

    public RuinsMapBuilder(Rect area, GameMap map, Tileset tileset) : base(area, map, tileset)
    {
        generator = new MapFacade(map, tileset);
    }

    public override void BuildDecorations()
    {
        generator.GenerateDecorations(area, 0.1f);
    }

    public override void BuildGround()
    {
        generator.CreateGround(area);
    }

    public override void BuildStructures()
    {
        generator.CreateRuinsInArea(area);
    }

    public override void SetExit()
    {
        throw new System.NotImplementedException();
    }
}


public abstract class MapBuilder
{
    protected Tileset tileset;
    protected GameMap map;
    protected Rect area;

    protected MapBuilder(Rect area, GameMap map, Tileset tileset)
    {
        this.area = area;
        this.map = map;
        this.tileset = tileset;
    }

    public abstract void BuildGround();
    public abstract void BuildDecorations();
    public abstract void BuildStructures();
    public abstract void SetExit();
    public GameMap GetMap()
    {
        return map;
    }
}


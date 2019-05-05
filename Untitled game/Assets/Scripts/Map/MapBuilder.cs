using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

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
    int MAX_DUNGEONS = 2;

    public DungonMapBuilder(Rect area, GameMap map, ITileset tileset) : base(area, map, tileset)
    {
       
    }

    public override void BuildDecorations()
    {
        throw new System.NotImplementedException();
    }

    public override void BuildGround()
    {
        System.Func<ITileset, TileBase> getGround = tileset => tileset.GetGroundTile();
        MapHelper.FillRect(map.ground, area, getGround, tileset);
        MapHelper.GenerateFadeout(map.ground, area, getGround, tileset, Mathf.CeilToInt((area.height + area.width)/16));
    }

    public override void BuildStructures()
    {
        StructureGenerator gen = new StructureGenerator(map, tileset);
        int numberOfDungeons = (int)(Random.value * (MAX_DUNGEONS - 1)) + 1;
        for (int i = 0; i < numberOfDungeons; i++)
        {
            gen.CreateDungeon(MapHelper.createRandomRectangleInArea(area, new Vector2Int((int)area.width/3, (int)area.height/3), new Vector2Int((int)area.width/2, (int)area.height/2)));
        }
    }
}

public class VillageMapBuilder : MapBuilder
{
    int MAX_DUNGEONS = 2;

    public VillageMapBuilder(Rect area, GameMap map, ITileset tileset) : base(area, map, tileset)
    {

    }

    public override void BuildDecorations()
    {
        throw new System.NotImplementedException();
    }

    public override void BuildGround()
    {
        System.Func<ITileset, TileBase> getGround = tileset => tileset.GetGroundTile();
        MapHelper.FillRect(map.ground, area, getGround, tileset);
        MapHelper.GenerateFadeout(map.ground, area, getGround, tileset, (int)((area.width + area.height) / 16));
    }

    public override void BuildStructures()
    {
        StructureGenerator gen = new StructureGenerator(map, tileset);
        gen.CreateVillage(MapHelper.createRandomRectangleInArea(area, new Vector2Int((int)area.width / 2, (int)area.height / 2), new Vector2Int((int)(area.width / 1.2), (int)(area.height / 1.2))));
    }
}

public abstract class MapBuilder
{
    protected ITileset tileset;
    protected GameMap map;
    protected Rect area;

    protected MapBuilder(Rect area, GameMap map, ITileset tileset)
    {
        this.area = area;
        this.map = map;
        this.tileset = tileset;
    }

    public abstract void BuildGround();
    public abstract void BuildDecorations();
    public abstract void BuildStructures();
}


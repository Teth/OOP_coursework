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
    }

}

public class ForestMapBuilder : MapBuilder
{
    const int MAX_STRUCTURE_SIZE = 10;
    int[,] occupiedTiles;

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
        occupiedTiles = new int[size/2, size / 2];
        Rect mapArea = new Rect(-size / 2, -size / 2, size, size);
        MapHelper.FillRect(map.ground, mapArea, tiles[0]);
    }

    public override void BuildStructures()
    {
        StructureGenerator gen = new StructureGenerator(map, tiles);
        gen.CreateDungeon(new Rect(-size / 2, -size / 2, size/2, size/2));
        //int numBerOfStructures = Mathf.CeilToInt(Mathf.Sqrt(size / 6.5f));
        //for(int i = 0; i <= Mathf.CeilToInt(Random.value * numBerOfStructures); i++)
        //{
        //    int w = (int)(Random.value * (MAX_STRUCTURE_SIZE -3) ) + 3;
        //    int h = (int)(Random.value * (MAX_STRUCTURE_SIZE -3) ) + 3;
        //    int posX = (int)(Random.value * (size-w));
        //    int posY = (int)(Random.value * (size-h));
        //    Vector3Int stVec = new Vector3Int(-size / 2 + posX, -size / 2 + posY, 0);
        //    Vector3Int endVec = new Vector3Int(-size / 2 + posX + w, -size / 2 + posY + h, 0);
        //    MapHelper.MakeBox(map.structures, stVec, endVec, tiles[2]);
        //}

        // Makes a border for map
        //MapHelper.MakeBox(map.structures, new Vector3Int(-size / 2 - 1, -size / 2 - 1, 0), new Vector3Int(size / 2 + 1, size / 2 + 1, 0), tiles[0]);
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

public static class MapHelper
{
    public static TileBase[] GetForestTileset()
    {
        TileBase ground = (TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/ground.asset", typeof(TileBase));
        TileBase fossil = (TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/fossil.asset", typeof(TileBase));
        TileBase stone = (TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone.asset", typeof(TileBase));
        TileBase stone_floor = (TileBase)AssetDatabase.LoadAssetAtPath("Assets/Tiles/stone_floor.asset", typeof(TileBase));
        if (ground == null)
        {
            Debug.Log("cant find files");
        }
        return new TileBase[] { ground, fossil, stone, stone_floor };
    }

    public static Tilemap MakeBox(Tilemap target, Rect rect, TileBase tile, float fillPerc = 1)
    {
        int width = (int)(rect.width);
        int height = (int)(rect.height);
        for (int counter_width = 0; counter_width < width; counter_width++)
        {
            for (int counter_height = 0; counter_height < height; counter_height++)
            {
                if (UnityEngine.Random.value < fillPerc && ((counter_width == 0 || counter_width == width - 1) || (counter_height == 0 || counter_height == height - 1)))
                {
                    target.SetTile(new Vector3Int((int)(rect.xMin) + counter_width, (int)(rect.yMin) + counter_height, 0), tile);
                }
            }
        }
        return target;
    }

    public static Tilemap MakeRoom(Tilemap target, Rect rect, TileBase tile, float fillPerc = 1)
    {
        int width = (int)(rect.width);
        int height = (int)(rect.height);
        int doorPosX = (int)((width - 2) * Random.value) + 1;
        int doorPosY = (int)((height - 2) * Random.value) + 1;
        if(Mathf.RoundToInt(Random.value) == 1)
        {
            // door on vertical
            if(width - doorPosX <= doorPosX)
            {
                doorPosX = width - 1;
            }
            else
            {
                doorPosX = 0;
            }
        }
        else
        {
            if (height - doorPosY <= doorPosY)
            {
                doorPosY = height - 1;
            }
            else
            {
                doorPosY = 0;
            }
        }

        for (int counter_width = 0; counter_width < width; counter_width++)
        {
            for (int counter_height = 0; counter_height < height; counter_height++)
            {
                if (UnityEngine.Random.value < fillPerc && ((counter_width == 0 || counter_width == width - 1) || (counter_height == 0 || counter_height == height - 1)))
                {
                    if(!(counter_width == doorPosX && counter_height == doorPosY))
                        target.SetTile(new Vector3Int((int)(rect.xMin) + counter_width, (int)(rect.yMin) + counter_height, 0), tile);
                }
            }
        }
        return target;
    }

    public static void FillRect(Tilemap target, Rect rect, TileBase tile, float fillPerc = 1)
    {
        int width = (int)(rect.width);
        int height = (int)(rect.height);
        for (int counter_width = 0; counter_width < width; counter_width++)
        {
            for (int counter_height = 0; counter_height < height; counter_height++)
            {

                if (UnityEngine.Random.value < fillPerc)
                {
                    
                    target.SetTile(new Vector3Int((int)(rect.xMin) + counter_width, (int)(rect.yMin) + counter_height, 0), tile);
                }
            }
        }
    }

    // fills with alt if perc not satisfied
    public static Tilemap FillSquareAlt(Tilemap target, int side, Vector3Int stPos, float fillPerc, TileBase tile, TileBase altTile)
    {
        for (int counter_width = 0; counter_width < side; counter_width++)
        {
            for (int counter_height = 0; counter_height < side; counter_height++)
            {
                if (UnityEngine.Random.value < fillPerc)
                    target.SetTile(new Vector3Int(stPos.x + counter_width, stPos.y + counter_height, 0), tile);
                else
                    target.SetTile(new Vector3Int(stPos.x + counter_width, stPos.y + counter_height, 0), altTile);
            }
        }
        return target;
    }
}

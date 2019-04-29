﻿using System.Collections;
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
        Vector3Int stPos = new Vector3Int(-size / 2, -size / 2, 0);
        Vector3Int endPos = new Vector3Int(size / 2, size / 2, 0);
        MapHelper.FillRect(map.ground, stPos, endPos, tiles[0]);
    }

    public override void BuildStructures()
    {
        
        int numBerOfStructures = Mathf.CeilToInt(Mathf.Sqrt(size / 6.5f));
        for(int i = 0; i <= Mathf.CeilToInt(Random.value * numBerOfStructures); i++)
        {
            int w = (int)(Random.value * (MAX_STRUCTURE_SIZE -3) ) + 3;
            int h = (int)(Random.value * (MAX_STRUCTURE_SIZE -3) ) + 3;
            int posX = (int)(Random.value * (size-w));
            int posY = (int)(Random.value * (size-h));
            Vector3Int stVec = new Vector3Int(-size / 2 + posX, -size / 2 + posY, 0);
            Vector3Int endVec = new Vector3Int(-size / 2 + posX + w, -size / 2 + posY + h, 0);
            MapHelper.MakeBox(map.structures, stVec, endVec, tiles[2]);
        }
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
        if (ground == null)
        {
            Debug.Log("cant find files");
        }
        return new TileBase[] { ground, fossil, stone };
    }

    public static Tilemap MakeBox(Tilemap target, Vector3Int stPos, Vector3Int endPos, TileBase tile, float fillPerc = 1)
    {
        int width = endPos.x - stPos.x;
        int height = endPos.y - stPos.y;
        bool hasDoor = false;
        for (int cw = 0; cw < width; cw++)
        {
            for (int ch = 0; ch < height; ch++)
            {
                if (UnityEngine.Random.value < fillPerc && ((cw == 0 || cw == width - 1) || (ch == 0 || ch == height - 1)))
                {
                    if (!hasDoor && UnityEngine.Random.value < 0.1f && (cw != 0 || cw != width) && (ch != 0 || ch != height))
                    {
                        // leave empty
                        hasDoor = true;
                    }
                    else
                        target.SetTile(new Vector3Int(stPos.x + cw, stPos.y + ch, 0), tile);
                }
            }
        }
        return target;
    }
    public static void FillRect(Tilemap target, Vector3Int stPos, Vector3Int endPos, TileBase tile, float fillPerc = 1)
    {
        if(stPos.x >= endPos.x || stPos.y >= endPos.y)
        {
            throw new System.Exception("Start vector coord less than end vector");
        }
        for (int cw = 0; cw < endPos.x - stPos.x; cw++)
        {
            for (int ch = 0; ch < endPos.y - stPos.y; ch++)
            {

                if (UnityEngine.Random.value < fillPerc)
                {
                    
                    target.SetTile(new Vector3Int(stPos.x + cw, stPos.y + ch, 0), tile);
                }
            }
        }
    }

    // fills with alt if perc not satisfied
    public static Tilemap FillSquareAlt(Tilemap target, int side, Vector3Int stPos, float fillPerc, TileBase tile, TileBase altTile)
    {
        for (int cw = 0; cw < side; cw++)
        {
            for (int ch = 0; ch < side; ch++)
            {
                if (UnityEngine.Random.value < fillPerc)
                    target.SetTile(new Vector3Int(stPos.x + cw, stPos.y + ch, 0), tile);
                else
                    target.SetTile(new Vector3Int(stPos.x + cw, stPos.y + ch, 0), altTile);
            }
        }
        return target;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class MapHelper
{
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

    public static Tilemap MakeBox(Tilemap target, Rect rect, System.Func<ITileset, TileBase> func, ITileset tileset, float fillPerc = 1)
    {
        int width = (int)(rect.width);
        int height = (int)(rect.height);
        Debug.Log(fillPerc);
        for (int counter_width = 0; counter_width < width; counter_width++)
        {
            for (int counter_height = 0; counter_height < height; counter_height++)
            {
                if (((counter_width == 0 || counter_width == width - 1) || (counter_height == 0 || counter_height == height - 1)))
                {
                    if(Random.value < fillPerc)
                    {
                        target.SetTile(new Vector3Int((int)(rect.xMin) + counter_width, (int)(rect.yMin) + counter_height, 0), func.Invoke(tileset));
                        
                    }
                }
            }
        }
        return target;
    }

    public static Tilemap MakeRoom(Tilemap target, Rect rect, System.Func<ITileset, TileBase> func, ITileset tileset, float fillPerc = 1)
    {
        int width = (int)(rect.width);
        int height = (int)(rect.height);
        int doorPosX = (int)((width - 2) * Random.value) + 1;
        int doorPosY = (int)((height - 2) * Random.value) + 1;
        if (Mathf.RoundToInt(Random.value) == 1)
        {
            // door on vertical
            if (width - doorPosX <= doorPosX)
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
                    if (!(counter_width == doorPosX && counter_height == doorPosY))
                        target.SetTile(new Vector3Int((int)(rect.xMin) + counter_width, (int)(rect.yMin) + counter_height, 0), func.Invoke(tileset));
                }
            }
        }
        return target;
    }

    public static void GenerateFadeout(Tilemap ground, Rect area, System.Func<ITileset, TileBase> getGround, ITileset tileset, int fadeoutCycles)
    {
        Rect fadeoutRect = new Rect(area);
        float randStep = 1 / (float)fadeoutCycles;
        for (int i = 0; i < fadeoutCycles; i++)
        {
            fadeoutRect.x--;
            fadeoutRect.y--;
            fadeoutRect.width += 2;
            fadeoutRect.height += 2;
            Debug.Log(fadeoutRect.ToString());
            float thisCycleChance = 1 - randStep * (i + 1);
            MakeBox(ground, fadeoutRect, getGround, tileset, thisCycleChance);
        }
    }

    public static void FillRect(Tilemap target, Rect rect, System.Func<ITileset, TileBase> func, ITileset tileset, float fillPerc = 1)
    {
        int width = (int)(rect.width);
        int height = (int)(rect.height);
        for (int counter_width = 0; counter_width < width; counter_width++)
        {
            for (int counter_height = 0; counter_height < height; counter_height++)
            {

                if (UnityEngine.Random.value < fillPerc)
                {

                    target.SetTile(new Vector3Int((int)(rect.xMin) + counter_width, (int)(rect.yMin) + counter_height, 0), func.Invoke(tileset));
                }
            }
        }
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

    public static Rect createRandomRectangleInArea(Rect Area, Vector2Int mins, Vector2Int maxs)
    {
        int width = (int)((maxs.x - mins.x) * Random.value + mins.x);
        int height = (int)((maxs.y - mins.y) * Random.value + mins.y);
        int ypos = (int)((Area.height - height) * Random.value);
        int xpos = (int)((Area.width - width) * Random.value);

        return new Rect((int)(Area.xMin + xpos), (int)(Area.yMin + ypos), width, height);
    }
}
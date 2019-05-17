using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Directions
{
    Up,
    Down,
    Left,
    Right
}

// tiles operations
public static class Tiles
{    
    public static TileBase GetRandomTile(List<TileBase> tiles, float defectChance = 0)
    {
        if (Random.value > defectChance)
        {
            int c = Mathf.FloorToInt(Random.value * tiles.Count);
            return tiles[c];
        }
        return tiles[0];
    }
}

public class TilemapModifier
{
    // Primitive map modifying
    private Tilemap tilemap;

    public TilemapModifier(Tilemap tilemap)
    {
        this.tilemap = tilemap;
    }

    public void SetTile(Vector3Int coords, TileBase tile)
    {
        tilemap.SetTile(coords, tile);
    }

    public List<Vector3Int> MakeBox(Rect rect, System.Func<Tileset, TileBase> func, Tileset tileset, float fillPerc = 1)
    {
        int width = (int)(rect.width);
        int height = (int)(rect.height);
        List<Vector3Int> listOfWallTiles = new List<Vector3Int>();
        for (int counter_width = 0; counter_width < width; counter_width++)
        {
            for (int counter_height = 0; counter_height < height; counter_height++)
            {
                if (((counter_width == 0 || counter_width == width - 1) || (counter_height == 0 || counter_height == height - 1)))
                {
                    if (Random.value < fillPerc)
                    {
                        Vector3Int coords = new Vector3Int((int)(rect.xMin) + counter_width, (int)(rect.yMin) + counter_height, 0);
                        tilemap.SetTile(coords, func.Invoke(tileset));
                        listOfWallTiles.Add(coords);
                    }
                }
            }
        }
        return listOfWallTiles;
    }

    public List<Vector3Int> MakeBox(Rect rect, TileBase tile, float fillPerc = 1)
    {
        int width = (int)(rect.width);
        int height = (int)(rect.height);
        List<Vector3Int> listOfWallTiles = new List<Vector3Int>();
        for (int counter_width = 0; counter_width < width; counter_width++)
        {
            for (int counter_height = 0; counter_height < height; counter_height++)
            {
                if (((counter_width == 0 || counter_width == width - 1) || (counter_height == 0 || counter_height == height - 1)))
                {
                    if (Random.value < fillPerc)
                    {
                        Vector3Int coords = new Vector3Int((int)(rect.xMin) + counter_width, (int)(rect.yMin) + counter_height, 0);
                        tilemap.SetTile(coords, tile);
                        listOfWallTiles.Add(coords);
                    }
                }
            }
        }
        return listOfWallTiles;
    }

    public void MakeRoom(Rect rect, System.Func<Tileset, TileBase> func, Tileset tileset, float fillPerc = 1)
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
                        tilemap.SetTile(new Vector3Int((int)(rect.xMin) + counter_width, (int)(rect.yMin) + counter_height, 0), func.Invoke(tileset));
                }
            }
        }
    }

    public void GenerateFadeout(Rect area, System.Func<Tileset, TileBase> getGround, Tileset tileset, int fadeoutCycles)
    {
        Rect fadeoutRect = new Rect(area);
        float randStep = 1 / (float)fadeoutCycles;
        for (int i = 0; i < fadeoutCycles; i++)
        {
            fadeoutRect.x--;
            fadeoutRect.y--;
            fadeoutRect.width += 2;
            fadeoutRect.height += 2;
            float thisCycleChance = 1 - randStep * (i + 1);
            MakeBox(fadeoutRect, getGround, tileset, thisCycleChance);
        }
    }

    public void FillRect(Rect rect, System.Func<Tileset, TileBase> func, Tileset tileset, float fillPerc = 1)
    {
        int width = (int)(rect.width);
        int height = (int)(rect.height);
        for (int counter_width = 0; counter_width < width; counter_width++)
        {
            for (int counter_height = 0; counter_height < height; counter_height++)
            {

                if (UnityEngine.Random.value < fillPerc)
                {

                    tilemap.SetTile(new Vector3Int((int)(rect.xMin) + counter_width, (int)(rect.yMin) + counter_height, 0), func.Invoke(tileset));
                }
            }
        }
    }

}

public class MapModifier
{
    GameMap gameMap;

    public MapModifier(GameMap gameMap)
    {
        this.gameMap = gameMap;
    }
    /// <summary>
    /// Deprecated
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="tileset"></param>
    /// <returns></returns>
    public bool IsCorner(Vector3Int tile, Tileset tileset)
    {
        List<Vector3Int> list = VectorOperations.GetSurroundingTiles(tile);
        int corners = 0;
        if (tileset.GetIndoorTiles().Contains(gameMap.ground.GetTile(list[1])))
        {
            corners++;
        }
        if (tileset.GetIndoorTiles().Contains(gameMap.ground.GetTile(list[3])))
        {
            corners++;
        }
        if (tileset.GetIndoorTiles().Contains(gameMap.ground.GetTile(list[5])))
        {
            corners++;
        }
        if (tileset.GetIndoorTiles().Contains(gameMap.ground.GetTile(list[7])))
        {
            corners++;
        }
        return corners == 1;
    }

    public TileBase GetGroundTile(Vector3Int position)
    {
        return gameMap.ground.GetTile(position);
    }

    public TileBase GetStructureTile(Vector3Int position)
    {
        return gameMap.structures.GetTile(position);
    }
}

public class RandomRectangleConstructor
{
    Vector2Int minimalXY;
    Vector2Int maximalXY;

    public RandomRectangleConstructor(Vector2Int minimalXY, Vector2Int maximalXY)
    {
        this.minimalXY = minimalXY;
        this.maximalXY = maximalXY;
    }

    public Rect CreateRandomRectangleInArea(Rect Area)
    {
        int width = (int)((maximalXY.x - minimalXY.x) * Random.value + minimalXY.x);
        int height = (int)((maximalXY.y - minimalXY.y) * Random.value + minimalXY.y);
        int ypos = (int)((Area.height - height) * Random.value);
        int xpos = (int)((Area.width - width) * Random.value);
        return new Rect((int)(Area.xMin + xpos), (int)(Area.yMin + ypos), width, height);
    }

    public List<Rect> CreateRandomRectanglesInArea(Rect area, int tries)
    {
        List<Rect> rooms = new List<Rect>();
        for (int i = 0; i < tries; i++)
        {
            Rect newRoom = CreateRandomRectangleInArea(area);
            Rect newRoomPlus1 = new Rect(newRoom);
            newRoomPlus1.yMin -= 1;
            newRoomPlus1.xMin -= 1;
            newRoomPlus1.height += 2;
            newRoomPlus1.width += 2;
            //Debug.Log(newRoom.width + " " + newRoom.height);
            bool overlaps = false;
            foreach (Rect room in rooms)
            {
                if (!overlaps && newRoomPlus1.Overlaps(room, true))
                {
                    overlaps = true;
                }
            }
            if (!overlaps)
            {
                rooms.Add(newRoom);
            }
        }
        return rooms;
    }


}

public static class VectorOperations
{
    public static Vector3Int GetVector3FromVector2(Vector2Int vector)
    {
        return new Vector3Int(vector.x, vector.y, 0);
    }

    public static Vector3Int GenerateRandomPositionOnBorder(Rect r1)
    {
        int tunPosX = (int)((r1.width - 2) * Random.value) + 1;
        int tunPosY = (int)((r1.height - 2) * Random.value) + 1;
        if (Mathf.RoundToInt(Random.value) == 1)
        {
            if (r1.width - tunPosX <= tunPosX)
            {
                tunPosX = (int)r1.xMax - 1;
            }
            else
            {
                tunPosX = (int)r1.xMin;
            }
            tunPosY = tunPosY + (int)r1.yMin;
        }
        else
        {
            if (r1.height - tunPosY <= tunPosY)
            {
                tunPosY = (int)r1.yMax - 1;
            }
            else
            {
                tunPosY = (int)r1.yMin;
            }
            tunPosX = tunPosX + (int)r1.xMin;
        }
        return new Vector3Int(tunPosX, tunPosY, 0);
    }

    public static Vector3Int MoveCursor(Vector3Int cursor, Directions dir)
    {
        Vector3Int newCursor = cursor;
        switch (dir)
        {
            case Directions.Down:
                {
                    newCursor.y--;
                    break;
                }
            case Directions.Up:
                {
                    newCursor.y++;
                    break;
                }
            case Directions.Left:
                {
                    newCursor.x--;
                    break;
                }
            case Directions.Right:
                {
                    newCursor.x++;
                    break;
                }
        }
        return newCursor;
    }

    public static List<Vector3Int> GetSurroundingTiles(Vector3Int tile)
    {
        List<Vector3Int> listOfTiles = new List<Vector3Int>();
        listOfTiles.Add(new Vector3Int(tile.x, tile.y + 1, 0));
        listOfTiles.Add(new Vector3Int(tile.x + 1, tile.y + 1, 0));
        listOfTiles.Add(new Vector3Int(tile.x + 1, tile.y, 0));
        listOfTiles.Add(new Vector3Int(tile.x + 1, tile.y - 1, 0));
        listOfTiles.Add(new Vector3Int(tile.x, tile.y - 1, 0));
        listOfTiles.Add(new Vector3Int(tile.x - 1, tile.y - 1, 0));
        listOfTiles.Add(new Vector3Int(tile.x - 1, tile.y, 0));
        listOfTiles.Add(new Vector3Int(tile.x - 1, tile.y + 1, 0));
        return listOfTiles;
    }

    public static List<Vector3Int> GetRandomCoordinatesInArea(Rect area, float fillPerc)
    {
        int width = (int)(area.width);
        int height = (int)(area.height);
        List<Vector3Int> coords = new List<Vector3Int>();
        for (int counter_width = (int)area.xMin; counter_width < (int)area.xMax; counter_width++)
        {
            for (int counter_height = (int)area.yMin; counter_height < (int)area.yMax; counter_height++)
            {
                if (UnityEngine.Random.value < fillPerc)
                {
                    coords.Add(new Vector3Int(counter_width, counter_height, 0));
                }
            }
        }
        return coords;
    }

}

public static class DirectonsOperations
{
    public static Directions GetTunnelDirectionFromRoom(Vector3Int cursor, Rect room)
    {
        if (cursor.y == room.yMax - 1)
        {
            return Directions.Up;
        }
        if (cursor.y == room.yMin)
        {
            return Directions.Down;
        }
        if (cursor.x == room.xMax - 1)
        {
            return Directions.Right;
        }
        if (cursor.x == room.xMin)
        {
            return Directions.Left;
        }
        else
        {
            throw new System.Exception("Cursor is not on wall");
        }
    }

    public static Directions ChangeDirection(Directions dir)
    {
        int newDir = (int)dir;
        int forbiddenDirection = 0;
        switch (dir)
        {
            case Directions.Down:
                {
                    forbiddenDirection = (int)Directions.Up;
                    break;
                }
            case Directions.Up:
                {
                    forbiddenDirection = (int)Directions.Down;
                    break;
                }
            case Directions.Left:
                {
                    forbiddenDirection = (int)Directions.Right;
                    break;
                }
            case Directions.Right:
                {
                    forbiddenDirection = (int)Directions.Left;
                    break;
                }
        }
        do
        {
            newDir = Random.Range((int)Directions.Up, (int)Directions.Right);
        } while (newDir == forbiddenDirection);
        return (Directions)newDir;
    }

}
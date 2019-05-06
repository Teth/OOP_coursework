using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StructureGenerator 
{
    GameMap map;
    Tileset tiles;

    const int DUNGEON_GENERATE_CYCLE = 10000;
    const int TUNNELS_FROM_ROOM = 5;
    const int MAX_WIDHT = 18;
    const int MAX_HEIGHT = 18;
    const int MIN_WIDHT = 5;
    const int MIN_HEIGHT = 4;


    public StructureGenerator(GameMap map, Tileset tileset)
    {
        this.map = map;
        this.tiles = tileset;
    }


    Vector3Int GetVector3FromVector2(Vector2Int vector)
    {
        return new Vector3Int(vector.x, vector.y, 0);
    }
    Rect CreateRandomRectangleInArea(Rect Area)
    {
        int width = (int)((MAX_WIDHT - MIN_WIDHT) * Random.value + MIN_WIDHT);
        int height = (int)((MAX_HEIGHT - MIN_HEIGHT) * Random.value + MIN_HEIGHT);
        int ypos = (int)((Area.height - height) * Random.value);
        int xpos = (int)((Area.width - width) * Random.value);
       
        return new Rect((int)(Area.xMin + xpos), (int)(Area.yMin + ypos),width , height);
    }
    bool IsCorner(Vector3Int tile)
    {
        List<Vector3Int> list = GetSurroundingTiles(tile);
        int corners = 0; 
        if(tiles.GetIndoorTiles().Contains(map.ground.GetTile(list[1])))
        {
            corners++;
        }
        if (tiles.GetIndoorTiles().Contains(map.ground.GetTile(list[3])))
        {
            corners++;
        }
        if (tiles.GetIndoorTiles().Contains(map.ground.GetTile(list[5])))
        {
            corners++;
        }
        if (tiles.GetIndoorTiles().Contains(map.ground.GetTile(list[7])))
        {
            corners++;
        }
        return corners == 1;
    }
    Vector2Int GenerateRandomPositionOnBorder(Rect r1)
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
        return new Vector2Int(tunPosX, tunPosY);
    }
    Directions GetTunnelDirectionFromRoom(Vector3Int cursor, Rect room)
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
    Directions ChangeDirection(Directions dir)
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
    Vector3Int MoveCursor(Vector3Int cursor, Directions dir)
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
    List<Vector3Int> CreateTunnel(Rect room)
    {
        bool connected = false;
        int changeDirectionCounter = 0;
        int maxLength = 12;
        int length = 0;
        int lengthBeforeChangingDirection = (int)(Random.value * 5) + 2;


        Directions dir;
        Vector3Int cursor = GetVector3FromVector2(GenerateRandomPositionOnBorder(room));
        dir = GetTunnelDirectionFromRoom(cursor, room);
        map.structures.SetTile(new Vector3Int(cursor.x, cursor.y, 0), null);
        
        List<Vector3Int> tunnelTiles = new List<Vector3Int>();
        while (!connected)
        {
            map.ground.SetTile(cursor, tiles.GetIndoorTile());
            tunnelTiles.Add(cursor);
            cursor = MoveCursor(cursor, dir);

            if (tiles.GetStructureTiles().Contains(map.structures.GetTile(cursor)))
            {
                //merge into a room
                //clears a block to enter the rooom
                if(IsCorner(new Vector3Int(cursor.x, cursor.y, 0)))
                {
                    //Extends 1 block to reach room
                    tunnelTiles.Add(cursor);
                    map.structures.SetTile(cursor, null);
                    cursor = MoveCursor(cursor, dir);
                }
                tunnelTiles.Add(cursor);
                map.structures.SetTile(cursor, null);
                connected = true;
            }
            else if (tiles.GetIndoorTiles().Contains(map.ground.GetTile(cursor)))
            {
                //merge into another tunnel
                connected = true;
            }
            if (changeDirectionCounter == lengthBeforeChangingDirection)
            {
                //random direction change
                lengthBeforeChangingDirection = (int)(Random.value * 5) + 2;
                dir = ChangeDirection(dir);
                changeDirectionCounter = 0;
            }
            length++;
            changeDirectionCounter++;

            if (length == maxLength)
            {
                // end of tunnel
                map.ground.SetTile(new Vector3Int(cursor.x, cursor.y, 0), tiles.GetIndoorTile());
                connected = true;
            }
        }
        return tunnelTiles;
    }
    private void CreateTunnelWalls(List<Vector3Int> tunnelTiles)
    {
        foreach(Vector3Int tile in tunnelTiles)
        {
            foreach(Vector3Int surrTile in GetSurroundingTiles(tile))
            {
                if(!tiles.GetIndoorTiles().Contains(map.ground.GetTile(surrTile)) && !tiles.GetStructureTiles().Contains(map.structures.GetTile(surrTile)))
                {
                    map.structures.SetTile(surrTile, tiles.GetStructureTile());
                }
            }
        }
    }
    List<Vector3Int> GetSurroundingTiles(Vector3Int tile)
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
    List<Rect> GetRandomRoomsInArea(Rect area, int tries)
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

    public void CreateDungeon(Rect dungeonArea)
    {

        List<Rect> rooms = GetRandomRoomsInArea(dungeonArea, DUNGEON_GENERATE_CYCLE);
        List<Vector3Int> tunnels = new List<Vector3Int>();
        System.Func<Tileset, TileBase> getStructure = tileset => tileset.GetStructureTile();
        System.Func<Tileset, TileBase> getIndoor = tileset => tileset.GetIndoorTile();
        foreach (Rect room in rooms)
        {
            MapHelper.MakeBox(map.structures, room, getStructure, tiles);
            MapHelper.FillRect(map.ground, room, getIndoor, tiles);
        }
        foreach (Rect room in rooms)
        {
            for (int i = 0; i < TUNNELS_FROM_ROOM; i++)
            {
                tunnels.AddRange(CreateTunnel(room));
            }
        }
        CreateTunnelWalls(tunnels);
        
    }

    public void CreateVillage(Rect area)
    {
        List<Rect> rooms = GetRandomRoomsInArea(area, DUNGEON_GENERATE_CYCLE);
        List<Vector3Int> tunnels = new List<Vector3Int>();
        System.Func<Tileset, TileBase> getStructure = tileset => tileset.GetStructureTile();
        System.Func<Tileset, TileBase> getIndoor = tileset => tileset.GetIndoorTile();
        foreach (Rect room in rooms)
        {
            MapHelper.MakeRoom(map.structures, room, getStructure, tiles);
            MapHelper.FillRect(map.ground, room, getIndoor, tiles);
        }
    }
}

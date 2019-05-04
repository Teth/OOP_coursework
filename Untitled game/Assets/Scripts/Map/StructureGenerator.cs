using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StructureGenerator 
{
    GameMap map;
    ITileset tiles;

    const int DUNGEON_GENERATE_CYCLE = 10000;
    const int TUNNELS_FROM_ROOM = 5;
    const int MAX_WIDHT = 18;
    const int MAX_HEIGHT = 18;
    const int MIN_WIDHT = 5;
    const int MIN_HEIGHT = 4;


    public StructureGenerator(GameMap map, ITileset tileset)
    {
        this.map = map;
        this.tiles = tileset;
    }

    Rect createRandomRectangleInArea(Rect Area)
    {
        int width = (int)((MAX_WIDHT - MIN_WIDHT) * Random.value + MIN_WIDHT);
        int height = (int)((MAX_HEIGHT - MIN_HEIGHT) * Random.value + MIN_HEIGHT);
        int ypos = (int)((Area.height - height) * Random.value);
        int xpos = (int)((Area.width - width) * Random.value);
       
        return new Rect((int)(Area.xMin + xpos), (int)(Area.yMin + ypos),width , height);
    }
    enum directions
    {
        Up,
        Down,
        Left,
        Right
    }
    bool isCorner(Vector3Int tile)
    {
        List<Vector3Int> list = getSurroundingTiles(tile);
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
    Vector2Int generateRandomPositionOnWall(Rect r1)
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
    directions getTunnelDirectionFromRoom(Vector2Int cursor, Rect room)
    {
        if (cursor.y == room.yMax - 1)
        {
            return directions.Up;
        }
        if (cursor.y == room.yMin)
        {
            return directions.Down;
        }
        if (cursor.x == room.xMax - 1)
        {
            return directions.Right;
        }
        if (cursor.x == room.xMin)
        {
            return directions.Left;
        }
        else
        {
            throw new System.Exception("Cursor is not on wall");
        }
        
    }
    directions changeDirection(directions dir)
    {
        int newDir = (int)dir;
        int forbiddenDirection = 0;
        switch (dir)
        {
            case directions.Down:
                {
                    forbiddenDirection = (int)directions.Up;
                    break;
                }
            case directions.Up:
                {
                    forbiddenDirection = (int)directions.Down;
                    break;
                }
            case directions.Left:
                {
                    forbiddenDirection = (int)directions.Right;
                    break;
                }
            case directions.Right:
                {
                    forbiddenDirection = (int)directions.Left;
                    break;
                }
        }
        do
        {
            newDir = Random.Range((int)directions.Up, (int)directions.Right);
        } while (newDir == forbiddenDirection);
        return (directions)newDir;
    }
    Vector2Int MoveCursor(Vector2Int cursor, directions dir)
    {
        Vector2Int newCursor = cursor;
        switch (dir)
        {
            case directions.Down:
                {
                    newCursor.y--;
                    break;
                }
            case directions.Up:
                {
                    newCursor.y++;
                    break;
                }
            case directions.Left:
                {
                    newCursor.x--;
                    break;
                }
            case directions.Right:
                {
                    newCursor.x++;
                    break;
                }
        }
        return newCursor;
    }
    List<Vector3Int> CreateTunnel(Rect room)
    {    
        directions dir;

        Vector2Int cursor = generateRandomPositionOnWall(room);
        dir = getTunnelDirectionFromRoom(cursor, room);
        bool connected = false;
        map.structures.SetTile(new Vector3Int(cursor.x, cursor.y, 0), null);
        int ch_dir = 0;
        int maxLength = 12;
        int length = 0;
        int max_chDirCounter = (int)(Random.value * 5) + 2;
        List<Vector3Int> tunnelTiles = new List<Vector3Int>();
        while (!connected)
        {
            map.ground.SetTile(new Vector3Int(cursor.x, cursor.y, 0), tiles.GetIndoorTile());

            tunnelTiles.Add(new Vector3Int(cursor.x, cursor.y, 0));

            cursor = MoveCursor(cursor, dir);

            if (tiles.GetStructureTiles().Contains(map.structures.GetTile(new Vector3Int(cursor.x, cursor.y, 0))))
            {
                //merge into a room
                if(isCorner(new Vector3Int(cursor.x, cursor.y, 0)))
                {
                    tunnelTiles.Add(new Vector3Int(cursor.x, cursor.y, 0));
                    map.structures.SetTile(new Vector3Int(cursor.x, cursor.y, 0), null);
                    cursor = MoveCursor(cursor, dir);
                }
                tunnelTiles.Add(new Vector3Int(cursor.x, cursor.y, 0));
                map.structures.SetTile(new Vector3Int(cursor.x, cursor.y, 0), null);
                connected = true;
            }
            else if (tiles.GetIndoorTiles().Contains(map.ground.GetTile(new Vector3Int(cursor.x, cursor.y, 0))))
            {
                //merge into another tunnel
                connected = true;
            }

            if (ch_dir == max_chDirCounter)
            {
                //random direction change
                max_chDirCounter = (int)(Random.value * 5) + 2;
                dir = changeDirection(dir);
                ch_dir = 0;
            }
            length++;
            ch_dir++;

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
            foreach(Vector3Int surrTile in getSurroundingTiles(tile))
            {
                if(!tiles.GetIndoorTiles().Contains(map.ground.GetTile(surrTile)) && !tiles.GetStructureTiles().Contains(map.structures.GetTile(surrTile)))
                {
                    map.structures.SetTile(surrTile, tiles.GetStructureTile());
                }
            }
        }
    }
    List<Vector3Int> getSurroundingTiles(Vector3Int tile)
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
    List<Rect> getRandomRoomsInArea(Rect area, int tries)
    {
        List<Rect> rooms = new List<Rect>();
        for (int i = 0; i < tries; i++)
        {
            Rect newRoom = createRandomRectangleInArea(area);
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

        List<Rect> rooms = getRandomRoomsInArea(dungeonArea, DUNGEON_GENERATE_CYCLE);
        List<Vector3Int> tunnels = new List<Vector3Int>();
        System.Func<ITileset, TileBase> getStructure = tileset => tileset.GetStructureTile();
        System.Func<ITileset, TileBase> getIndoor = tileset => tileset.GetIndoorTile();
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
        Debug.Log("HEY");
        CreateTunnelWalls(tunnels);
        
    }
}

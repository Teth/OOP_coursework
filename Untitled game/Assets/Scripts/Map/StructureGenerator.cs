using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StructureGenerator 
{
    GameMap map;
    TileBase[] tiles;

    const int dungeonTries = 1000;
    const int MAX_WIDHT = 10;
    const int MAX_HEIGHT = 10;


    public StructureGenerator(GameMap map, TileBase[] tiles)
    {
        this.map = map;
        this.tiles = tiles;
    }

    Rect createRandomRectangleInArea(Rect Area)
    {
        int width = (int)((MAX_WIDHT - 4) * Random.value + 4);
        int height = (int)((MAX_HEIGHT - 4) * Random.value + 4);
        int ypos = (int)((Area.height - height) * Random.value);
        int xpos = (int)((Area.width - width) * Random.value);
       
        return new Rect((int)(Area.xMin + xpos), (int)(Area.yMin + ypos),width , height);
    }

    void ConnectRooms(Rect r1)
    {
        // DODELAT
        Debug.Log("Connectiong rooms");
        int tunPosX = (int)((r1.width) * Random.value);
        int tunPosY = (int)((r1.height) * Random.value);
        if (Mathf.RoundToInt(Random.value) == 1)
        {
            // door on vertical
            if (r1.width - tunPosX <= tunPosX)
            {
                tunPosX = (int)r1.xMax;
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
                tunPosY = (int)r1.yMax;
            }
            else
            {
                tunPosY = (int)r1.yMin;
            }
            tunPosX = tunPosX + (int)r1.xMin;
        }
        Vector2Int cursor = new Vector2Int(tunPosX, tunPosY);
        bool connected = false;
        map.decorations.SetTile(new Vector3Int(cursor.x, cursor.y, 0), tiles[1]);
        while (!connected)
        {
            if(map.structures.GetTile(new Vector3Int(cursor.x, cursor.y + 1, 0)) == tiles[2] && map.structures.GetTile(new Vector3Int(cursor.x, cursor.y - 1, 0)) == tiles[2])
            {
                // start point is verticall wall
                if (map.ground.GetTile(new Vector3Int(cursor.x + 1, cursor.y, 0)) == tiles[3])
                    map.decorations.SetTile(new Vector3Int(cursor.x - 1, cursor.y, 0), tiles[1]);
                else
                    map.decorations.SetTile(new Vector3Int(cursor.x + 1, cursor.y, 0), tiles[1]);
            }else if (map.structures.GetTile(new Vector3Int(cursor.x + 1, cursor.y, 0)) == tiles[2] && map.structures.GetTile(new Vector3Int(cursor.x, cursor.y - 1, 0)) == tiles[2])
            {
                // start point is horisontal wall
                if (map.ground.GetTile(new Vector3Int(cursor.x , cursor.y + 1, 0)) == tiles[3])
                    map.decorations.SetTile(new Vector3Int(cursor.x , cursor.y - 1, 0), tiles[1]);
                else
                    map.decorations.SetTile(new Vector3Int(cursor.x, cursor.y + 1, 0), tiles[1]);
            }
            else
            {
                Debug.Log("Ayyy");
            }
            connected = true;
        }
    }


    //{
    //    List<int> xSharedTiles = new List<int>();
    //    if(r1.xMin + r1.width < r2.xMin || r2.xMin + r2.width < r1.xMin)
    //    {
    //        int lowerBound = (int)(r1.xMin < r2.xMin ? r1.xMin : r2.xMin);
    //        int maxWidth = (int)(r1.width < r2.width ? r2.width : r1.width);
    //        for (int xCounter = lowerBound; xCounter < lowerBound + maxWidth; xCounter++)
    //        {
    //            if(xCounter >= r1.xMin && xCounter <= r2.xMin || xCounter >= r2.xMin && xCounter <= r1.xMin)
    //            {
    //                xSharedTiles.Add(xCounter);
    //            }
    //        }
    //    }
    //    foreach(int x in xSharedTiles)

    //    //List<int> ySharedTiles = new List<int>();
    //    //if (r1.yMin + r1.height < r2.yMin || r2.yMin + r2.height < r1.yMin)
    //    //{
    //    //    int lowerBound = (int)(r1.yMin < r2.yMin ? r1.yMin : r2.yMin);
    //    //    int maxHeight = (int)(r1.height < r2.height ? r2.height : r1.width);
    //    //    for (int yCounter = lowerBound; yCounter < lowerBound + maxHeight; yCounter++)
    //    //    {
    //    //        if (yCounter >= r1.yMin && yCounter <= r2.yMin || yCounter >= r2.yMin && yCounter <= r1.yMin)
    //    //        {
    //    //            ySharedTiles.Add(yCounter);
    //    //        }
    //    //    }
    //    //}
    //    //MapHelper.FillRect(map.decorations, new Rect(xSharedTiles[0] + 1, ySharedTiles[0] + 1, xSharedTiles.Count - 2, ySharedTiles.Count -2),tiles[1]);

    public void CreateDungeon(Rect dungeonArea)
    {
        List<Rect> rooms = new List<Rect>();
        for(int i = 0; i < dungeonTries; i++)
        {
            Rect newRoom = createRandomRectangleInArea(dungeonArea);
            Rect newRoomPlus1 = new Rect(newRoom);
            newRoomPlus1.yMin -= 1;
            newRoomPlus1.xMin -= 1;
            newRoomPlus1.height += 2;
            newRoomPlus1.width += 2;
            bool overlaps = false;
            foreach(Rect room in rooms){
                if (!overlaps && newRoomPlus1.Overlaps(room, true))
                {
                    overlaps = true;
                }
            }
            if(!overlaps)
            {
                rooms.Add(newRoom);
                MapHelper.MakeRoom(map.structures, newRoom, tiles[2]);
                MapHelper.FillRect(map.ground, newRoom, tiles[3]);
            }
        }
        ConnectRooms(rooms[0]);
    }
}

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
    const int MAX_WIDTH = 13;
    const int MAX_HEIGHT = 15;
    const int MIN_WIDTH = 5;
    const int MIN_HEIGHT = 4;

    protected TilemapModifier groundModifier;
    protected TilemapModifier structuresModifier;
    protected RandomRectangleConstructor roomRectangleConstructor;
    protected MapOperations mapOperations;
    public StructureGenerator(GameMap map, Tileset tileset)
    {
        this.map = map;
        this.tiles = tileset;
        structuresModifier = new TilemapModifier(map.structures);
        groundModifier = new TilemapModifier(map.ground);
        roomRectangleConstructor = new RandomRectangleConstructor(new Vector2Int(MIN_WIDTH, MIN_HEIGHT), new Vector2Int(MAX_WIDTH, MAX_HEIGHT));
        mapOperations = new MapOperations(map);
    }

    List<Vector3Int> CreateTunnelFromRoom(Rect room)
    {
        int maxLength = 12;
        int lengthBeforeChangingDirection = (int)(Random.value * 5) + 2;

        Directions dir;
        bool connected = false;
        int length = 0;
        int changeDirectionCounter = 0;

        Vector3Int cursor = VectorOperations.GetVector3FromVector2(VectorOperations.GenerateRandomPositionOnBorder(room));
        dir = DirectonsOperations.GetTunnelDirectionFromRoom(cursor, room);
        map.structures.SetTile(cursor, null);
        
        List<Vector3Int> tunnelTiles = new List<Vector3Int>();
        
        while (!connected)
        {
            map.ground.SetTile(cursor, tiles.GetIndoorTile());
            tunnelTiles.Add(cursor);
            cursor = VectorOperations.MoveCursor(cursor, dir);

            if (tiles.GetIndoorTiles().Contains(map.ground.GetTile(cursor)))
            {
                //merge into another tunnel
                connected = true;
            }
            if (changeDirectionCounter == lengthBeforeChangingDirection)
            {
                //random direction change
                lengthBeforeChangingDirection = (int)(Random.value * 5) + 2;
                dir = DirectonsOperations.ChangeDirection(dir);
                changeDirectionCounter = 0;
            }
            length++;
            changeDirectionCounter++;

            if (length == maxLength)
            {
                // end of tunnel
                map.ground.SetTile(cursor, tiles.GetIndoorTile());
                connected = true;
            }
        }
        return tunnelTiles;
    }

    private void CreateWalls(List<Vector3Int> tunnelTiles)
    {
        foreach(Vector3Int tile in tunnelTiles)
        {
            foreach(Vector3Int surrTile in VectorOperations.GetSurroundingTiles(tile))
            {
                if(!tiles.GetIndoorTiles().Contains(mapOperations.GetGroundTile(surrTile)))
                {
                    map.structures.SetTile(surrTile, tiles.GetStructureTile());
                }
            }
        }
    }

    public void CreateDungeon(Rect dungeonArea)
    {
        Debug.Log(roomRectangleConstructor);
        List<Rect> rooms = roomRectangleConstructor.CreateRandomRectanglesInArea(dungeonArea, DUNGEON_GENERATE_CYCLE);
        
        System.Func<Tileset, TileBase> getStructure = tileset => tileset.GetStructureTile();
        System.Func<Tileset, TileBase> getIndoor = tileset => tileset.GetIndoorTile();

        List<Vector3Int> tilesToWallOff = new List<Vector3Int>();

        foreach (Rect room in rooms)
        {
            groundModifier.FillRect(room, getIndoor, tiles);
            tilesToWallOff.AddRange(structuresModifier.MakeBox(room, null));
        }
        foreach (Rect room in rooms)
        {
            for (int i = 0; i < TUNNELS_FROM_ROOM; i++)
            {
                tilesToWallOff.AddRange(CreateTunnelFromRoom(room));
            }
        }
        CreateWalls(tilesToWallOff);   
    }

    public void CreateVillage(Rect area)
    {
        List<Rect> rooms = roomRectangleConstructor.CreateRandomRectanglesInArea(area, DUNGEON_GENERATE_CYCLE);

        TilemapModifier structuresModifier = new TilemapModifier(map.structures);
        TilemapModifier groundModifier = new TilemapModifier(map.ground);

        System.Func<Tileset, TileBase> getStructure = tileset => tileset.GetStructureTile();
        System.Func<Tileset, TileBase> getIndoor = tileset => tileset.GetIndoorTile();

        foreach (Rect room in rooms)
        {
            structuresModifier.MakeRoom(room, getStructure, tiles);
            groundModifier.FillRect(room, getIndoor, tiles);
        }
    }
}

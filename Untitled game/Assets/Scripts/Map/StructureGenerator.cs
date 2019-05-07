using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// FACADE pattern

public class MapFacade 
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
    protected TilemapModifier decorationsModifier;
    protected RandomRectangleConstructor roomRectangleConstructor;
    protected MapOperations mapOperations;
    protected RandomRectangleConstructor globalMapRectangleConstructor;

    public MapFacade(GameMap map, Tileset tileset)
    {
        this.map = map;
        this.tiles = tileset;
        structuresModifier = new TilemapModifier(map.structures);
        groundModifier = new TilemapModifier(map.ground);
        decorationsModifier = new TilemapModifier(map.decorations);

        roomRectangleConstructor = new RandomRectangleConstructor(new Vector2Int(MIN_WIDTH, MIN_HEIGHT), new Vector2Int(MAX_WIDTH, MAX_HEIGHT));
        globalMapRectangleConstructor = new RandomRectangleConstructor(new Vector2Int(map.sizeX / 3, map.sizeY / 3), new Vector2Int((int)(map.sizeX / 1.2), (int)(map.sizeY / 1.2)));

        mapOperations = new MapOperations(map);
    }

    public void CreateDungeonInArea(Rect area)
    {
        CreateDungeon(globalMapRectangleConstructor.CreateRandomRectangleInArea(area));
    }

    public void CreateVillageInArea(Rect area)
    {
        CreateVillage(globalMapRectangleConstructor.CreateRandomRectangleInArea(area));
    }

    public void CreateRuinsInArea(Rect area)
    {
        CreateRuins(globalMapRectangleConstructor.CreateRandomRectangleInArea(area));
    }

    private void CreateRuins(Rect area)
    {
        List<Rect> rooms = roomRectangleConstructor.CreateRandomRectanglesInArea(area, DUNGEON_GENERATE_CYCLE);

        TilemapModifier structuresModifier = new TilemapModifier(map.structures);
        TilemapModifier groundModifier = new TilemapModifier(map.ground);

        System.Func<Tileset, TileBase> getStructure = tileset => tileset.GetStructureTile();
        System.Func<Tileset, TileBase> getIndoor = tileset => tileset.GetIndoorTile();

        foreach (Rect room in rooms)
        {
            structuresModifier.MakeRoom(room, getStructure, tiles, 0.4f);
            groundModifier.FillRect(room, getIndoor, tiles);
            groundModifier.GenerateFadeout(room, getIndoor, tiles, 2);
        }
    }

    public void GenerateDecorations(Rect area, float appearRate = 0.05f)
    {
        List<Vector3Int> listOfDecoCoordinates = VectorOperations.GetRandomCoordinatesInArea(area, appearRate);
        foreach(Vector3Int coord in listOfDecoCoordinates)
        {
            bool isIndoor = true;
            if(tiles.GetGroundTiles().Contains(mapOperations.GetGroundTile(coord)))
            {
                isIndoor = false;
            }
            decorationsModifier.SetTile(coord, tiles.GetDecorationTile(isIndoor));
        }
    }

    public void CreateGround(Rect area)
    {
        System.Func<Tileset, TileBase> getGround = tileset => tileset.GetGroundTile();

        groundModifier.FillRect(area, getGround, tiles);
        groundModifier.GenerateFadeout(area, getGround, tiles, Mathf.CeilToInt((area.height + area.width) / 16));
    }

    

    List<Vector3Int> CreateTunnelFromRoom(Rect room)
    {
        int maxLength = 12;
        int lengthBeforeChangingDirection = (int)(Random.value * 5) + 2;

        Directions dir;
        bool connected = false;
        int length = 0;
        int changeDirectionCounter = 0;

        Vector3Int cursor = VectorOperations.GenerateRandomPositionOnBorder(room);
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




    // private methods
    private void CreateVillage(Rect area)
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


    private void CreateWalls(List<Vector3Int> tunnelTiles)
    {
        foreach (Vector3Int tile in tunnelTiles)
        {
            foreach (Vector3Int surrTile in VectorOperations.GetSurroundingTiles(tile))
            {
                if (!tiles.GetIndoorTiles().Contains(mapOperations.GetGroundTile(surrTile)))
                {
                    map.structures.SetTile(surrTile, tiles.GetStructureTile());
                }
            }
        }
    }


    void CreateDungeon(Rect dungeonArea)
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

}

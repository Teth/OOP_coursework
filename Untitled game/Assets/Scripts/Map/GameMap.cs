using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameMap
{
    int sizeX;
    int sizeY;
    public Tilemap ground;
    public Tilemap decorations;
    public Tilemap structures;

    public GameMap(int sizeX, int sizeY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
    }
}

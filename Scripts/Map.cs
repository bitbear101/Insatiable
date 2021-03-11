using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EventCallback;

//The tile types for the map
public enum TileType
{
    FLOOR,
    STONE,
    WALL,
    DOOR,
    LADDER,
    NONE
};
public class Map : Node2D
{
    [Export]
    //The seed for the map
    int seed = 0;
    Vector2 spawnTile = Vector2.Zero;
    Vector2 exiTile = Vector2.Zero;
    //The size of the map
    int width = 50, height = 50;
    //The size of the tile
    const int tileSize = 16;
    //The number of the level
    int levelNumber = 0;
    //The tile map node in the node tree
    TileMap tileMap;
    List<TileType> map = new List<TileType>();
    //The simplex noise used for the map generation
    OpenSimplexNoise noise = new OpenSimplexNoise();
    //Create the random number generator for the map
    RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        //The stone to floor listener
        StoneToFloorEvent.RegisterListener(OnStoneToFloorEvent);
        //The listener for the get tile event
        GetTileEvent.RegisterListener(OnGetTileEvent);
        //The listener for the players spawn event
        GetPlayerSpawnPointEvent.RegisterListener(OnGetPlayerSpawnPointEvent);
        //The event message to get he used cells from the map
        GetUsedCellsEvent.RegisterListener(OnGetUsedCellsEvent);
        //Get the tile map node in the scene to be able to control it from the script
        tileMap = GetNode<TileMap>("TileMap");
        //Randomaize the output of the generator
        rng.Randomize();
        //Generates the noise
        GenerateNoise();
        //Build the level
        BuildLevel();
        //Get the tile the player will spawn at
        GetStartTile();
        //Get the tile the floor exit will spawn on
        GetExitTile();
    }

    private void OnStoneToFloorEvent(StoneToFloorEvent stfe)
    {
        //Change the stone tile to a floor tile in the map list
        map[(int)(stfe.TileToChange.x * width + stfe.TileToChange.y)] = TileType.FLOOR;
        //Change the stone tile to a floor tile in the tile map node
        tileMap.SetCell((int)stfe.TileToChange.x, (int)stfe.TileToChange.y, (int)TileType.FLOOR);
        //Update the astars map of the walkable cells
        UpdateMapCellsEvent umce = new UpdateMapCellsEvent();
        umce.callerClass = "Map - OnStoneToFloorEvent()";
        umce.FireEvent();
    }

    private void OnGetTileEvent(GetTileEvent gte)
    {
        //Get the tile from the map list
        gte.tile = map[(int)(gte.pos.x * width + gte.pos.y)];
    }

    private void OnGetUsedCellsEvent(GetUsedCellsEvent guce)
    {
        //Get the cells in the map
        guce.cells = tileMap.GetUsedCellsById((int)TileType.FLOOR).Cast<Vector2>().ToList();
    }

    private void OnGetPlayerSpawnPointEvent(GetPlayerSpawnPointEvent gpspe)
    {
        //Return the players spawn point to the message sender
        gpspe.spawnPos = spawnTile * tileSize;

    }

    private void GenerateNoise()
    {
        //Randomize the seed
        noise.Seed = seed;
        //Set the noises actave
        noise.Octaves = 2;
        //Set the noises period
        noise.Period = 10;
        noise.Lacunarity = 1.5f;
        noise.Persistence = 0.75f;
    }

    public void BuildLevel()
    {
        //Loop through the size of the map to create the base tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map.Add(GetTileFromIndex(noise.GetNoise2d((float)x, (float)y)));
                //Cast the enum value of the tile to an in to get the tile to spawn
                tileMap.SetCell(x, y, (int)GetTileFromIndex(noise.GetNoise2d((float)x, (float)y)));
                //If the tile is on the perimiter of the map we create a wall tile to encircle the whole map so the player cant escape HAHAHAHA!!11!1!
                if (x == 0 || y == 0)
                {
                    //Cast the enum value of the tile to an in to get the tile to spawn
                    tileMap.SetCell(x, y, (int)TileType.WALL);
                    map[x * width + y] = TileType.WALL;
                }
                if (x == width - 1 || y == height - 1)
                {
                    //Cast the enum value of the tile to an in to get the tile to spawn
                    tileMap.SetCell(x, y, (int)TileType.WALL);
                    map[x * width + y] = TileType.WALL;
                }
            }
        }
    }

    private TileType GetTileFromIndex(float index)
    {
        //Create the temp tile
        TileType tile = TileType.NONE;
        if (index < 0.01f)
        {
            tile = TileType.FLOOR;
        }
        if (index > 0.01f)
        {
            tile = TileType.STONE;
        }
        return tile;
    }

    private void GetStartTile()
    {
        //The alternative directions to check for a spawn point
        Vector2 checkTilePosSE, checkTilePosS, checkTilePosE, startTile;
        //Set the starting posisitons for all the directions to the top left of the map
        startTile = new Vector2(2, 2);
        checkTilePosSE = startTile;
        checkTilePosS = startTile;
        checkTilePosE = startTile;

        //If the top left of the maps tile is found to be a floor
        if (map[(int)startTile.x * (int)startTile.y] == TileType.FLOOR)
        {
            //We set the new spawn point to it
            spawnTile = startTile;
        }
        //Loop throught the the tiles until a candidate is found for the spawn tile
        while (spawnTile == Vector2.Zero)
        {
            //Increment the tile check positions by 3 tiles to either side
            checkTilePosSE += Vector2.One * 2;
            checkTilePosS += Vector2.Down * 2;
            checkTilePosE += Vector2.Right * 2;
            //Check the new tile positions
            if (map[(int)(checkTilePosSE.x * checkTilePosSE.y)] == TileType.FLOOR)
            {
                //We set the new spawn point to it
                spawnTile = checkTilePosSE;
            }
            else if (map[(int)(checkTilePosS.x * checkTilePosS.y)] == TileType.FLOOR)
            {
                //We set the new spawn point to it
                spawnTile = checkTilePosS;
            }
            else if (map[(int)(checkTilePosE.x * checkTilePosE.y)] == TileType.FLOOR)
            {
                //We set the new spawn point to it
                spawnTile = checkTilePosE;
            }
        }
    }
    private void GetExitTile()
    {
        //Spawn the exit in any of the corners the player is not at

        //Set the corners for the exit spawn
        Vector2 topRight, bottomLeft, bottomRight;
        topRight = new Vector2(width - 2, 2);
        bottomLeft = new Vector2(2, height - 2);
        bottomRight = new Vector2(width - 2, height - 2);

    }

}

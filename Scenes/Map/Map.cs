using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EventCallback;

//The tile types for the map
public enum TileType
{
    STONE = 1,
    FLOOR,
    WALL,
    DOOR,
    LADDER,
    NONE
};
public class Map : Node2D
{
    //The seed for the map
    [Export] int seed = 0;
    //The tile where the player is spawning and the entrance/stairs tile
    Vector2 spawnTile = Vector2.Zero;
    //The location where the lader tile spawns for the exit to the level
    Vector2 exiTile = Vector2.Zero;
    //The size of the map
    int width = 200, height = 200;
    //The size of the tile
    const int tileSize = 16;
    //The number of the level
    int currentLevel = 1;
    //The max amount of levels the game can have
    int maxLevels = 5;
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
        //The event message to get the used cells from the map
        GetUsedCellsEvent.RegisterListener(OnGetUsedCellsEvent);
        //The event listener for the generate level event message
        GenerateLevelEvent.RegisterListener(OnGenerateLevelEvent);
        //The listener for the get map level
        GetMapLevelEvent.RegisterListener(OnGetMapLevelEvent);
        //The listener for the Get Random Floor Tile Event message
        GetRandomFloorTileEvent.RegisterListener(OnGetRandomFloorTileEvent);
        //Get the tile map node in the scene to be able to control it from the script
        tileMap = GetNode<TileMap>("TileMap");
        //Generate the level when instances the first time
        GenerateLevel();
    }

    private void OnGenerateLevelEvent(GenerateLevelEvent gle)
    {
        //Call the generate level method
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        //Inciment the level whenever a new levl is generated
        currentLevel++;
        //Set the new map size
        width += (int)((float)width * 0.25f);
        height += (int)((float)height * 0.25f);
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
        if (map[(int)(stfe.TileToChange.x * width + stfe.TileToChange.y)] != TileType.WALL)
        {
            //Change the stone tile to a floor tile in the map list
            map[(int)(stfe.TileToChange.x * width + stfe.TileToChange.y)] = TileType.FLOOR;
            //Change the stone tile to a floor tile in the tile map node
            tileMap.SetCell((int)stfe.TileToChange.x, (int)stfe.TileToChange.y, (int)TileType.FLOOR);
            //Update the tile maps bit mask to show in game
            tileMap.UpdateBitmaskArea(stfe.TileToChange);
        }

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

    private void OnGetMapLevelEvent(GetMapLevelEvent gmle)
    {
        //Returning the requested map level to the message sender
        gmle.mapLevel = currentLevel;
        //Return the max levels the game has
        gmle.maxLevels = maxLevels;
    }

    private void OnGetRandomFloorTileEvent(GetRandomFloorTileEvent grfte)
    {
        //Get all the cells that are floor tile then loop through t them randomly and choose on to send back
        Vector2[] floorTiles = tileMap.GetUsedCellsById((int)TileType.FLOOR).Cast<Vector2>().ToArray();
        //Send the Vector2 of one of the randomly slected floor tiles back to the message caller
        grfte.tilePos = floorTiles[rng.RandiRange(0, floorTiles.Length - 1)];
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
        //Update the cell region for the map to make autotile look correct
        tileMap.UpdateBitmaskRegion(Vector2.Zero, new Vector2(width, height));
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
        //Get all the cells that are floor tile then loop through them randomly and choose one to send back
        Vector2[] floorTiles = tileMap.GetUsedCellsById((int)TileType.FLOOR).Cast<Vector2>().ToArray();
        //Randomize the seed for the random number generator
        rng.Randomize();
        //Get a random spawn tile from the map 
        int tilePos = rng.RandiRange(1, floorTiles.Length / 5);
        //Set the starting posisitons for all the directions to the top left of the map
        spawnTile = floorTiles[tilePos];
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

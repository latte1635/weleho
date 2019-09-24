using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CaveGen : MonoBehaviour
{

    [Header("Prefabs")]

    public GameObject playerPrefab;

    [Header("Wall Variants")]
    public GameObject caveWall;
    public GameObject caveWallVariant1;
    public GameObject caveWallVariant2;
    public GameObject caveWallVariant3;
    public GameObject caveWallVariant4;

    public GameObject caveUnbreakableWall;
    [Header("Floor Variants")]
    public GameObject caveFloor;
    public GameObject caveFloorVariant1;
    public GameObject caveFloorVariant2;
    public GameObject caveFloorVariant3;
    public GameObject caveFloorVariant4;

    public GameObject exitPrefab;
    public GameObject campfire;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    [Header("Spawnrates")]

    public int enemy1Count;
    public int enemy2Count;
    public int enemy3Count;
    public int enemy4Count;
    
    public List<GameObject> clones = new List<GameObject>();

    [Header("Cave generation variables")]

    
    public int width;
    public int height;

    public int worldBorder;

    public int seed;

    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    
    public int startAreaSize;
    public int safeAreaSize;

    [Range(0, 100)]
    public int wallRegionThresholdSize;

    [Range(0, 100)]
    public int floorRegionThresholdSize;

    int currentFloor = 0;

    int[,] map;

    public Text FloorNumberUI;

    List<Coord> spawnableArea = new List<Coord>();

    bool started = false;
    
    void Start()
    {
        MusicManager.PlaySound("card_castle");
        FloorNumberUI.text = "Current Floor: " + currentFloor.ToString();
        GenerateMap(worldBorder);
        Debug.Log("Clonecount: " + clones.Count);
        started = true;
    }

    void Update()
    {
        if (started == false)
        {
            Start();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            
            GenerateMap(worldBorder);
            Debug.Log("Clonecount: " + clones.Count);
        }
        
    }

    public int GetCurrentFloor()
    {
        return currentFloor;
    }

    void ClearMap()
    {
        if (clones.Count > 0)
        {
            width += 1;
            height += 1;
            //randomFillPercent--;
            for (int i = 0; i < clones.Count; i++)
            {
                Destroy(clones[i]);
            }
            clones.Clear();
        }
        if(spawnableArea.Count > 0)
        {
            spawnableArea.Clear();
        }
    }

    public void GenerateMap(int borderSize)
    {
        if (GameObject.FindWithTag("Player") == null)
        {
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        ClearMap();
        map = new int[width, height];

        GameObject.FindWithTag("Player").transform.position = Vector3.zero;
        GameObject.FindWithTag("MainCamera").transform.position = Vector3.zero;

        RandomFillMap(startAreaSize);

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }

        ProcessMap(wallRegionThresholdSize, floorRegionThresholdSize);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 0)
                {
                    spawnableArea.Add(new Coord(x, y));
                }
                int neighbourWallTiles = GetSurroundingWallCount(x, y);



                GenerateTileGameObjects(x, y, neighbourWallTiles);
            }
        }

        GenerateExit(exitPrefab, startAreaSize);
        GenerateCampfire(campfire);
        GenerateEnemiesOfType(enemy1, enemy1Count);
        GenerateEnemiesOfType(enemy2, enemy2Count);
        GenerateEnemiesOfType(enemy3, enemy3Count);
        GenerateEnemiesOfType(enemy4, enemy4Count);

        enemy1Count++;
        enemy2Count++;
        enemy3Count++;
        enemy4Count++;
        currentFloor++;
        FloorNumberUI.text = "Current Floor: " + currentFloor.ToString();
        
    }

    void ProcessMap(int wallThresholdSize, int roomThresholdSize)
    {
        List<List<Coord>> wallRegions = GetRegions(1);
        

        foreach (List<Coord> wallRegion in wallRegions)
        {
            if(wallRegion.Count < wallThresholdSize)
            {
                foreach (Coord tile in wallRegion)
                {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }

        List<List<Coord>> roomRegions = GetRegions(0);

        List<Room> survivingRooms = new List<Room>();

        
        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < roomThresholdSize)
            {
                foreach (Coord tile in roomRegion)
                {
                    map[tile.tileX, tile.tileY] = 1;
                }
            }
            else
            {
                survivingRooms.Add(new Room(roomRegion, map));
            }
        }

        survivingRooms.Sort();
        
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;

        ConnectClosestRooms(survivingRooms);
    }

    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
    {

        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessibilityFromMainRoom)
        {
            foreach (Room room in allRooms)
            {
                if (room.isAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }
        
        int bestDistance = 0;

        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();

        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;
        
        foreach (Room roomA in roomListA)
        {
            if (!forceAccessibilityFromMainRoom)
            {
                possibleConnectionFound = false;
                if(roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }
            foreach(Room roomB in roomListB)
            {
                if(roomA == roomB || roomA.IsConnected(roomB))
                {
                    continue;
                }

                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                    {
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        if(distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;

                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }

            if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }

        }

        if (possibleConnectionFound && forceAccessibilityFromMainRoom)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }

        if (!forceAccessibilityFromMainRoom)
        {
            ConnectClosestRooms(allRooms, true);
        }
    }

    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        Room.ConnectRooms(roomA, roomB);
        //Debug.Log("Viiva");
        //Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileA), Color.green, 1000);

        List<Coord> line = GetLine(tileA, tileB);
        //Debug.Log("Lines drawn between rooms: " + line.Count);
        foreach(Coord c in line)
        {
            DrawCircle(c, 2);
        }

    }

    void DrawCircle(Coord c, int r)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if(x*x + y*y <= r * r)
                {
                    int drawX = c.tileX + x;
                    int drawY = c.tileY + y;

                    if(IsInMapRange(drawX, drawY))
                    {
                        map[drawX, drawY] = 0;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();

        int x = from.tileX;
        int y = from.tileY;

        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;

        bool inverted = false;
        int step = Math.Sign (dx);
        int gradientStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for(int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));

            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradientAccumulation += shortest;
            if(gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }
        return line;
    }

    Vector3 CoordToWorldPoint(Coord tile)
    {
        return new Vector3(-width / 2 + .5f + tile.tileX, -height / 2 + .5f + tile.tileY, 0);
    }

    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(mapFlags[x, y] == 0 && map[x, y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }
        
        return regions;
    }
    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while(queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for(int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if(IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                    {
                        if(mapFlags[x, y] == 0 && map[x, y] == tileType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }
        return tiles;
    }

    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }


    void RandomFillMap(int startingAreaSize)
    {
        if (useRandomSeed)
        {
            seed = UnityEngine.Random.Range(1, 1000000);
        }
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 2;
                }
                /*if (x == 1 || x == width - 2 || y == 1 || y == height - 2)
                {
                    map[x, y] = 1;
                }*/
                else if (x > width / 2 - startingAreaSize && x < width / 2 + startingAreaSize && y > height / 2 - startingAreaSize && y < height / 2 + startingAreaSize)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = pseudoRandom.Next(0, 100) < randomFillPercent ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                
                if(neighbourWallTiles > 4 && map[x, y] != 2)
                {
                    map[x, y] = 1;
                }
                else if(neighbourWallTiles < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    void GenerateTileGameObjects(int x, int y, int neighbourWallTiles)
    {
        if(map[x, y] == 2 && neighbourWallTiles < 8)
        {
            GameObject unbreakableWall = Instantiate(caveUnbreakableWall, CoordToWorldPoint(new Coord(x, y)), Quaternion.identity);
            clones.Add(unbreakableWall);
        }
        if (map[x, y] == 1 && neighbourWallTiles < 8)
        {
            SelectAndSpawnWallPrefab(x, y);
        }
        if (map[x, y] == 0)
        {
            SelectAndSpawnFloorPrefab(x, y);
        }
    }

    void GenerateExit(GameObject exitPrefab, int startingAreaSize)
    {
        int randomX;
        int randomY;

        bool successfulSpawn = false;

        while (successfulSpawn == false)
        {

            randomX = UnityEngine.Random.Range(1, width);
            randomY = UnityEngine.Random.Range(1, height);

            if (map[randomX, randomY] == 0 && (randomX > width - 20 || randomX < 20 && randomY > height - 20 || randomY < 20))
            {
                GameObject exit = Instantiate(exitPrefab, CoordToWorldPoint(new Coord(randomX, randomY)), Quaternion.identity);
                clones.Add(exit);
                successfulSpawn = true;
            }
        }
    }

    void GenerateCampfire(GameObject campfire)
    {
        int randomX;
        int randomY;

        bool successfulSpawn = false;

        while (successfulSpawn == false)
        {

            randomX = UnityEngine.Random.Range(width / 2 - 6, width / 2 + 6);
            randomY = UnityEngine.Random.Range(height / 2 - 6, height / 2 + 6);

            if (map[randomX, randomY] == 0)
            {
                GameObject campFire = Instantiate(campfire, CoordToWorldPoint(new Coord(randomX, randomY)), Quaternion.identity);
                clones.Add(campFire);
                successfulSpawn = true;
            }
        }
    }

    void GenerateEnemiesOfType(GameObject enemy, int toSpawn)
    {
        int randomX;
        int randomY;

        int spawnCount = 0;
        while (spawnCount < toSpawn)
        {
            randomX = UnityEngine.Random.Range(1, width);
            randomY = UnityEngine.Random.Range(1, height);

            Coord randomTestCoord = new Coord(randomX, randomY);
            for(int i = 0; i < spawnableArea.Count; i++)
            {
                if(randomTestCoord.tileX == spawnableArea[i].tileX && randomTestCoord.tileY == spawnableArea[i].tileY && NotInSafeArea(randomX, randomY)) 
                {
                    GameObject enemy1 = Instantiate(enemy, CoordToWorldPoint(new Coord(randomX, randomY)), Quaternion.identity);
                    clones.Add(enemy1);
                    spawnCount++;
                    break;
                }
            }


            /*if (map[randomX, randomY] == 0 && NotInSpawnArea(randomX, randomY))
            {
                GameObject enemy1 = Instantiate(enemy, CoordToWorldPoint(new Coord(randomX, randomY)), Quaternion.identity);
                clones.Add(enemy1);
                spawnCount++;
            }*/
        }
    }

    bool NotInSafeArea(int x, int y)
    {
        if(!(x > width / 2 - safeAreaSize && x < width / 2 + safeAreaSize && y > height / 2 - safeAreaSize && y < height / 2 + safeAreaSize))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool MineWallTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject[] caveTiles = GameObject.FindGameObjectsWithTag("CaveWall");

        GameObject ClickedTile = null;

        foreach (GameObject tile in caveTiles)
        {
            if ((int)Math.Floor(tile.transform.position.x) == (int)Math.Floor(mousePos.x) && (int)Math.Floor(tile.transform.position.y) == (int)Math.Floor(mousePos.y))
            {

                ClickedTile = tile;
                Destroy(ClickedTile);
                SoundManagerScript.PlaySound("mine");
            }
        }
        if (ClickedTile != null)
        {
            ReplaceCaveTiles((int)Math.Floor(ClickedTile.transform.position.x), (int)Math.Floor(ClickedTile.transform.position.y));
            Destroy(ClickedTile);
            return true;
        }
        else
        {
            return false;
        }
    }

    void ReplaceCaveTiles(int x, int y)
    {
        int mapCoordX = (int)Math.Floor(width / 2 + x + .5f);
        int mapCoordY = (int)Math.Floor(height / 2 + y + .5f);

        Coord mapCoord = new Coord(mapCoordX, mapCoordY);

        CoordToWorldPoint(mapCoord);

        map[mapCoordX, mapCoordY] = 0;
        SelectAndSpawnFloorPrefab(mapCoordX, mapCoordY);

        for (int neighbourX = mapCoordX - 1; neighbourX <= mapCoordX + 1; neighbourX++)
        {
            for (int neighbourY = mapCoordY - 1; neighbourY <= mapCoordY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != mapCoordX || neighbourY != mapCoordY)
                    {
                        if(map[neighbourX, neighbourY] == 1 && TileEmpty(neighbourX, neighbourY))
                        {

                            SelectAndSpawnWallPrefab(neighbourX, neighbourY);
                            Debug.Log("Clonecount: " + clones.Count);
                        }
                        else if(map[neighbourX, neighbourY] == 2 && TileEmpty(neighbourX, neighbourY))
                        {
                            GameObject caveUnbreakableWallClone = Instantiate(caveUnbreakableWall, CoordToWorldPoint(new Coord(neighbourX, neighbourY)), Quaternion.identity);
                            clones.Add(caveUnbreakableWallClone);
                            Debug.Log("Clonecount: " + clones.Count);
                        }
                    }
                }
            }
        }
    }

    bool TileEmpty(int x, int y)
    {
        GameObject[] caveWallTiles = GameObject.FindGameObjectsWithTag("CaveWall");
        GameObject[] caveUnbreakableWallTiles = GameObject.FindGameObjectsWithTag("CaveUnbreakableWall");

        List<GameObject> tileStack = new List<GameObject>();

        foreach (GameObject tile in caveWallTiles)
        {
            if (tile.transform.position.x == CoordToWorldPoint(new Coord(x, y)).x && tile.transform.position.y == CoordToWorldPoint(new Coord(x, y)).y)
            {
                tileStack.Add(tile.gameObject);
            }
        }
        foreach (GameObject tile in caveUnbreakableWallTiles)
        {
            if (tile.transform.position.x == CoordToWorldPoint(new Coord(x, y)).x && tile.transform.position.y == CoordToWorldPoint(new Coord(x, y)).y)
            {
                tileStack.Add(tile.gameObject);
            }
        }
        if (tileStack.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        if (!(map[neighbourX, neighbourY] == 0))
                            wallCount++;
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }
    
    struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }

    class Room : IComparable<Room>
    {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;

        public int roomSize;

        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        public Room()
        {}

        public Room(List<Coord> roomTiles, int[,] map)
        {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();

            edgeTiles = new List<Coord>();

            foreach(Coord tile in tiles)
            {
                for(int x = tile.tileX - 1; x < tile.tileX + 1; x++)
                {
                    for (int y = tile.tileY - 1; y < tile.tileY + 1; y++)
                    {
                        if(x == tile.tileX || y == tile.tileY)
                        {
                            if(map[x, y] == 1)
                            {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public void SetAccessibleFromMainRoom()
        {
            if (!isAccessibleFromMainRoom)
            {
                isAccessibleFromMainRoom = true;
                foreach(Room connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public static void ConnectRooms(Room roomA, Room roomB)
        {
            if (roomA.isAccessibleFromMainRoom)
            {
                roomB.SetAccessibleFromMainRoom();
            }
            else if(roomB.isAccessibleFromMainRoom)
            {
                roomA.SetAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool IsConnected(Room otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }

        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }

    }

    void SelectAndSpawnWallPrefab(int x, int y)
    {
        int selector = UnityEngine.Random.Range(0, 4);
        switch (selector)
        {
            case 0:
                GameObject wall1 = Instantiate(caveWallVariant1, CoordToWorldPoint(new Coord(x, y)), Quaternion.identity);
                clones.Add(wall1);
                break;
            case 1:
                GameObject wall2 = Instantiate(caveWallVariant2, CoordToWorldPoint(new Coord(x, y)), Quaternion.identity);
                clones.Add(wall2);
                break;
            case 2:
                GameObject wall3 = Instantiate(caveWallVariant3, CoordToWorldPoint(new Coord(x, y)), Quaternion.identity);
                clones.Add(wall3);
                break;
            case 3:
                GameObject wall4 = Instantiate(caveWallVariant4, CoordToWorldPoint(new Coord(x, y)), Quaternion.identity);
                clones.Add(wall4);
                break;
        }
    }

    void SelectAndSpawnFloorPrefab(int x, int y)
    {
        int selector = UnityEngine.Random.Range(0, 4);
        switch (selector)
        {
            case 0:
                GameObject floor1 = Instantiate(caveFloorVariant1, CoordToWorldPoint(new Coord(x, y)), Quaternion.identity);
                clones.Add(floor1);
                break;
            case 1:
                GameObject floor2 = Instantiate(caveFloorVariant2, CoordToWorldPoint(new Coord(x, y)), Quaternion.identity);
                clones.Add(floor2);
                break;
            case 2:
                GameObject floor3 = Instantiate(caveFloorVariant3, CoordToWorldPoint(new Coord(x, y)), Quaternion.identity);
                clones.Add(floor3);
                break;
            case 3:
                GameObject floor4 = Instantiate(caveFloorVariant4, CoordToWorldPoint(new Coord(x, y)), Quaternion.identity);
                clones.Add(floor4);
                break;
        }
    }
}

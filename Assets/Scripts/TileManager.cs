using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public GameObject[] bottomTilePrefabs;
    public GameObject[] deadlyObstaclePrefabs;
    public GameObject[] cloudPrefabs;
    public GameObject[] pterodactylPrefabs;

    private Transform cameraTransform;

    private List<GameObject> activeBottomTiles = new List<GameObject>();
    private List<GameObject> activeTiles = new List<GameObject>();
    private List<GameObject> activeDeadlyObstacles = new List<GameObject>();
    private List<GameObject> activeClouds = new List<GameObject>();
    private List<GameObject> activePterodactyls = new List<GameObject>();

    enum PrefabType
    {
        GROUND,
        DEADLY,
        BOTTOM,
        CLOUD,
        PTERODACTYL
    };

    private Vector2 lastCactusPosition = new Vector2();
    
    private float spawnX = -10f;
    private float safeZone = 12f;
    private float tileLength;
    private float tileHeight;
    
    private float cactusChance = 20f;
    private float cloudChance = 10f;
    private float pterodactylChance = 5f;
    
    private float cloudLowerMargin = 2f;
    private float cloudUpperMargin = 7.7f;
    private float pteroSpeed = 5f;

    private int amnTilesOnScreen = 15;
    private int depth = 5;
    private int cactusBreak = 5;

    private void Start()
    {
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;
        tileLength = tilePrefabs[0].gameObject.GetComponent<BoxCollider2D>().size.x *
                     tilePrefabs[0].gameObject.transform.localScale.x;
        tileHeight = tilePrefabs[0].gameObject.GetComponent<BoxCollider2D>().size.y *
                     tilePrefabs[0].gameObject.transform.localScale.y;
        
        SpawnForTheFirstTime();
    }

    private void Update()
    {
        WorldGenerationHandler();
    }

    void SpawnTile(bool firstTime = false)
    {
        //this method spawns ground tiles and has a chance to spawn a deadly obstacle
        int spawnChance = Random.Range(0, 100);

        GameObject tileObject;
        tileObject = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
        tileObject.transform.SetParent(transform);

        tileObject.transform.position = Vector2.right * spawnX;
        
        SpawnBottomTiles();
        
        spawnX += tileLength;

        activeTiles.Add(tileObject);

        if (!firstTime)
        {
            if (spawnChance < cactusChance)
            {
                SpawnDeadlyObstacle(tileObject.transform.position);
            }

            if (spawnChance < cloudChance)
            {
                SpawnCloud();
            }
            
            if(spawnChance < pterodactylChance)
                SpawnPterodactyl();
        }
    }
    void SpawnBottomTiles()
    {
        for (int i = 1; i <= depth; i++)
        {
            GameObject bottomTileObject;
            bottomTileObject = Instantiate(bottomTilePrefabs[RandomPrefabIndex(PrefabType.BOTTOM)]) as GameObject;
            bottomTileObject.transform.SetParent(transform);

            bottomTileObject.transform.position = new Vector2(spawnX, -tileHeight * 2 * i);
            activeBottomTiles.Add(bottomTileObject);   
        }
    }
    void SpawnDeadlyObstacle(Vector3 groundPosition)
    {
        if (cameraTransform.position.x > 15f &&
            lastCactusPosition.x < groundPosition.x - cactusBreak * tileLength)
        {
            GameObject obstacleObject;
            obstacleObject = Instantiate(deadlyObstaclePrefabs[RandomPrefabIndex(PrefabType.DEADLY)]) as GameObject;
            obstacleObject.transform.SetParent(transform);
        
            obstacleObject.transform.position = new Vector2(groundPosition.x,
                groundPosition.y + tileHeight);

            activeDeadlyObstacles.Add(obstacleObject);
            lastCactusPosition = obstacleObject.transform.position;
        }
    }
    
    void SpawnCloud()
    {
        GameObject cloudObject;
        cloudObject = Instantiate(cloudPrefabs[RandomPrefabIndex(PrefabType.CLOUD)]) as GameObject;
        cloudObject.transform.SetParent(transform);

        float yPosition = Random.Range(cloudLowerMargin, cloudUpperMargin);

        cloudObject.transform.position = new Vector2(cameraTransform.position.x + 20f,
            yPosition);
        activeClouds.Add(cloudObject);
    }
    void SpawnPterodactyl()
    {
        GameObject pteroObject;
        pteroObject = Instantiate(pterodactylPrefabs[RandomPrefabIndex(PrefabType.PTERODACTYL)]) as GameObject;
        pteroObject.transform.SetParent(transform);

        float yPosition = Random.Range(cloudLowerMargin, cloudUpperMargin);

        pteroObject.transform.position = new Vector2(cameraTransform.position.x + 20f,
            yPosition);
        activePterodactyls.Add(pteroObject);
        pteroObject.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-pteroSpeed, 0);
    }

    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);

        for (int i = 0; i < depth; i++)
        {
            Destroy(activeBottomTiles[0]);
            activeBottomTiles.RemoveAt(0);   
        }
    }
    void DeleteObstacle()
    {
        Destroy(activeDeadlyObstacles[0]);
        activeDeadlyObstacles.RemoveAt(0);
    }

    void DeleteCloud()
    {
        Destroy(activeClouds[0]);
        activeClouds.RemoveAt(0);
    }
    void DeletePterodactyl()
    {
        Destroy(activePterodactyls[0]);
        activePterodactyls.RemoveAt(0);
    }

    private int RandomPrefabIndex(PrefabType type = PrefabType.GROUND)
    {
        int randomNumber = 0;
        
        switch (type)
        {
            case PrefabType.GROUND:
                randomNumber = Random.Range(0, tilePrefabs.Length);
                break;
            case PrefabType.DEADLY:
                randomNumber = Random.Range(0, deadlyObstaclePrefabs.Length);
                break;
            case PrefabType.BOTTOM:
                randomNumber = Random.Range(0, bottomTilePrefabs.Length);
                break;
            case PrefabType.CLOUD:
                randomNumber = Random.Range(0, cloudPrefabs.Length);
                break;
            case PrefabType.PTERODACTYL:
                randomNumber = Random.Range(0, pterodactylPrefabs.Length);
                break;
        }

        return randomNumber;
    }

    void SpawnForTheFirstTime()
    {
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            SpawnTile(true);
        }
    }

    void WorldGenerationHandler()
    {
        // this function spawns and deletes tiles as the player moves
        if (cameraTransform.position.x - safeZone> spawnX - amnTilesOnScreen * tileLength)
        {
            SpawnTile();
            DeleteTile();
        }

        if (activeDeadlyObstacles.Count > 10)
        {
            DeleteObstacle();
        }
        
        if(activeClouds.Count > 15)
            DeleteCloud();
        
        if(activePterodactyls.Count > 5)
            DeletePterodactyl();
    }
}

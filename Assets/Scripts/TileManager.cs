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

    private Transform playerTransform;

    private List<GameObject> activeBottomTiles = new List<GameObject>();
    private List<GameObject> activeTiles = new List<GameObject>();
    private List<GameObject> activeDeadlyObstacles = new List<GameObject>();

    enum PrefabType
    {
        GROUND,
        DEADLY,
        BOTTOM,
        CLOUD
    };
    
    private float spawnX = -10f;
    private float safeZone = 12f;
    private float tileLength;
    private float tileHeight;
    private float cactusChance = 20f;
    
    private int amnTilesOnScreen = 15;
    private int depth = 5;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
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

    void SpawnTile(int prefabIndex = 0)
    {
        //this method spawns ground tiles and has a chance to spawn a deadly obstacle
        int deadlyObstacleChance;
        deadlyObstacleChance = Random.Range(0, 100);

        GameObject tileObject;
        tileObject = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
        tileObject.transform.SetParent(transform);

        tileObject.transform.position = Vector2.right * spawnX;
        
        SpawnBottomTiles();
        
        spawnX += tileLength;

        activeTiles.Add(tileObject);
        
        if (deadlyObstacleChance < cactusChance)
        {
            SpawnDeadlyObstacle(tileObject.transform.position);
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
        if (playerTransform.position.x > 15f)
        {
            GameObject obstacleObject;
            obstacleObject = Instantiate(deadlyObstaclePrefabs[RandomPrefabIndex(PrefabType.DEADLY)]) as GameObject;
            obstacleObject.transform.SetParent(transform);
        
            obstacleObject.transform.position = new Vector2(groundPosition.x,
                groundPosition.y + tileHeight);

            activeDeadlyObstacles.Add(obstacleObject);
        }
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
        }

        return randomNumber;
    }

    void SpawnForTheFirstTime()
    {
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    void WorldGenerationHandler()
    {
        // this function spawns and deletes tiles as the player moves
        if (playerTransform.position.x - safeZone> spawnX - amnTilesOnScreen * tileLength)
        {
            SpawnTile();
            DeleteTile();
        }

        if (activeDeadlyObstacles.Count > 10)
        {
            DeleteObstacle();
        }
    }
}

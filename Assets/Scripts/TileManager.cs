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
    public GameObject[] deadlyObstaclePrefabs;

    private Transform playerTransform;

    private List<GameObject> activeTiles = new List<GameObject>();
    private List<GameObject> activeDeadlyObstacles = new List<GameObject>();

    enum PrefabType
    {
        GROUND,
        DEADLY
    };
    
    private float spawnX = -10f;
    private float safeZone = 12f;
    private float tileLength;
    private float tileHeight;
    private float cactusChance = 10f;
    
    private int amnTilesOnScreen = 15;

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
        spawnX += tileLength;

        activeTiles.Add(tileObject);
        
        if (deadlyObstacleChance < cactusChance)
        {
            SpawnDeadlyObstacle(tileObject.transform.position);
        }
        
    }
    void SpawnDeadlyObstacle(Vector3 groundPosition)
    {
        GameObject obstacleObject;
        obstacleObject = Instantiate(deadlyObstaclePrefabs[RandomPrefabIndex(PrefabType.DEADLY)]) as GameObject;
        obstacleObject.transform.SetParent(transform);
        
        obstacleObject.transform.position = new Vector2(groundPosition.x,
            groundPosition.y + tileHeight);

        activeDeadlyObstacles.Add(obstacleObject);
    }

    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    private Transform playerTransform;

    private List<GameObject> activeTiles = new List<GameObject>();
    
    private float spawnX = -10f;
    private float safeZone = 12f;
    private float tileLength;
    
    private int amnTilesOnScreen = 15;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        tileLength = tilePrefabs[0].gameObject.GetComponent<BoxCollider2D>().size.x *
                     tilePrefabs[0].gameObject.transform.localScale.x;

        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    private void Update()
    {
        if (playerTransform.position.x - safeZone> spawnX - amnTilesOnScreen * tileLength)
        {
            SpawnTile();
            DeleteTile();
        }
    }

    void SpawnTile(int prefabIndex = 0)
    {
        GameObject tileObject;
        tileObject = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
        tileObject.transform.SetParent(transform);
        
        tileObject.transform.position = Vector3.right * spawnX;
        spawnX += tileLength;
        
        activeTiles.Add(tileObject);
    }

    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    private int RandomPrefabIndex()
    {
        int randomNumber = Random.Range(0, tilePrefabs.Length);

        return randomNumber;
    }
}

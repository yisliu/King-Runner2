using UnityEngine;
using System.Collections.Generic;

public class chunky : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject fencePrefab;
    [SerializeField] GameObject applePrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float appleSpawnChance = 0.3f;
    [SerializeField] float coinSpawnChance = 0.5f;
    [SerializeField] float coinApartDist = 2f;
    [SerializeField] float[] lanes = { -2.5f, 0f, 2.5f};
    LevelCooker levelGenerator;
    scoreManager scoreManager;
    
    
    List<int> availableLanes = new List<int>{0, 1, 2};

    void Start()
    {
        SpawnFenchs();
        spawnApple();
        spawnCoin();
    }

    public void init(LevelCooker levelGenerator, scoreManager scoreManager)
    {
        this.levelGenerator = levelGenerator;
        this.scoreManager = scoreManager;
        
    }

    void SpawnFenchs()
    {
        int fencesSpawn = Random.Range(0, lanes.Length);
        for (int i = 0; i < fencesSpawn; i++)
        {
            if(availableLanes.Count <= 0) break;
            int selectedLane = spawnerStuff();
            Vector3 spawnPos = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, spawnPos, Quaternion.identity, this.transform);
        }
    }
    
    void spawnApple()
    {
        if (Random.value > appleSpawnChance||availableLanes.Count <= 0) return;
        
        int selectedLane = spawnerStuff();
        
        Vector3 spawnPos = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
        AppleUp newApple = Instantiate(applePrefab, spawnPos, Quaternion.identity, this.transform).GetComponent<AppleUp>();
        newApple.init(levelGenerator);
    }
    
    void spawnCoin()
    {
        if (Random.value > coinSpawnChance||availableLanes.Count <= 0) return;
        
        int selectedLane = spawnerStuff();
        int coinMax = 6;
        int coinsNumber = Random.Range(1, coinMax);
        float topOfChunkZ = transform.position.z + (coinApartDist * 2f);
        
        for (int i = 0; i < coinsNumber; i++)
        {
            float spawnPosZ = topOfChunkZ - (i*coinApartDist);
            Vector3 spawnPos = new Vector3(lanes[selectedLane], transform.position.y, spawnPosZ);
            CoinUp newCoin = Instantiate(coinPrefab, spawnPos, Quaternion.identity, this.transform).GetComponent<CoinUp>();
            newCoin.init(scoreManager);
        }
    }
    
    int spawnerStuff()
    {
        int randomLaneIndex = Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomLaneIndex];
        availableLanes.RemoveAt(randomLaneIndex);
        return selectedLane;
    }
}

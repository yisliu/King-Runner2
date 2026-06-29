/*
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

    [SerializeField] private float heightAdjustments = 0f;
    LevelCooker levelGenerator;
    scoreManager scoreManager;
    private int difficultyLevel = 0;

    List<int> availableLanes = new List<int>{0, 1, 2};

    public void init(LevelCooker levelGenerator, scoreManager scoreManager, int difficultyLevel = 0)
    {
        this.levelGenerator = levelGenerator;
        this.scoreManager = scoreManager;
        this.difficultyLevel = difficultyLevel;
        SpawnFenchs();
        spawnApple();
        spawnCoin();
    }

    void SpawnFenchs()
    {
        int minFences = Mathf.Min(difficultyLevel / 3, lanes.Length - 1);
        int fencesSpawn = Random.Range(minFences, lanes.Length + 1);
        for (int i = 0; i < fencesSpawn; i++)
        {
            if(availableLanes.Count <= 0) break;
            int selectedLane = spawnerStuff();
            Vector3 spawnPos = new Vector3(lanes[selectedLane], transform.position.y + heightAdjustments, transform.position.z);
            Instantiate(fencePrefab, spawnPos, Quaternion.identity, this.transform);
        }
    }
    
    void spawnApple()
    {
        if (Random.value > appleSpawnChance||availableLanes.Count <= 0) return;
        
        int selectedLane = spawnerStuff();
        
        Vector3 spawnPos = new Vector3(lanes[selectedLane], transform.position.y + heightAdjustments, transform.position.z);
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
            Vector3 spawnPos = new Vector3(lanes[selectedLane], transform.position.y + heightAdjustments, spawnPosZ);
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
*/
/*
using UnityEngine;
using System.Collections.Generic;

public class chunky : MonoBehaviour
{
    [SerializeField] GameObject fencePrefab;
    [SerializeField] GameObject applePrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float appleSpawnChance = 0.3f;
    [SerializeField] float coinSpawnChance = 0.5f;
    [SerializeField] float coinApartDist = 2f;
    [SerializeField] float[] lanes = { -2.5f, 0f, 2.5f};

    [SerializeField] private float heightAdjustments = 0f;
    LevelCooker levelGenerator;
    scoreManager scoreManager;
    private int difficultyLevel = 0;

    List<int> availableLanes = new List<int>{0, 1, 2};

    public void init(LevelCooker levelGenerator, scoreManager scoreManager, int difficultyLevel = 0)
    {
        this.levelGenerator = levelGenerator;
        this.scoreManager = scoreManager;
        this.difficultyLevel = difficultyLevel;
        
        // Reset lanes list at initialization
        availableLanes = new List<int>{0, 1, 2};

        SpawnFenchs();
        spawnApple();
        spawnCoin();
    }

    void SpawnFenchs()
    {
        int minFences = Mathf.Min(difficultyLevel / 3, lanes.Length - 1);
        int fencesSpawn = Random.Range(minFences, lanes.Length + 1);
        for (int i = 0; i < fencesSpawn; i++)
        {
            if(availableLanes.Count <= 0) break;
            int selectedLane = spawnerStuff();
            
            // FIX: Create a LOCAL position relative to the chunk's pivot
            Vector3 localSpawnPos = new Vector3(lanes[selectedLane], heightAdjustments, 0f);
            
            // FIX: Instantiate using localPosition and localRotation
            GameObject fence = Instantiate(fencePrefab, transform);
            fence.transform.localPosition = localSpawnPos;
            fence.transform.localRotation = Quaternion.identity;
        }
    }
    
    void spawnApple()
    {
        if (Random.value > appleSpawnChance || availableLanes.Count <= 0) return;
        
        int selectedLane = spawnerStuff();
        
        // FIX: Create a LOCAL position relative to the chunk's pivot
        Vector3 localSpawnPos = new Vector3(lanes[selectedLane], heightAdjustments, 0f);
        
        GameObject appleGO = Instantiate(applePrefab, transform);
        appleGO.transform.localPosition = localSpawnPos;
        appleGO.transform.localRotation = Quaternion.identity;

        AppleUp newApple = appleGO.GetComponent<AppleUp>();
        if (newApple != null) newApple.init(levelGenerator);
    }
    
    void spawnCoin()
    {
        if (Random.value > coinSpawnChance || availableLanes.Count <= 0) return;
        
        int selectedLane = spawnerStuff();
        int coinMax = 6;
        int coinsNumber = Random.Range(1, coinMax);
        
        // FIX: Calculate local forward offsets (Z is local forward relative to the chunk)
        float localStartCheckZ = coinApartDist * 1f; 
        
        for (int i = 0; i < coinsNumber; i++)
        {
            float localSpawnPosZ = localStartCheckZ - (i * coinApartDist);
            Vector3 localSpawnPos = new Vector3(lanes[selectedLane], heightAdjustments, localSpawnPosZ);
            
            GameObject coinGO = Instantiate(coinPrefab, transform);
            coinGO.transform.localPosition = localSpawnPos;
            coinGO.transform.localRotation = Quaternion.identity;

            CoinUp newCoin = coinGO.GetComponent<CoinUp>();
            if (newCoin != null) newCoin.init(scoreManager);
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
*/
using UnityEngine;
using System.Collections.Generic;

public class chunky : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject fencePrefab;
    [SerializeField] GameObject applePrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject tigerPrefab;

    [Header("Spawn Settings")]
    [SerializeField] Vector3 fenceRotation = Vector3.zero;
    [SerializeField] float tigerSpawnChance = 0.2f;
    [SerializeField] float appleSpawnChance = 0.3f;
    [SerializeField] float coinSpawnChance = 0.5f;
    [SerializeField] float coinApartDist = 2f;
    [SerializeField] float[] lanes = { -2.5f, 0f, 2.5f };
    [SerializeField] private float heightAdjustments = 0f;

    [Tooltip("Small positive value lifts props above the terrain surface to avoid clipping")]
    [SerializeField] private float groundOffsetPadding = 0.05f;

    ILevelCooker levelGenerator;
    scoreManager scoreManager;
    private int difficultyLevel = 0;

    List<int> availableLanes = new List<int> { 0, 1, 2 };

    public void init(ILevelCooker levelGenerator, scoreManager scoreManager, int difficultyLevel = 0)
    {
        this.levelGenerator = levelGenerator;
        this.scoreManager = scoreManager;
        this.difficultyLevel = difficultyLevel;
        
        // Reset lanes list at initialization
        availableLanes = new List<int> { 0, 1, 2 };

        SpawnFenchs();
        spawnApple();
        spawnCoin();
        spawnTiger();
    }

    void SpawnFenchs()
    {
        int minFences = Mathf.Min(difficultyLevel / 3, lanes.Length - 1);
        int fencesSpawn = Random.Range(minFences, lanes.Length + 1);
        for (int i = 0; i < fencesSpawn; i++)
        {
            if (availableLanes.Count <= 0) break;
            int selectedLane = spawnerStuff();
            
            Vector3 localSpawnPos = new Vector3(lanes[selectedLane], heightAdjustments, 0f);
            
            // Snap this local position down to the terrain's height via LayerMask
            localSpawnPos = SnapToTerrainLocal(localSpawnPos);
            
            GameObject fence = Instantiate(fencePrefab, transform);
            fence.transform.localPosition = localSpawnPos;
            fence.transform.localRotation = Quaternion.Euler(fenceRotation);
        }
    }
    
    void spawnApple()
    {
        if (Random.value > appleSpawnChance || availableLanes.Count <= 0) return;
        
        int selectedLane = spawnerStuff();
        
        Vector3 localSpawnPos = new Vector3(lanes[selectedLane], heightAdjustments, 0f);
        
        // Snap this local position down to the terrain's height via LayerMask
        localSpawnPos = SnapToTerrainLocal(localSpawnPos);
        
        GameObject appleGO = Instantiate(applePrefab, transform);
        appleGO.transform.localPosition = localSpawnPos;
        appleGO.transform.localRotation = Quaternion.identity;

        AppleUp newApple = appleGO.GetComponent<AppleUp>();
        if (newApple != null)
            newApple.init(levelGenerator);
    }
    
    void spawnCoin()
    {
        if (Random.value > coinSpawnChance || availableLanes.Count <= 0) return;
        
        int selectedLane = spawnerStuff();
        int coinMax = 6;
        int coinsNumber = Random.Range(1, coinMax);
        
        float localStartCheckZ = coinApartDist * 1f; 
        
        for (int i = 0; i < coinsNumber; i++)
        {
            float localSpawnPosZ = localStartCheckZ - (i * coinApartDist);
            Vector3 localSpawnPos = new Vector3(lanes[selectedLane], heightAdjustments, localSpawnPosZ);
            
            // Snap each coin individually to the terrain height profile via LayerMask
            localSpawnPos = SnapToTerrainLocal(localSpawnPos);
            
            GameObject coinGO = Instantiate(coinPrefab, transform);
            coinGO.transform.localPosition = localSpawnPos;
            coinGO.transform.localRotation = Quaternion.identity;

            CoinUp newCoin = coinGO.GetComponent<CoinUp>();
            if (newCoin != null) newCoin.init(scoreManager);
        }
    }
    
    void spawnTiger()
    {
        if (tigerPrefab == null || Random.value > tigerSpawnChance) return;

        // Spawn at chunk centre — TigerObstacle offsets itself to the side at runtime
        GameObject tigerGO = Instantiate(tigerPrefab, transform);
        tigerGO.transform.localPosition = new Vector3(0f, heightAdjustments, 0f);
        tigerGO.transform.localRotation = Quaternion.identity;
    }

    int spawnerStuff()
    {
        int randomLaneIndex = Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomLaneIndex];
        availableLanes.RemoveAt(randomLaneIndex);
        return selectedLane;
    }

    Vector3 SnapToTerrainLocal(Vector3 localPos)
    {
        // 1. Convert local space coordinates to World coordinates
        Vector3 worldPos = transform.TransformPoint(localPos);
        
        // 2. Lift starting height high into the sky above the chunk
        worldPos.y += 30f; 

        // 3. Create a LayerMask that ONLY looks for the "Terrain" layer
        int terrainLayerIndex = LayerMask.NameToLayer("Terrain");
        
        // Safety check fallback: if the layer isn't created, scan for any default physics collider
        if (terrainLayerIndex == -1)
        {
            if (Physics.Raycast(worldPos, Vector3.down, out RaycastHit fallbackHit, 60f))
            {
                // FIX: Changed 'hit.point.y' to 'fallbackHit.point.y' and applied offset padding
                Vector3 snappedWorldPos = new Vector3(worldPos.x, fallbackHit.point.y + groundOffsetPadding, worldPos.z);
                return transform.InverseTransformPoint(snappedWorldPos);
            }
            return localPos;
        }

        int layerMask = 1 << terrainLayerIndex;

        // 4. Cast the ray passing ONLY our specific terrain layer mask
        if (Physics.Raycast(worldPos, Vector3.down, out RaycastHit hit, 60f, layerMask))
        {
            // Override the world Y position with the exact terrain surface height minus our offset adjustments
            Vector3 snappedWorldPos = new Vector3(worldPos.x, hit.point.y + groundOffsetPadding, worldPos.z);
            
            // Convert back to local space so items stick firmly to their chunk orientations
            return transform.InverseTransformPoint(snappedWorldPos);
        }

        // Fallback if the laser missed the terrain structure completely
        return localPos;
    }
}
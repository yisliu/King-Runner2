using UnityEngine;
using System.Collections.Generic;

public class IslandPropSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask groundLayer;

    [Header("Spawn Area")]
    [SerializeField] private float spawnWidth = 4f;
    [SerializeField] private float spawnAheadMin = 20f;
    [SerializeField] private float spawnAheadMax = 40f;
    [SerializeField] private float raycastHeight = 60f;
    [SerializeField] private bool flipSpawnDirection = false;

    [Header("Timing")]
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private float despawnDistance = 20f;

    [Header("Props")]
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject applePrefab;

    [Header("Spawn Chances")]
    [Range(0f, 1f)] [SerializeField] private float obstacleChance = 0.5f;
    [Range(0f, 1f)] [SerializeField] private float coinChance = 0.35f;
    [Range(0f, 1f)] [SerializeField] private float appleChance = 0.15f;

    private List<GameObject> spawnedProps = new List<GameObject>();
    private scoreManager scoreManager;
    private LevelCooker levelCooker;
    private float timer;

    void Start()
    {
        scoreManager = FindFirstObjectByType<scoreManager>();
        levelCooker = FindFirstObjectByType<LevelCooker>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawnProp();
        }
        DespawnBehindPlayer();
    }

    void TrySpawnProp()
    {
        float ahead = Random.Range(spawnAheadMin, spawnAheadMax);
        float side = Random.Range(-spawnWidth, spawnWidth);

        Vector3 forward = flipSpawnDirection ? -player.forward : player.forward;
        Vector3 right = flipSpawnDirection ? -player.right : player.right;
        Vector3 origin = player.position
            + forward * ahead
            + right * side;
        origin.y += raycastHeight;

        // Only spawn if the raycast hits the island surface
        if (!Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastHeight * 2f, groundLayer))
            return;

        GameObject prefab = PickPrefab();
        if (prefab == null) return;

        GameObject prop = Instantiate(prefab, hit.point, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
        InitProp(prop);
        spawnedProps.Add(prop);
    }

    void InitProp(GameObject prop)
    {
        CoinUp coin = prop.GetComponent<CoinUp>();
        if (coin != null)
        {
            coin.init(scoreManager);
            return;
        }

        AppleUp apple = prop.GetComponent<AppleUp>();
        if (apple != null && levelCooker != null)
            apple.init(levelCooker);
    }

    GameObject PickPrefab()
    {
        float roll = Random.value;

        if (roll < obstacleChance && obstaclePrefabs.Length > 0)
            return obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        roll -= obstacleChance;

        if (roll < coinChance && coinPrefab != null)
            return coinPrefab;
        roll -= coinChance;

        if (roll < appleChance && applePrefab != null)
            return applePrefab;

        return null;
    }

    void DespawnBehindPlayer()
    {
        for (int i = spawnedProps.Count - 1; i >= 0; i--)
        {
            if (spawnedProps[i] == null)
            {
                spawnedProps.RemoveAt(i);
                continue;
            }

            Vector3 toProp = spawnedProps[i].transform.position - player.position;
            if (Vector3.Dot(toProp, player.forward) < -despawnDistance)
            {
                Destroy(spawnedProps[i]);
                spawnedProps.RemoveAt(i);
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class boxMaker : MonoBehaviour
{
    [SerializeField] GameObject[] boxPrefab;
    [SerializeField] float boxSpawnTime = 1f;
    [SerializeField] private float spawnWidth = 4f;
    [SerializeField] private float spawnAheadDistance = 30f;
    [SerializeField] private float spawnHeight = 0f;
    [SerializeField] private float minBoxSpawnTime = 0.3f;
    [SerializeField] private float spawnTimeDecreasePerLevel = 0.15f;

    void Start()
    {
        StartCoroutine(SpawnObstacleRoutine());
    }

    public void SetDifficulty(int level)
    {
        boxSpawnTime = Mathf.Max(minBoxSpawnTime, boxSpawnTime - spawnTimeDecreasePerLevel);
    }

    IEnumerator SpawnObstacleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(boxSpawnTime);
            GameObject obsPrefab = boxPrefab[Random.Range(0, boxPrefab.Length)];
            float spawnZ = Camera.main.transform.position.z + spawnAheadDistance;
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), spawnHeight, spawnZ);
            Instantiate(obsPrefab, spawnPosition, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
        }
    }
}

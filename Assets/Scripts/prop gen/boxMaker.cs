using UnityEngine;
using System.Collections;

public class boxMaker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject[] boxPrefab;
    [SerializeField] float boxSpawnTime = 1f;
    [SerializeField] Transform obsParent;
    [SerializeField] private float spawnWidth = 4f;
    void Start()
    {
        StartCoroutine(SpawnObstacleRoutine());
    }

    // Update is called once per frame
    IEnumerator SpawnObstacleRoutine()
    {
        
        while (true)
        {   
            GameObject obsPrefab = boxPrefab[Random.Range(0, boxPrefab.Length)];
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y,
                transform.position.z);
            yield return new WaitForSeconds(boxSpawnTime);
            Instantiate(obsPrefab, spawnPosition, Random.rotation, obsParent);
        }
    }
}

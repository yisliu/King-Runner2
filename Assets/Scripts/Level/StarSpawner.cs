using UnityEngine;
using System.Collections;

public class ShootingStarSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject shootingStarPrefab; 
    [SerializeField] private Transform spawnParent;  
    
    [SerializeField] private float minSpawnDelay = 0.5f; 
    [SerializeField] private float maxSpawnDelay = 2.0f;

    [Header("Size Settings")]
    [SerializeField] private float minScale = 0.4f;
    [SerializeField] private float maxScale = 1.5f;

    private void Start()
    {
        if (spawnParent == null) spawnParent = transform;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

            SpawnStar();
        }
    }

    private void SpawnStar()
    {
        if (shootingStarPrefab == null) return;

        GameObject newStar = Instantiate(shootingStarPrefab, spawnParent);

        float randomScale = Random.Range(minScale, maxScale);
        newStar.GetComponent<RectTransform>().localScale = new Vector3(randomScale, randomScale, 1f);
    }
}
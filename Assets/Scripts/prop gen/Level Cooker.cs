using UnityEngine;
using System.Collections.Generic;

public class LevelCooker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is create
    [Header("References")]
    [SerializeField] private playerCamara CamaraPlayer;
    [SerializeField] GameObject[] chunkPrefab;
    [SerializeField] GameObject checkpointChunkPrefab;

    [SerializeField] Transform ChunkParent;
    [SerializeField] scoreManager scoreManager;


    [Header("Level Settings")]

    [SerializeField] int startingChunksAmount = 12;
    [Tooltip("Do not change chunk length value unless chunk prefab size reflects change")]
    [SerializeField] float chunkLength = 10f;
    //[SerializeField] int chunkCount = 8;

    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float minMoveSpeed = 2f;
    [SerializeField] float maxMoveSpeed = 20f;

    
    [SerializeField] float minGravityZ = -22f;

    [SerializeField] float maxGravityZ = -2f;

    private int chunksOut = 8;

    List<GameObject> chunks = new List<GameObject>();
    void Start()
    {
        spawnStartingChunks();
    }

    void Update()
    {
        moveChunks();
    }

    public void changeChunkSpeed(float speedNum)
    {
        float newMoveSpeed = moveSpeed + speedNum;
        newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed);

        if (newMoveSpeed!=moveSpeed)
        {
            moveSpeed = newMoveSpeed;
            
            float newGavityZ = Physics.gravity.z - speedNum;
            newGavityZ = Mathf.Clamp(newGavityZ, minGravityZ, maxGravityZ);
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGavityZ);
            if (CamaraPlayer != null)
            {
                CamaraPlayer.changeCameraFOV(speedNum);
            }
        }

    }
    void spawnStartingChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {

            spawnChunk();
        }
    }

    void spawnChunk()
    {
        float spawnPositionZ = returnChunk();
        GameObject chunkToSpawn = whichChunkToSpawn();
        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
        
        GameObject newChunkGo = Instantiate(chunkToSpawn, chunkSpawnPos, Quaternion.identity, ChunkParent);
        chunks.Add(newChunkGo);
        chunky newChunky = newChunkGo.GetComponent<chunky>();
        newChunky.init(this, scoreManager);
        chunksOut++;
    }

    private GameObject whichChunkToSpawn()
    {
        GameObject chunkToSpawn;
        if (chunksOut != 8 && chunksOut % 8 == 0)
        {
            chunkToSpawn = checkpointChunkPrefab;
        }
        else
        {
            chunkToSpawn = chunkPrefab[Random.Range(0, chunkPrefab.Length)];
        }

        return chunkToSpawn;
    }
    float returnChunk()
    {
        float spawnPositionZ;
        if (chunks.Count == 0)
        {
            spawnPositionZ = transform.position.z;
        }
        else
        {
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;
        }
        return spawnPositionZ;
    }

    void moveChunks()
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            GameObject chunk = chunks[i];
            chunk.transform.Translate(-transform.forward*(moveSpeed*Time.deltaTime));
            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
            {
                chunks.Remove(chunk);
                Destroy(chunk);
                spawnChunk();

            }
        }
    }

    // Update is called once per frame

}

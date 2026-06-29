/*
using UnityEngine;
using System.Collections.Generic;

public class IslandLevelCooker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private IslandPath path;
    [SerializeField] private scoreManager scoreManager;
    [SerializeField] private playerCamara cameraPlayer;

    [Header("Chunks")]
    [SerializeField] private GameObject[] chunkPrefabs;
    [SerializeField] private GameObject checkpointChunkPrefab;
    [SerializeField] private int startingChunksAmount = 10;
    [SerializeField] private float chunkLength = 10f;

    [Header("Speed")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float minMoveSpeed = 2f;
    [SerializeField] private float maxMoveSpeed = 20f;

    [Header("Difficulty")]
    [SerializeField] private float difficultyRampInterval = 15f;
    [SerializeField] private float speedIncreasePerRamp = 1f;
    [SerializeField] private int maxDifficultyLevel = 5;

    public float MoveSpeed => moveSpeed;
    public int DifficultyLevel => difficultyLevel;

    private class PathChunk
    {
        public GameObject go;
        public float distance;
    }

    private List<PathChunk> chunks = new List<PathChunk>();
    private int chunksSpawned = 0;
    private int difficultyLevel = 0;
    private float difficultyTimer = 0f;

    void Start()
    {
        for (int i = 0; i < startingChunksAmount; i++)
            SpawnChunk();
    }

    void Update()
    {
        MoveChunks();
        TickDifficulty();
    }

    void SpawnChunk()
    {
        // Place new chunk ahead of the last one
        float spawnDist = chunks.Count == 0
            ? path.TotalLength
            : chunks[chunks.Count - 1].distance + chunkLength;

        Vector3 pos = path.GetPositionAtDistance(spawnDist);
        Quaternion rot = path.GetRotationAtDistance(spawnDist);
        GameObject prefab = SelectPrefab();

        GameObject go = Instantiate(prefab, pos, rot);
        chunksSpawned++;

        chunky c = go.GetComponent<chunky>();
        if (c != null)
            c.init(null, scoreManager, difficultyLevel);

        chunks.Add(new PathChunk { go = go, distance = spawnDist });
    }

    void MoveChunks()
    {
        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            PathChunk chunk = chunks[i];
            if (chunk.go == null) { chunks.RemoveAt(i); continue; }

            chunk.distance -= moveSpeed * Time.deltaTime;

            // Only update transform while chunk is still ahead of the player
            if (chunk.distance >= 0f)
            {
                chunk.go.transform.position = path.GetPositionAtDistance(chunk.distance);
                chunk.go.transform.rotation = path.GetRotationAtDistance(chunk.distance);
            }

            // Recycle once fully past the player start
            if (chunk.distance < -chunkLength)
            {
                Destroy(chunk.go);
                chunks.RemoveAt(i);
                SpawnChunk();
            }
        }
    }

    GameObject SelectPrefab()
    {
        if (checkpointChunkPrefab != null && chunksSpawned > 0 && chunksSpawned % 8 == 0)
            return checkpointChunkPrefab;
        return chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
    }

    void TickDifficulty()
    {
        if (difficultyLevel >= maxDifficultyLevel) return;
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer < difficultyRampInterval) return;

        difficultyTimer = 0f;
        difficultyLevel++;
        ChangeSpeed(speedIncreasePerRamp);
    }

    public void ChangeSpeed(float amount)
    {
        moveSpeed = Mathf.Clamp(moveSpeed + amount, minMoveSpeed, maxMoveSpeed);
        if (cameraPlayer != null) cameraPlayer.changeCameraFOV(amount);
    }
}
*/
/*

using UnityEngine;
using System.Collections.Generic;

public class IslandLevelCooker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private IslandPath path;
    [SerializeField] private scoreManager scoreManager;
    [SerializeField] private PlayerPhysicalProgress player; // Reference to our moving player

    [Header("Chunks")]
    [SerializeField] private GameObject[] chunkPrefabs;
    [SerializeField] private GameObject checkpointChunkPrefab;
    [SerializeField] private int startingChunksAmount = 10;
    [SerializeField] private float chunkLength = 10f;
    [SerializeField] private float lookAheadDistance = 60f; // How far ahead to generate

    private class PathChunk
    {
        public GameObject go;
        public float distance;
    }

    private List<PathChunk> chunks = new List<PathChunk>();
    private int chunksSpawned = 0;
    private float nextSpawnDistance = 0f;

    void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerPhysicalProgress>();

        // Build initial track starting from 0 distance
        nextSpawnDistance = 0f;
        for (int i = 0; i < startingChunksAmount; i++)
        {
            SpawnChunkAt(nextSpawnDistance);
            nextSpawnDistance += chunkLength;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Dynamic Generation: If the player gets close to the end of the built track, spawn more
        while (nextSpawnDistance < player.CurrentDistance + lookAheadDistance)
        {
            SpawnChunkAt(nextSpawnDistance);
            nextSpawnDistance += chunkLength;
        }

        // Clean Up: Delete chunks that the player has physically passed
        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            // If chunk is further behind than one full chunk length
            if (chunks[i].distance < player.CurrentDistance - chunkLength)
            {
                Destroy(chunks[i].go);
                chunks.RemoveAt(i);
            }
        }
    }

    void SpawnChunkAt(float spawnDist)
    {
        Vector3 pos = path.GetPositionAtDistance(spawnDist);
        Quaternion rot = path.GetRotationAtDistance(spawnDist);
        GameObject prefab = SelectPrefab();

        GameObject go = Instantiate(prefab, pos, rot);
        chunksSpawned++;

        chunky c = go.GetComponent<chunky>();
        if (c != null)
            c.init(null, scoreManager, 0); // Obstacles spawn locally, perfectly aligned!

        chunks.Add(new PathChunk { go = go, distance = spawnDist });
    }

    GameObject SelectPrefab()
    {
        if (checkpointChunkPrefab != null && chunksSpawned > 0 && chunksSpawned % 8 == 0)
            return checkpointChunkPrefab;
        return chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
    }

    public void ChangeSpeed(float amount)
    {
        if (player != null)
        {
            player.AlterSpeed(amount);
        }
    }
}
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class IslandLevelCooker : MonoBehaviour, ILevelCooker
{
    [Header("References")]
    [SerializeField] private IslandPath path;
    [SerializeField] private scoreManager scoreManager;
    [SerializeField] private PlayerPhysicalProgress player;
    private IslandPlayerMover islandMover;

    [Header("Speed Up Flash")]
    [SerializeField] private TMP_Text speedUpText;

    [Header("Level Config (optional — overrides fields below if assigned)")]
    [SerializeField] private LevelConfig levelConfig;

    [Header("Chunks")]
    [SerializeField] private GameObject[] chunkPrefabs;
    [SerializeField] private GameObject checkpointChunkPrefab;
    [SerializeField] private int startingChunksAmount = 10;
    [SerializeField] private float chunkLength = 10f;
    [SerializeField] private float lookAheadDistance = 60f;

    [SerializeField] private float despawnBuffer = 30f;

    [Header("Difficulty Scaling (Optional)")]
    [SerializeField] private float difficultyRampInterval = 15f;
    [SerializeField] private int maxDifficultyLevel = 5;

    private class PathChunk
    {
        public GameObject go;
        public float distance;
    }

    private List<PathChunk> chunks = new List<PathChunk>();
    private int chunksSpawned = 0;
    private float nextSpawnDistance = 0f;
    private int difficultyLevel = 0;
    private float difficultyTimer = 0f;
    private float timeCounter = 0f;

    void Start()
    {
        if (levelConfig != null)
        {
            if (levelConfig.chunkPrefabs != null && levelConfig.chunkPrefabs.Length > 0)
                chunkPrefabs = levelConfig.chunkPrefabs;
            if (levelConfig.checkpointChunkPrefab != null)
                checkpointChunkPrefab = levelConfig.checkpointChunkPrefab;
            startingChunksAmount = levelConfig.startingChunksAmount;
            chunkLength = levelConfig.chunkLength;
            difficultyRampInterval = levelConfig.difficultyRampInterval;
            maxDifficultyLevel = levelConfig.maxDifficultyLevel;
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<PlayerPhysicalProgress>();
                islandMover = playerObj.GetComponent<IslandPlayerMover>();
            }
            if (player == null && islandMover == null)
            {
                player = FindFirstObjectByType<PlayerPhysicalProgress>();
                islandMover = FindFirstObjectByType<IslandPlayerMover>();
            }
        }

        nextSpawnDistance = 0f;
        for (int i = 0; i < startingChunksAmount; i++)
        {
            SpawnChunkAt(nextSpawnDistance);
            nextSpawnDistance += chunkLength;
        }
    }

    float PlayerDistance => player != null ? player.CurrentDistance
                          : islandMover != null ? islandMover.CurrentDistance : 0f;
    bool HasPlayer => player != null || islandMover != null;

    void Update()
    {
        if (!HasPlayer) return;

        timeCounter += Time.deltaTime;
        TickDifficulty();

        float dist = PlayerDistance;

        while (nextSpawnDistance < dist + lookAheadDistance)
        {
            SpawnChunkAt(nextSpawnDistance);
            nextSpawnDistance += chunkLength;
        }

        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            if (chunks[i].distance < dist - chunkLength - despawnBuffer)
            {
                Destroy(chunks[i].go);
                chunks.RemoveAt(i);
            }
        }
    }

    void SpawnChunkAt(float spawnDist)
    {
        Vector3 pos = path.GetPositionAtDistance(spawnDist);
        Quaternion rot = path.GetRotationAtDistance(spawnDist);
        GameObject prefab = SelectPrefab();

        GameObject go = Instantiate(prefab, pos, rot);
        chunksSpawned++;

        chunky c = go.GetComponent<chunky>();
        if (c != null)
        {
            // FIX 1: Pass 'this' instead of 'null' so the apple knows who we are.
            // FIX 2: Pass 'difficultyLevel' instead of '0' so your fences scale up.
            c.init(this, scoreManager, difficultyLevel); 
        }

        chunks.Add(new PathChunk { go = go, distance = spawnDist });
    }

    GameObject SelectPrefab()
    {
        if (checkpointChunkPrefab != null && chunksSpawned > 0 && chunksSpawned % 8 == 0)
            return checkpointChunkPrefab;
        return chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
    }

    void TickDifficulty()
    {
        if (difficultyLevel >= maxDifficultyLevel) return;
        
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= difficultyRampInterval)
        {
            difficultyTimer = 0f;
            difficultyLevel++;
        }
    }

    public void ChangeSpeed(float amount)
    {
        player?.AlterSpeed(amount);
        islandMover?.AlterSpeed(amount);

        playerCamara cameraPlayer = FindFirstObjectByType<playerCamara>();
        if (cameraPlayer != null)
            cameraPlayer.changeCameraFOV(amount);

        if (amount > 0) ShowSpeedFlash();
    }

    void ShowSpeedFlash()
    {
        if (speedUpText == null) return;
        speedUpText.text = "FASTER!";
        speedUpText.transform.localScale = Vector3.zero;
        Color c = speedUpText.color;
        speedUpText.color = new Color(c.r, c.g, c.b, 1f);
        DOTween.Kill(speedUpText.transform);
        Sequence seq = DOTween.Sequence();
        seq.Append(speedUpText.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        seq.AppendInterval(0.5f);
        seq.Append(DOTween.To(() => speedUpText.color, x => speedUpText.color = x,
            new Color(c.r, c.g, c.b, 0f), 0.3f));
    }

    public void EndRunSuccessful()
    {
        RunData.FinalScore = scoreManager != null ? scoreManager.Score : 0;
        RunData.CoinsCollected = scoreManager != null ? scoreManager.CoinCount : 0;
        RunData.CoinGoal = scoreManager != null ? scoreManager.CoinGoal : 50;
        RunData.TimeSurvived = timeCounter;
        RunData.LevelSceneName = SceneManager.GetActiveScene().name;
        RunData.IsWinState = true;

        int oldHighScore = PlayerPrefs.GetInt("BestScore", 0);
        if (RunData.FinalScore > oldHighScore)
        {
            PlayerPrefs.SetInt("BestScore", RunData.FinalScore);
            PlayerPrefs.Save();
            RunData.IsNewHighScore = true;
        }
        else
        {
            RunData.IsNewHighScore = false;
        }

        SceneManager.LoadScene("Win Screen");
    }
}
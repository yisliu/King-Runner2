/*using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelCooker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private playerCamara CamaraPlayer;
    [SerializeField] private GameObject[] chunkPrefab;
    [SerializeField] private GameObject checkpointChunkPrefab;
    [SerializeField] private Transform ChunkParent;
    [SerializeField] private scoreManager scoreManager;

    [Header("Ship Arrival")]
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private Vector3 shipSpawnPosition = new Vector3(-2.8f, 26.63f, 13f);

    [Header("Level Settings")]
    [SerializeField] private int startingChunksAmount = 12;
    [Tooltip("Do not change chunk length value unless chunk prefab size reflects change")]
    [SerializeField] private float chunkLength = 10f;

    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float minMoveSpeed = 2f;
    [SerializeField] private float maxMoveSpeed = 20f;

    [SerializeField] private float minGravityZ = -22f;
    [SerializeField] private float maxGravityZ = -2f;

    [Header("Difficulty")]
    [SerializeField] private float difficultyRampInterval = 15f;
    [SerializeField] private float speedIncreasePerRamp = 1f;
    [SerializeField] private int maxDifficultyLevel = 5;
    [SerializeField] private boxMaker boxMakerRef;

    private int difficultyLevel = 0;
    private float difficultyTimer = 0f;

    public int DifficultyLevel => difficultyLevel;
    public float MoveSpeed => moveSpeed;

    private int chunksOut = 8;
    private List<GameObject> chunks = new List<GameObject>();
    private float timeCounter;

    private void Start()
    {
        timeCounter = 0f;
        spawnStartingChunks();
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;
        moveChunks();
        tickDifficulty();
    }

    private void tickDifficulty()
    {
        if (difficultyLevel >= maxDifficultyLevel) return;
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= difficultyRampInterval)
        {
            difficultyTimer = 0f;
            difficultyLevel++;
            changeChunkSpeed(speedIncreasePerRamp);
            if (boxMakerRef != null) boxMakerRef.SetDifficulty(difficultyLevel);
        }
    }

    public void changeChunkSpeed(float speedNum)
    {
        float newMoveSpeed = Mathf.Clamp(moveSpeed + speedNum, minMoveSpeed, maxMoveSpeed);

        if (!Mathf.Approximately(newMoveSpeed, moveSpeed))
        {
            moveSpeed = newMoveSpeed;

            float newGravityZ = Mathf.Clamp(Physics.gravity.z - speedNum, minGravityZ, maxGravityZ);
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);

            if (CamaraPlayer != null)
                CamaraPlayer.changeCameraFOV(speedNum);
        }
    }

    private void spawnStartingChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
            spawnChunk();
    }

    private void spawnChunk()
    {
        float spawnPositionZ = returnChunk();
        GameObject chunkToSpawn = whichChunkToSpawn();
        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);

        GameObject newChunkGo = Instantiate(chunkToSpawn, chunkSpawnPos, Quaternion.identity, ChunkParent);
        chunks.Add(newChunkGo);

        chunky newChunky = newChunkGo.GetComponent<chunky>();
        if (newChunky != null)
            newChunky.init(this, scoreManager, difficultyLevel);

        chunksOut++;
    }

    private GameObject whichChunkToSpawn()
    {
        if (chunksOut != 8 && chunksOut % 8 == 0)
            return checkpointChunkPrefab;
        return chunkPrefab[Random.Range(0, chunkPrefab.Length)];
    }

    private float returnChunk()
    {
        if (chunks.Count == 0)
            return transform.position.z;
        return chunks[chunks.Count - 1].transform.position.z + chunkLength;
    }

    private void moveChunks()
    {
        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = chunks[i];
            if (chunk == null) continue;

            chunk.transform.Translate(-transform.forward * (moveSpeed * Time.deltaTime));

            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
            {
                chunks.RemoveAt(i);
                Destroy(chunk);
                spawnChunk();
            }
        }
    }

    private void OnEnable()
    {
        if (scoreManager != null) scoreManager.onThresholdReached += SpawnShip;
    }

    private void OnDisable()
    {
        if (scoreManager != null) scoreManager.onThresholdReached -= SpawnShip;
    }

    private void SpawnShip()
    {
        GameObject spawnShip = Instantiate(shipPrefab, shipSpawnPosition, Quaternion.identity);

        ShipArrival ship = spawnShip.GetComponent<ShipArrival>();
        if (ship != null) ship.startArrival();

        LevelTransition levelTransition = spawnShip.GetComponent<LevelTransition>();
        if (levelTransition != null)
        {
            levelTransition.UnlockTransition();
            levelTransition.SetLevelCooker(this);
        }
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
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class LevelCooker : MonoBehaviour, ILevelCooker
{
    [Header("References")]
    [SerializeField] private playerCamara CamaraPlayer;
    [SerializeField] private GameObject[] chunkPrefab;
    [SerializeField] private GameObject checkpointChunkPrefab;
    [SerializeField] private Transform ChunkParent;
    [SerializeField] private scoreManager scoreManager;

    [Header("Speed Up Flash")]
    [SerializeField] private TMP_Text speedUpText;

    [Header("Ship Arrival")]
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private Vector3 shipSpawnPosition = new Vector3(-2.8f, 26.63f, 13f);

    [Header("Level Config (optional — overrides fields below if assigned)")]
    [SerializeField] private LevelConfig levelConfig;

    [Header("Level Settings")]
    [SerializeField] private int startingChunksAmount = 12;
    [Tooltip("Do not change chunk length value unless chunk prefab size reflects change")]
    [SerializeField] private float chunkLength = 10f;

    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float minMoveSpeed = 2f;
    [SerializeField] private float maxMoveSpeed = 20f;

    [SerializeField] private float minGravityZ = -22f;
    [SerializeField] private float maxGravityZ = -2f;

    [Header("Difficulty")]
    [SerializeField] private float difficultyRampInterval = 15f;
    [SerializeField] private float speedIncreasePerRamp = 1f;
    [SerializeField] private int maxDifficultyLevel = 5;
    [SerializeField] private boxMaker boxMakerRef;

    private int difficultyLevel = 0;
    private float difficultyTimer = 0f;

    public int DifficultyLevel => difficultyLevel;
    public float MoveSpeed => moveSpeed;

    private int chunksOut = 8;
    private List<GameObject> chunks = new List<GameObject>();
    private float timeCounter;

    private void Start()
    {
        if (levelConfig != null)
        {
            if (levelConfig.chunkPrefabs != null && levelConfig.chunkPrefabs.Length > 0)
                chunkPrefab = levelConfig.chunkPrefabs;
            if (levelConfig.checkpointChunkPrefab != null)
                checkpointChunkPrefab = levelConfig.checkpointChunkPrefab;
            startingChunksAmount = levelConfig.startingChunksAmount;
            chunkLength = levelConfig.chunkLength;
            moveSpeed = levelConfig.startMoveSpeed;
            minMoveSpeed = levelConfig.minMoveSpeed;
            maxMoveSpeed = levelConfig.maxMoveSpeed;
            difficultyRampInterval = levelConfig.difficultyRampInterval;
            speedIncreasePerRamp = levelConfig.speedIncreasePerRamp;
            maxDifficultyLevel = levelConfig.maxDifficultyLevel;
        }

        timeCounter = 0f;
        spawnStartingChunks();
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;
        moveChunks();
        tickDifficulty();
    }

    public void ChangeSpeed(float amount) => changeChunkSpeed(amount);

    // Called from changeChunkSpeed when speed increases
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

    private void tickDifficulty()
    {
        if (difficultyLevel >= maxDifficultyLevel) return;
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= difficultyRampInterval)
        {
            difficultyTimer = 0f;
            difficultyLevel++;
            changeChunkSpeed(speedIncreasePerRamp);
            if (boxMakerRef != null) boxMakerRef.SetDifficulty(difficultyLevel);
        }
    }

    public void changeChunkSpeed(float speedNum)
    {
        float newMoveSpeed = Mathf.Clamp(moveSpeed + speedNum, minMoveSpeed, maxMoveSpeed);

        if (!Mathf.Approximately(newMoveSpeed, moveSpeed))
        {
            moveSpeed = newMoveSpeed;

            float newGravityZ = Mathf.Clamp(Physics.gravity.z - speedNum, minGravityZ, maxGravityZ);
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);

            if (CamaraPlayer != null)
                CamaraPlayer.changeCameraFOV(speedNum);

            if (speedNum > 0) ShowSpeedFlash();
        }
    }

    private void spawnStartingChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
            spawnChunk();
    }

    private void spawnChunk()
    {
        float spawnPositionZ = returnChunk();
        GameObject chunkToSpawn = whichChunkToSpawn();
        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);

        GameObject newChunkGo = Instantiate(chunkToSpawn, chunkSpawnPos, Quaternion.identity, ChunkParent);
        chunks.Add(newChunkGo);

        chunky newChunky = newChunkGo.GetComponent<chunky>();
        if (newChunky != null)
            newChunky.init(this, scoreManager, difficultyLevel);

        chunksOut++;
    }

    private GameObject whichChunkToSpawn()
    {
        if (chunksOut != 8 && chunksOut % 8 == 0)
            return checkpointChunkPrefab;
        return chunkPrefab[Random.Range(0, chunkPrefab.Length)];
    }

    private float returnChunk()
    {
        if (chunks.Count == 0)
            return transform.position.z;
        return chunks[chunks.Count - 1].transform.position.z + chunkLength;
    }

    private void moveChunks()
    {
        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = chunks[i];
            if (chunk == null) continue;

            chunk.transform.Translate(-transform.forward * (moveSpeed * Time.deltaTime));

            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
            {
                chunks.RemoveAt(i);
                Destroy(chunk);
                spawnChunk();
            }
        }
    }

    private void OnEnable()
    {
        if (scoreManager != null) scoreManager.onThresholdReached += SpawnShip;
    }

    private void OnDisable()
    {
        if (scoreManager != null) scoreManager.onThresholdReached -= SpawnShip;
    }

    private void SpawnShip()
    {
        GameObject spawnShip = Instantiate(shipPrefab, shipSpawnPosition, Quaternion.identity);

        ShipArrival ship = spawnShip.GetComponent<ShipArrival>();
        if (ship != null) ship.startArrival();

        LevelTransition levelTransition = spawnShip.GetComponent<LevelTransition>();
        if (levelTransition != null)
        {
            levelTransition.UnlockTransition();
            levelTransition.SetLevelCooker(this);
        }
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
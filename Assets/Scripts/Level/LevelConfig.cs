using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "King Runner/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Chunks")]
    public GameObject[] chunkPrefabs;
    public GameObject checkpointChunkPrefab;
    public int startingChunksAmount = 12;
    public float chunkLength = 10f;

    [Header("Speed")]
    public float startMoveSpeed = 8f;
    public float minMoveSpeed = 2f;
    public float maxMoveSpeed = 20f;

    [Header("Difficulty")]
    public float difficultyRampInterval = 15f;
    public float speedIncreasePerRamp = 1f;
    public int maxDifficultyLevel = 5;
}
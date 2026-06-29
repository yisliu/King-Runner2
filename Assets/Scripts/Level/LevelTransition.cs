using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private Object nextLevel;
    [SerializeField] private float triggerRadius = 3f;

    [Header("Boarding Sequence")]
    [SerializeField] private Transform boardingPoint;       // child of ship — where player walks to
    [SerializeField] private float boardingMoveDuration = 1.8f;
    [SerializeField] private float coinCountdownDuration = 1.4f;

    private bool allowTransition = false;
    private bool transitioning = false;
    private Transform player;
    private ILevelCooker levelCooker;

    public void SetLevelCooker(ILevelCooker lc) => levelCooker = lc;

    private void OnEnable() => scoreManager.onThresholdReached += UnlockTransition;
    private void OnDisable() => scoreManager.onThresholdReached -= UnlockTransition;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        if (levelCooker == null)
            levelCooker = FindFirstObjectByType<IslandLevelCooker>() as ILevelCooker
                       ?? FindFirstObjectByType<LevelCooker>() as ILevelCooker;
    }

    private void Update()
    {
        if (!allowTransition || transitioning || player == null) return;

        Vector2 shipXZ = new Vector2(transform.position.x, transform.position.z);
        Vector2 playerXZ = new Vector2(player.position.x, player.position.z);
        if (Vector2.Distance(shipXZ, playerXZ) <= triggerRadius)
        {
            transitioning = true;
            StartCoroutine(BoardingSequence());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (allowTransition && !transitioning && other.CompareTag("Player"))
        {
            transitioning = true;
            StartCoroutine(BoardingSequence());
        }
    }

    public void UnlockTransition()
    {
        allowTransition = true;
        Debug.Log("[LevelTransition] Ship boarding unlocked — enough coins collected.");
    }

    private IEnumerator BoardingSequence()
    {
        if (player == null) yield break;

        // Lock player controls
        var mover = player.GetComponent<PlayerPhysicalProgress>();
        var collision = player.GetComponent<playerCollison>();
        var rb = player.GetComponent<Rigidbody>();

        if (mover != null) mover.enabled = false;
        if (collision != null) collision.enabled = false;
        if (rb != null) rb.isKinematic = true;

        // Kill ship's idle hover so DOTween doesn't conflict
        DOTween.Kill(transform);

        // Start coin countdown on the HUD
        scoreManager sm = FindFirstObjectByType<scoreManager>();
        if (sm != null) sm.PlayCoinSpendAnimation(coinCountdownDuration);

        // Destination: the designated boarding point, or fall back to just in front of ship
        Vector3 destination = boardingPoint != null
            ? boardingPoint.position
            : transform.position + new Vector3(0f, 0f, 0.5f);

        // Snap player to face the ship before walking in
        Vector3 dirToShip = transform.position - player.position;
        dirToShip.y = 0f;
        if (dirToShip.sqrMagnitude > 0.001f)
            player.DORotateQuaternion(Quaternion.LookRotation(dirToShip.normalized), 0.3f).SetEase(Ease.OutSine);

        // Walk toward the ship's boarding point
        yield return player.DOMove(destination, boardingMoveDuration)
                           .SetEase(Ease.InSine)
                           .WaitForCompletion();

        // Player disappears into the ship
        yield return player.DOScale(Vector3.zero, 0.45f)
                           .SetEase(Ease.InBack)
                           .WaitForCompletion();

        // Ship reacts with a brief punch-scale so it feels like something entered it
        transform.DOPunchScale(Vector3.one * 0.18f, 0.5f, 5, 0.4f);
        yield return new WaitForSeconds(0.8f);

        Time.timeScale = 1f;

        if (levelCooker != null)
            levelCooker.EndRunSuccessful();
        else if (nextLevel != null)
            SceneManager.LoadScene(nextLevel.name);
        else
            Debug.LogError("[LevelTransition] No next level assigned and no level cooker found.");
    }
}
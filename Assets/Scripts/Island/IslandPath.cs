using UnityEngine;

public class IslandPath : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    private float[] cumulativeDistances;
    private float totalLength;

    public float TotalLength => totalLength;

    void Awake() => BuildPath();

    void BuildPath()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        cumulativeDistances = new float[waypoints.Length];
        cumulativeDistances[0] = 0f;

        for (int i = 1; i < waypoints.Length; i++)
            cumulativeDistances[i] = cumulativeDistances[i - 1]
                + Vector3.Distance(waypoints[i - 1].position, waypoints[i].position);

        totalLength = cumulativeDistances[waypoints.Length - 1];
    }

    // Wraps any distance value into [0, totalLength)
    float Wrap(float distance)
    {
        if (totalLength <= 0f) return 0f;
        return ((distance % totalLength) + totalLength) % totalLength;
    }

    public Vector3 GetPositionAtDistance(float distance)
    {
        if (waypoints == null || waypoints.Length < 2) return transform.position;
        distance = Wrap(distance);

        for (int i = 1; i < waypoints.Length; i++)
        {
            if (distance <= cumulativeDistances[i])
            {
                float segStart = cumulativeDistances[i - 1];
                float segLen = cumulativeDistances[i] - segStart;
                float t = (distance - segStart) / segLen;
                return Vector3.Lerp(waypoints[i - 1].position, waypoints[i].position, t);
            }
        }
        return waypoints[waypoints.Length - 1].position;
    }

    public Quaternion GetRotationAtDistance(float distance)
    {
        if (waypoints == null || waypoints.Length < 2) return Quaternion.identity;
        distance = Wrap(distance);

        for (int i = 1; i < waypoints.Length; i++)
        {
            if (distance <= cumulativeDistances[i])
            {
                Vector3 dir = (waypoints[i].position - waypoints[i - 1].position).normalized;
                return dir != Vector3.zero ? Quaternion.LookRotation(dir) : Quaternion.identity;
            }
        }
        Vector3 lastDir = (waypoints[waypoints.Length - 1].position
            - waypoints[waypoints.Length - 2].position).normalized;
        return Quaternion.LookRotation(lastDir);
    }

    // Returns the world-space facing direction at the start of the path
    // Use this to orient the player when the scene starts
    public Vector3 StartDirection =>
        waypoints != null && waypoints.Length >= 2
            ? (waypoints[1].position - waypoints[0].position).normalized
            : Vector3.forward;

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }

        if (waypoints[0] != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(waypoints[0].position, 0.8f);
        }
        if (waypoints[waypoints.Length - 1] != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(waypoints[waypoints.Length - 1].position, 0.8f);
        }
    }
}

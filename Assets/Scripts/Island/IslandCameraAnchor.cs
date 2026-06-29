using UnityEngine;

// Point your Cinemachine virtual camera at this object instead of the player.
// It follows the player's position and Y rotation only — no pitch or roll.
public class IslandCameraAnchor : MonoBehaviour
{
    [SerializeField] private Transform player;

    void LateUpdate()
    {
        transform.position = player.position;
        transform.rotation = Quaternion.Euler(0f, player.eulerAngles.y, 0f);
    }
}

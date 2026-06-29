using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private CharacterData[] characters;
    [SerializeField] private Transform spawnPoint;

    void Awake()
    {
        if (characters == null || characters.Length == 0) return;

        int index = PlayerPrefs.GetInt("SelectedCharacter", 0);
        index = Mathf.Clamp(index, 0, characters.Length - 1);

        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion rot = spawnPoint != null ? spawnPoint.rotation : transform.rotation;

        Instantiate(characters[index].prefab, pos, rot);
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "King Runner/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public GameObject prefab;
    public Sprite portrait;
}
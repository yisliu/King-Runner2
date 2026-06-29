using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private CharacterData[] characters;
    [SerializeField] private Image[] portraits;
    [SerializeField] private TMP_Text[] nameLabels;
    [SerializeField] private GameObject[] selectionHighlights;

    private int selectedIndex;

    void Start()
    {
        selectedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        RefreshUI();
    }

    public void SelectCharacter(int index)
    {
        selectedIndex = Mathf.Clamp(index, 0, characters.Length - 1);
        PlayerPrefs.SetInt("SelectedCharacter", selectedIndex);
        PlayerPrefs.Save();
        RefreshUI();
    }

    void RefreshUI()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (i < portraits.Length && portraits[i] != null)
                portraits[i].sprite = characters[i].portrait;

            if (i < nameLabels.Length && nameLabels[i] != null)
                nameLabels[i].text = characters[i].characterName;

            if (i < selectionHighlights.Length && selectionHighlights[i] != null)
                selectionHighlights[i].SetActive(i == selectedIndex);
        }
    }
}
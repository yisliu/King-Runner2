using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum DifficultyLevel { Easy, Normal, Hard }


[CreateAssetMenu(fileName = "NewPlayerSettings", menuName = "Settings/Player Settings Data")]
[System.Serializable]
public class PlayerSettings : ScriptableObject
{
    public DifficultyLevel difficultyLevel = DifficultyLevel.Normal;
    public int selectedPlayerIndex = 0;
    public int selectedSpriteIndex = 0;
    public float musicVolume = 0.75f;
    public float sfxVolume = 0.75f;
}



public static class SettingsManager
{
    private const string DIFFICULTY_KEY = "Setting_Difficulty";
    private const string PLAYER_INDEX_KEY = "Setting_PlayerIndex";
    private const string SPRITE_INDEX_KEY = "Setting_SpriteIndex";
    private const string MUSIC_VOL_KEY = "Setting_MusicVolume";

    public static void SaveSettings(DifficultyLevel difficulty, int playerIndex, int spriteIndex, float musicVol)
    {
        PlayerPrefs.SetInt(DIFFICULTY_KEY, (int)difficulty);
        PlayerPrefs.SetInt(PLAYER_INDEX_KEY, playerIndex);
        PlayerPrefs.SetInt(SPRITE_INDEX_KEY, spriteIndex);
        PlayerPrefs.SetFloat(MUSIC_VOL_KEY, musicVol);
        PlayerPrefs.Save(); 
    }

    public static void LoadSettings(PlayerSettings targetSO)
    {
        if (targetSO == null) return;

        targetSO.difficultyLevel = (DifficultyLevel)PlayerPrefs.GetInt(DIFFICULTY_KEY, (int)DifficultyLevel.Normal);
        targetSO.selectedPlayerIndex = PlayerPrefs.GetInt(PLAYER_INDEX_KEY, 0);
        targetSO.selectedSpriteIndex = PlayerPrefs.GetInt(SPRITE_INDEX_KEY, 0);
        targetSO.musicVolume = PlayerPrefs.GetFloat(MUSIC_VOL_KEY, 0.75f);
    }
}

public class SettingsUIController : MonoBehaviour
{
    [Header("Data Link")]
    [SerializeField] private PlayerSettings runtimeSettings;

    [Header("UI Fields")]
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button[] difficultyButtons;
    private void Start()
    {
        if (runtimeSettings != null)
        {
            SettingsManager.LoadSettings(runtimeSettings);
            InitializeUIElements();
        }
        else
        {
            Debug.LogWarning("Please assign your PlayerSettings scriptable object asset to the SettingsUIController!");
        }
    }

    private void InitializeUIElements()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = runtimeSettings.musicVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            AudioListener.volume = runtimeSettings.musicVolume; 
        }

        if (difficultyButtons != null)
        {
            for (int i = 0; i < difficultyButtons.Length; i++)
            {
                if (difficultyButtons[i] == null) continue;
                int index = i; 
                difficultyButtons[i].onClick.AddListener(() => OnDifficultySelected((DifficultyLevel)index));
            }
            UpdateDifficultyVisuals();
        }
    }

    public void OpenPanel()
    {
        if (panelRect != null)
        {
            panelRect.DOAnchorPosX(0f, 0.35f).SetEase(Ease.OutCubic).SetUpdate(true);
        }
    }

    public void ClosePanel()
    {
        if (panelRect != null)
        {
            panelRect.DOAnchorPosX(1920f, 0.3f).SetEase(Ease.InCubic).SetUpdate(true);
        }
        
        SaveCurrentConfigurations();
    }

    private void OnVolumeChanged(float val)
    {
        if (runtimeSettings != null)
        {
            runtimeSettings.musicVolume = val;
        }
        AudioListener.volume = val; 
    }

    private void OnDifficultySelected(DifficultyLevel level)
    {
        if (runtimeSettings != null)
        {
            runtimeSettings.difficultyLevel = level;
        }
        UpdateDifficultyVisuals();
    }

    private void UpdateDifficultyVisuals()
    {
        if (difficultyButtons == null || runtimeSettings == null) return;

        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            if (difficultyButtons[i] == null) continue;

            ColorBlock cb = difficultyButtons[i].colors;
            cb.normalColor = (i == (int)runtimeSettings.difficultyLevel) ? Color.green : Color.gray;
            difficultyButtons[i].colors = cb;
        }
    }

    private void SaveCurrentConfigurations()
    {
        if (runtimeSettings == null) return;

        SettingsManager.SaveSettings(
            runtimeSettings.difficultyLevel,
            runtimeSettings.selectedPlayerIndex,
            runtimeSettings.selectedSpriteIndex,
            runtimeSettings.musicVolume
        );
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipTransition : MonoBehaviour
{
    [SerializeField] private string nextLevelName; 

    public void LoadScene()
    {
        PlayerPrefs.SetString("NextTargetLevel", nextLevelName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("ShipHub");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            LoadScene();
        }
    }
}
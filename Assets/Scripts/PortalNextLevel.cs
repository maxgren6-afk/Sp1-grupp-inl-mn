using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalLevelNext : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke(nameof(LoadNextLevel), 0);
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelIndex);
    }
    
}

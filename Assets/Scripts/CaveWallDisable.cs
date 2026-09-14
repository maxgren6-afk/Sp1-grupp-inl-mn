using UnityEngine;

public class CaveWallDisable : MonoBehaviour
{
    [SerializeField] private GameObject caveWall;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            caveWall.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            caveWall.SetActive(true);
        }
    }
}

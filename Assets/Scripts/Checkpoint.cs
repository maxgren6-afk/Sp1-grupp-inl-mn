// using Unity.Tutorials.Editor;
// using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform newSpawn;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spawnPosition.position = newSpawn.position;

            other.GetComponent<PlayerHealth>().CallNewSpawn(newSpawn);

            anim.SetTrigger("TouchCheckpoint");
        }
    }
}

using UnityEngine;

public class FallingWall : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject button;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("Move");

            button.SetActive(false);
        }
    }
}

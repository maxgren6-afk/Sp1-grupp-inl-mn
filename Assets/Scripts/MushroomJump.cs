using UnityEngine;

public class MushroomJump : MonoBehaviour
{

    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private AudioClip jumpMushroom;
    private Animator anim;
    private AudioSource audioSource;


    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rgbd = other.gameObject.GetComponent<Rigidbody2D>();
                if (rgbd != null)
            {
                rgbd.linearVelocity = new Vector2(rgbd.linearVelocity.x, 0);

                rgbd.AddForce(new Vector2(0, jumpForce));

                // Startar mushroom animation
                anim.SetTrigger("Activate");

                audioSource.pitch = Random.Range(0.75f, 1.25f);
                audioSource.PlayOneShot(jumpMushroom);
            }
        }
    }

}

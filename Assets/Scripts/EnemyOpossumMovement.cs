using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float bounciness = 100f;
    [SerializeField] private int damageGiven = 1;
    [SerializeField] private float knockbackForce = 100f;
    [SerializeField] private float upwardsForce = 5f;
    [SerializeField] private AudioClip enemyDestroy;



    private SpriteRenderer rend;
    private AudioSource audioSource;
    private bool canTakeDamage = true;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (moveSpeed < 0)
        {
            rend.flipX = false;
        }

        if (moveSpeed > 0)
        {
            rend.flipX = true;
        }
    }

    void FixedUpdate()
    {
        transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime);
    }

    public void ReverseDirection()
    {
        moveSpeed *= -1;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Lägg till || om enemy collision vill has
        if (other.gameObject.CompareTag("EnemyBlock") || other.gameObject.CompareTag("Enemy"))
        {
            moveSpeed = moveSpeed * -1;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageGiven);
            PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            BossOpossum boss = GetComponent<BossOpossum>();

            if (other.transform.position.x > transform.position.x)
            {
                if (boss != null)
                {
                    playerMovement.TakeBossKnockback(knockbackForce, upwardsForce);
                }
                else
                {
                    playerMovement.TakeKnockback(knockbackForce, upwardsForce);
                }
            }
            else
            {
                if (boss != null)
                {
                    playerMovement.TakeBossKnockback(-knockbackForce, upwardsForce);
                }
                else
                {
                    playerMovement.TakeKnockback(-knockbackForce, upwardsForce);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerSword"))
        {
            Rigidbody2D rgbd = other.attachedRigidbody;

            if (rgbd != null)
            {
                rgbd.linearVelocity = new Vector2(rgbd.linearVelocity.x, 0);
                rgbd.AddForce(new Vector2(0, bounciness));

                // Instantiate(enemyDestroy, transform.position, Quaternion.identity);
            }

            BossHealth bossHealth = GetComponent<BossHealth>();

            if (bossHealth != null && canTakeDamage)
            {
                bossHealth.TakeDamage(1);

                canTakeDamage = false;

                Invoke(nameof(CallBossTakeDamageReset), 0.1f);
            }
        }
    }

    private void CallBossTakeDamageReset()
    {
        canTakeDamage = true;
    }
}

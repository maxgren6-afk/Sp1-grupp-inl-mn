using UnityEngine;

public class BossOpossum : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 6f;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float timeBetweenDashes = 3f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private float timeBetweenJumps = 4f;
    [SerializeField] private float sizeMultiplier = 2f;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2D;
    private BossMovement enemyMovement;
    private float dashTimer;
    private float dashCooldown;
    private float jumpCooldown;
    private int dashDirection;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<BossMovement>();
        enemyMovement?.ReverseDirection();
        transform.localScale *= sizeMultiplier;
        dashCooldown = timeBetweenDashes;
        jumpCooldown = timeBetweenJumps;
    }

    private void Update()
    {
        dashCooldown -= Time.deltaTime;
        jumpCooldown -= Time.deltaTime;

        if (dashCooldown <= 0f && player != null)
        {
        // Start a dash toward the player's current position
            dashTimer = dashTime;
            dashCooldown = timeBetweenDashes;
            dashDirection = player.position.x >= transform.position.x ? 1 : -1;
        }

        // Move quickly while the dash is active
        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
            transform.Translate(Vector2.right * dashDirection * dashSpeed * Time.deltaTime);
        }

        // Jump when the jump timer is ready and the boss is on the ground
        if (jumpCooldown <= 0f && dashTimer <= 0f && rigidbody2D != null && Mathf.Abs(rigidbody2D.linearVelocity.y) < 0.1f)
        {
            rigidbody2D.AddForce(Vector2.up * jumpForce);
            jumpCooldown = timeBetweenJumps;
        }
    }

    private void LateUpdate()
    {
        if (dashTimer > 0f)
        {
            spriteRenderer.flipX = dashDirection < 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBlock") || other.gameObject.CompareTag("Enemy"))
        {
            dashDirection *= -1;
            enemyMovement?.ReverseDirection();
        }

    }
}

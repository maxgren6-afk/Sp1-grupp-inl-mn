using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 5;
    [SerializeField] private GameObject bossCanvas;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color normalHealthColor, mediumHealthColor, criticalHealthColor;

    private int currentHealth;
    private Vector3 spawnPosition;
    private Rigidbody2D rigidbody2D;
    private Transform player;
    private Vector3 playerSpawnPosition;
    private Rigidbody2D playerRigidbody;
    private PlayerHealth playerHealth;

    private void Start()
    {
        currentHealth = startingHealth;
        spawnPosition = transform.position;
        rigidbody2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerHealth = player != null ? player.GetComponent<PlayerHealth>() : null;

        if (playerHealth != null)
        {
            normalHealthColor = playerHealth.NormalHealthColor;
            mediumHealthColor = playerHealth.MediumHealthColor;
            criticalHealthColor = playerHealth.CriticalHealthColor;
        }

        if (player != null)
        {
            playerSpawnPosition = player.position;
            playerRigidbody = player.GetComponent<Rigidbody2D>();
        }

        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = startingHealth;
        }

        UpdateHealthbar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthbar();

        if (currentHealth <= 0)
        {
            GameObject canvasToHide = bossCanvas != null ? bossCanvas : GameObject.Find("BossCanvas");

            if (canvasToHide != null)
            {
                canvasToHide.SetActive(false);
            }

            Destroy(gameObject);
        }
        else
        {
            transform.position = spawnPosition;
            transform.localScale += Vector3.one;

            if (rigidbody2D != null)
            {
                rigidbody2D.linearVelocity = Vector2.zero;
            }
        }
    }

    private void UpdateHealthbar()
    {
        if (healthSlider == null || fillImage == null)
        {
            return;
        }

        healthSlider.value = currentHealth;

        if (currentHealth >= 3)
        {
            fillImage.color = normalHealthColor;
        }

        if (currentHealth == 2)
        {
            fillImage.color = mediumHealthColor;
        }

        if (currentHealth <= 1)
        {
            fillImage.color = criticalHealthColor;
        }
    }
}

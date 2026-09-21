using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 5;
    [SerializeField] private GameObject bossCanvas;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;

    private int currentHealth;
    private Vector3 spawnPosition;
    private Rigidbody2D rigidbody2D;
    private Transform player;
    private Vector3 playerSpawnPosition;
    private Rigidbody2D playerRigidbody;

    private void Start()
    {
        currentHealth = startingHealth;
        spawnPosition = transform.position;
        rigidbody2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

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
            if (player != null)
            {
                player.position = playerSpawnPosition;

                if (playerRigidbody != null)
                {
                    playerRigidbody.linearVelocity = Vector2.zero;
                }
            }

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
            fillImage.color = new Color32(0x6C, 0x9F, 0x14, 0xFF);
        }

        if (currentHealth == 2)
        {
            fillImage.color = new Color32(0xE5, 0xC1, 0x00, 0xFF);
        }

        if (currentHealth <= 1)
        {
            fillImage.color = new Color32(0xFF, 0x46, 0x00, 0xFF);
        }
    }
}

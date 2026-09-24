using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int startingHealth = 3;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Slider healthSlider;
    [SerializeField] Image fillImage;
    [SerializeField] private Color normalHealthColor, mediumHealthColor, criticalHealthColor;
    private int currentHealth;

    public Color NormalHealthColor => normalHealthColor;
    public Color MediumHealthColor => mediumHealthColor;
    public Color CriticalHealthColor => criticalHealthColor;


    
    void Start()
    {
        currentHealth = startingHealth;

        UpdateHealthbar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        UpdateHealthbar();

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = spawnPosition.position;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        currentHealth = startingHealth;

        UpdateHealthbar();

        fillImage.color = normalHealthColor;
    }

    private void UpdateHealthbar()
    {
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

    public bool RestoreHealth(int healthToRestore)
    {
        if (currentHealth >= startingHealth)
        {
            return false;
        }
        currentHealth += healthToRestore;
        UpdateHealthbar();

        if (currentHealth > startingHealth)
        {
            currentHealth = startingHealth;
        }

        return true;
    }

    public void CallTakeDamage(int damageTaken)
    {
        // Debug.Log("Damage2");
        currentHealth -= (damageTaken);

        UpdateHealthbar();

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    public void CallNewSpawn(Transform newSpawn)
    {
        spawnPosition = newSpawn;
    }

    public void CallHealthRestore()
    {
        currentHealth = startingHealth;

        UpdateHealthbar();
    }


}

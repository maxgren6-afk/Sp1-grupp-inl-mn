using UnityEngine;

public class SpikesDamage : MonoBehaviour
{
    [SerializeField] private int spikeDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Damage");
            other.GetComponent<PlayerHealth>().CallTakeDamage(spikeDamage);
        }
    }
}

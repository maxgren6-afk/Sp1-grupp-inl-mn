using UnityEngine;

public class SpikesDamage : MonoBehaviour
{
    [SerializeField] private int spikeDamage;
    private bool canTakeDamage = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (canTakeDamage)
            {
                other.GetComponent<PlayerHealth>().CallTakeDamage(spikeDamage);

                canTakeDamage = false;

                Invoke(nameof(CallCanTakeDamageReset), 0.2f);
            }
        }
    }

    private void CallCanTakeDamageReset()
    {
        canTakeDamage = true;
    }
}

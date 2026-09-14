using UnityEngine;

public class BananaPickup : MonoBehaviour
{

    [SerializeField] private GameObject bananaParticleSystem;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))   
        {
            other.gameObject.GetComponent<PlayerQuest>().AddBanana();

            Instantiate(bananaParticleSystem, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}

using UnityEngine;

public class Checkpoint_Bossrespawn : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossbar;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.SetActive(true);
            bossbar.SetActive(true);
        }

        /*if (boss != null)
        {
            GetComponent<BossRespawn>().RespawnBoss();
        }*/
    }
}

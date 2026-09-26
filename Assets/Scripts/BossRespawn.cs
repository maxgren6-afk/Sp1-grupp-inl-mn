using UnityEngine;

public class BossRespawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    public void RespawnBoss()
    {
        GetComponent<BossHealth>().CallResetHealth();

        transform.position = spawnPoint.position;
    }
}

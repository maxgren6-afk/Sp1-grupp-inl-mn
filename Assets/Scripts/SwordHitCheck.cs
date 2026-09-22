using System;
using Unity.Tutorials.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class SwordHitCheck : MonoBehaviour
{
    [SerializeField] private GameObject swordHitbox;
    [SerializeField] private float swordForce = 600f;
    [SerializeField] private ParticleSystem swordJumpParticle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.IsDestroyed();

            GetComponent<PlayerMovement>().CallSwordJump(swordForce);

            swordJumpParticle.Play();
        }

        //if (boss)
    }
}

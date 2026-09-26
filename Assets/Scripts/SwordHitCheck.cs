using System;
using Unity.Collections;
using Unity.Tutorials.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class SwordHitCheck : MonoBehaviour
{
    [SerializeField] private GameObject swordHitbox, player;
    [SerializeField] private float swordForce, swordKnockback, minSwordJumpHeight;
    [SerializeField] private int swordDamage = 1;
    [SerializeField] private ParticleSystem swordJumpParticle;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

            float enemyPosY = other.transform.position.y;
            float enemyPosX = other.transform.position.x;

            float playerPosY = player.transform.position.y;
            float playerPosX = player.transform.position.x;

            //Debug.Log("Hit");

            if (playerPosY >= (minSwordJumpHeight + enemyPosY))
            {
                GetComponentInParent<PlayerMovement>().CallSwordJump(swordForce);
            }

            if (playerPosY <= minSwordJumpHeight + enemyPosY)
            {
                //Debug.Log("Hit");

                if (playerPosX > enemyPosX)
                {
                    GetComponentInParent<PlayerMovement>().CallSwordKnockback(swordKnockback);
                }

                if (playerPosX < enemyPosX)
                {
                    //Debug.Log("Hit");

                    GetComponentInParent<PlayerMovement>().CallSwordKnockback(swordKnockback * -1);
                }
            }

            swordJumpParticle.transform.position = other.transform.position;
            swordJumpParticle.Play();

            other.gameObject.SetActive(false);

        }

        //if (other.CompareTag("Boss"))
        //{
        //    other.GetComponent<BossHealth>().TakeDamage(swordDamage);
        //}
    }

    //private void CallEnemyDestroy()
    //{
    //    swordJumpParticle.Play();
    //}
}

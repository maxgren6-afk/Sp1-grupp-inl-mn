using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject swordHitBox;
    [SerializeField] private float attackTimer;
    private Animator animator;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            CallSwordAttack();
        }
    }

    private void CallSwordAttack()
    {
        Debug.Log("Setting AttackTrigger");
        animator.SetTrigger("AttackTrigger");

        swordHitBox.SetActive(true);

        Invoke(nameof(CallSwordAttackEnd), attackTimer);
    }

    private void CallSwordAttackEnd()
    {
        swordHitBox.SetActive(false);
    }
}
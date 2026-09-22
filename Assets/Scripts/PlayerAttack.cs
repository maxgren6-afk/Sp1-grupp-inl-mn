using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Setting AttackTrigger");
            animator.SetTrigger("AttackTrigger");
        }
    }
}
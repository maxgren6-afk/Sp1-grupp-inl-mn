using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject swordHitBox;
    [SerializeField] private float attackTimer;
    private Animator animator;

    float mouseX;
    float mouseY;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        mouseX = mousePos.x;
        mouseY = mousePos.y;

        Debug.Log(mousePos);
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
        //Debug.Log("Setting AttackTrigger");
        animator.SetTrigger("AttackTrigger");

        swordHitBox.SetActive(true);

        Invoke(nameof(CallSwordAttackEnd), (attackTimer + Time.deltaTime));

        if (mouseX > 960f)
        {
            if (mouseY > 360f && mouseY < 720f)
            {
                if (!GetComponent<PlayerMovement>().CallCurrentDirection())
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                    CallAttackReset();
                }

                if (GetComponent<PlayerMovement>().CallCurrentDirection())
                {
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                    CallAttackReset();
                }
            }

            if (mouseY > 720f)
            {
                transform.position = new Vector2((transform.position.x), (transform.position.y + 0.5f));

                transform.rotation = Quaternion.Euler(0f, 0f, 90f);

                CallAttackReset();
            }

            if (mouseY < 360f)
            {
                transform.position = new Vector2((transform.position.x), (transform.position.y + 0.5f));

                transform.rotation = Quaternion.Euler(0f, 0f, 270f);

                CallAttackReset();
            }
        }

        if (mouseX < 960f)
        {
            if (mouseY > 360f && mouseY < 720f)
            {
                if (GetComponent<PlayerMovement>().CallCurrentDirection())
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                    CallAttackReset();
                }

                if (!GetComponent<PlayerMovement>().CallCurrentDirection())
                {
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                    CallAttackReset();
                }
            }

            if (mouseY > 720f)
            {
                transform.position = new Vector2((transform.position.x), (transform.position.y + 0.5f));

                transform.rotation = Quaternion.Euler(0f, 180f, 90f);

                CallAttackReset();
            }

            if (mouseY < 360f)
            {
                transform.position = new Vector2((transform.position.x), (transform.position.y + 0.5f));

                transform.rotation = Quaternion.Euler(0f, 180f, 270f);

                CallAttackReset();
            }
        }

    }

    private void CallSwordAttackEnd()
    {
        swordHitBox.SetActive(false);
    }

    private void CallAttackReset()
    {
        Invoke(nameof(CallResetRotation), attackTimer);
    }

    private void CallResetRotation()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
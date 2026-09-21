using System;
// using System.Runtime.CompilerServices;
// using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;
    private float moveDirection;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 200f;
    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float raycastDistance = 0.25f;
    [SerializeField] private AudioClip jumpSoundEffect;
    [SerializeField] private ParticleSystem jumpParticleSystem;

    bool canMove = true;




    // Hämtar andra resurser
    private AudioSource audioSource;
    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        
        jump.action.started += Jump;
    }


    // Update is called once per frame
    void Update()
    {
        moveDirection = move.action.ReadValue<float>();

        anim.SetFloat("MoveSpeed",MathF.Abs(rgbd.linearVelocity.x));
        anim.SetFloat("VerticalSpeed", rgbd.linearVelocity.y);
        anim.SetBool("IsGrounded", CheckIsGrounded());

        if (moveDirection < 0f)
        {
            FlipSprite(true);
        }

        if (moveDirection > 0)
        {
            FlipSprite(false);
        }
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
        rgbd.linearVelocity = new Vector2(moveDirection * moveSpeed * Time.deltaTime, rgbd.linearVelocity.y);
    }

    private void OnDisable()
    {
        jump.action.started -= Jump;
    }

    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }

    // Metod för jump funktionen, context viktigt
    private void Jump(InputAction.CallbackContext context)
    {
        if (CheckIsGrounded() == true)
        {
            rgbd.AddForce(new Vector2(0, jumpForce));
            audioSource.PlayOneShot(jumpSoundEffect);

            jumpParticleSystem.Play();
        }
        
    }

    private bool CheckIsGrounded()
    {
        // Kollar om det finns ett object med Layer 6 (Ground) i Raycast distansen
        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, raycastDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFoot.position, Vector2.down, raycastDistance, whatIsGround);

        // Debug.DrawRay(leftFoot.position, Vector2.down * raycastDistance, Color.blue, 0.25f);
        // Debug.DrawRay(rightFoot.position, Vector2.down * raycastDistance, Color.blue, 0.25f);

        if (leftHit.collider != null && leftHit || rightHit.collider != null && rightHit)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void TakeKnockback(float knockbackForce, float upwardsForce)
    {
        canMove = false;
        rgbd.AddForce(new Vector2(knockbackForce, upwardsForce));
        Invoke("CanMoveAgain", 0.25f);
    }

    public void TakeBossKnockback(float knockbackForce, float upwardsForce)
    {
        canMove = false;
        rgbd.AddForce(new Vector2(knockbackForce, upwardsForce) * 0.25f, ForceMode2D.Impulse);
        Invoke("CanMoveAgain", 0.25f);
    }

    private void CanMoveAgain()
    {
        canMove = true;
    }
}

using System;
using Unity.Hierarchy;

// using System.Runtime.CompilerServices;
// using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference dash;
    private float moveDirection;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 200f;
    [SerializeField] private Transform leftFoot, rightFoot, leftArm, rightArm;
    [SerializeField] private LayerMask whatIsGround, whatIsWall;
    [SerializeField] private float raycastDistance = 0.25f;
    [SerializeField] private AudioClip[] jumpStartSoundEffects, footstepSoundEffects, jumpLandSoundEffects;
    [SerializeField] private AudioClip dashSoundEffect;
    [SerializeField] private ParticleSystem jumpParticleSystem, dashParticleSystem;
    [SerializeField] private float wallJumpForceX, wallJumpForceY, wallJumpCooldown, dashForce, dashCooldown;
 

    bool canMove = true;
    bool canWallJump = true;
    bool canDash = true;
    private bool wasGrounded;


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
        dash.action.started += Dash;
    }


    // Update is called once per frame
    void Update()
    {
        moveDirection = move.action.ReadValue<float>();

        bool isGrounded = CheckIsGrounded();

        if (isGrounded && !wasGrounded)
        {
            PlayRandomSound(jumpLandSoundEffects);
        }
        wasGrounded = isGrounded;

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
        dash.action.started -= Dash;
    }

    private void FlipSprite(bool direction)
    {
        if (rend.flipX != direction)
        {
            rend.flipX = direction;

            if (CheckIsGrounded() == true)
            {
                PlayFootstepSound();
            }
        }
    }

    // Metod för jump funktionen, context viktigt
    private void Jump(InputAction.CallbackContext context)
    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftArm.position, Vector2.left, raycastDistance, whatIsWall);
        RaycastHit2D rightHit = Physics2D.Raycast(rightArm.position, Vector2.right, raycastDistance, whatIsWall);


        if (CheckIsGrounded() == true)
        {
            rgbd.AddForce(new Vector2(0, jumpForce));
            PlayRandomSound(jumpStartSoundEffects);

            CallJumpParticle();
        }

        if (CheckIsWall() == true && !CheckIsGrounded())
        {
            if (leftHit.collider != null && leftHit && canWallJump)
            {
                //Resettar all velocity så att man inte kan chain hoppa på väggar
                rgbd.linearVelocity = new Vector2(0, 0);

                rgbd.AddForce(new Vector2(wallJumpForceX, wallJumpForceY));
                PlayRandomSound(jumpStartSoundEffects);

                CallJumpParticle();

                CallWalljumpSpent();
            }

            if (rightHit.collider != null && rightHit && canWallJump)
            {
                //Resettar all velocity så att man inte kan chain hoppa på väggar
                rgbd.linearVelocity = new Vector2(0, 0);

                rgbd.AddForce(new Vector2(-wallJumpForceX, wallJumpForceY));
                PlayRandomSound(jumpStartSoundEffects);

                CallJumpParticle();

                CallWalljumpSpent();
            }
        }
        
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (canDash)
        {
            canMove = false;

            rgbd.linearVelocity = new Vector2(0, 0);

            if (!rend.flipX)
            {
                rgbd.AddForce(new Vector2(dashForce, 0));
            }
            else
            {
                rgbd.AddForce(new Vector2(-dashForce, 0));
            }

            //Gör en trigger som heter Dash om man vill ha en animation
            //anim.SetTrigger("Dash");

            CallDashParticle();

            CallDashSpent();

            audioSource.PlayOneShot(dashSoundEffect);

            Invoke("CanMoveAgain", 0.3f);


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

    private bool CheckIsWall()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftArm.position, Vector2.left, raycastDistance, whatIsWall);
        RaycastHit2D rightHit = Physics2D.Raycast(rightArm.position, Vector2.right, raycastDistance, whatIsWall);

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

    private void CallJumpParticle()
    {
        jumpParticleSystem.Play();
    }

    private void CallDashParticle()
    {
        dashParticleSystem.Play();
    }

    private void CallWalljumpSpent()
    {
        canWallJump = false;

        Invoke(nameof(CallWallJumpReset), wallJumpCooldown);
    }

    private void CallWallJumpReset()
    {
        canWallJump = true;
    }

    private void CallDashSpent()
    {
        canDash = false;

        Invoke("CallDashReset", dashCooldown);
    }
    private void CallDashReset()
    {
        canDash = true;
    }

    public void CallSwordJump(float swordForce)
    {
        rgbd.linearVelocity = new Vector2(rgbd.linearVelocity.x, 0);

        canMove = false;

        rgbd.AddForce(new Vector2(rgbd.linearVelocity.x, swordForce));

        Invoke(nameof(CanMoveAgain), (0.1f + Time.deltaTime));
    }

    public void CallSwordKnockback(float swordKnockbackPM)
    {
        rgbd.linearVelocity = new Vector2(0, rgbd.linearVelocity.y);

        canMove = false;

        rgbd.AddForce(new Vector2(swordKnockbackPM, rgbd.linearVelocity.y));

        Invoke(nameof(CanMoveAgain), (0.1f + Time.deltaTime));
    }

    public bool CallCurrentDirection()
    {
        return rend.flipX;
    }

    public void PlayFootstepSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, 10);
        audioSource.PlayOneShot(footstepSoundEffects[randomIndex]);
    }

    private void PlayRandomSound(AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0 && audioSource != null)
        {
            int randomIndex = UnityEngine.Random.Range(0, clips.Length);
            audioSource.PlayOneShot(clips[randomIndex]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    [SerializeField] private float deathHeight = -30f;

    private float dirX = 0f;
    // [SerializedField] exposes vars in unity inspector
    [SerializeField] private float baseMoveSpeed = 7f;
    [SerializeField] private float baseJumpForce = 14f;

    private float currentMoveSpeed;
    private float currentJumpForce;

    private enum MovementState { idle, running, jumping, falling};

    [SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello, world!");
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        currentMoveSpeed = baseMoveSpeed;
        currentJumpForce = baseJumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        // using rb.velocity.y to keep same vector when mixing run and jump
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * currentMoveSpeed, rb.velocity.y);
        
        if (Input.GetButtonDown("Jump") && IsGrounded()) 
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
        }

        if (gameObject.transform.position.y < deathHeight)
        {
            transform.position = new Vector2(transform.position.x, deathHeight+1);
            rb.velocity = new Vector2(0, 0);
            GetComponent<PlayerLife>().Die();
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        // 0.1 instead of 0 to account for imprecision
        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }else if(rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    public void updateItemWeight(float totalItemWeight)
    {
        float newMoveSpeed = baseMoveSpeed - totalItemWeight;
        float newJumpForce = baseJumpForce - totalItemWeight*0.5f;

        if (newMoveSpeed >= 5f)
        {
            currentMoveSpeed = newMoveSpeed;
        }
        //if (newJumpForce >= 12f)
        //{
        //    currentJumpForce = newJumpForce;
        //}
    }

    public float getMoveSpeed()
    {
        return currentMoveSpeed;
    }

}

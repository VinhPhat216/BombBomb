using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerMove")]
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private KeyCode inputUp = KeyCode.W;
    [SerializeField] private KeyCode inputDown = KeyCode.S;
    [SerializeField] private KeyCode inputRight = KeyCode.D;
    [SerializeField] private KeyCode inputLeft = KeyCode.A;
    private Vector2 direction = Vector2.down;
    public float speed;

    [Header("Animation")]
    [SerializeField] private AnimatedSpriteRenderer spriteRendererUp;
    [SerializeField] private AnimatedSpriteRenderer spriteRendererDown;
    [SerializeField] private AnimatedSpriteRenderer spriteRendererLeft;
    [SerializeField] private AnimatedSpriteRenderer spriteRendererRight;
    [SerializeField] private AnimatedSpriteRenderer spriteRendererDeath;
    private AnimatedSpriteRenderer activeSpriteRenderer;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        Vector2 position = playerRb.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        playerRb.MovePosition(position + translation);
    }

    private void Move()
    {
        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up, spriteRendererUp);
        }
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down, spriteRendererDown);
        }
        else if (Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right, spriteRendererRight);
        }
        else if (Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
        }
        else
        {
            SetDirection(Vector2.zero, activeSpriteRenderer);
        }
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    public void AddSpeed()
    {
        speed += 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;

        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererUp.enabled = false;
        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
    }
}


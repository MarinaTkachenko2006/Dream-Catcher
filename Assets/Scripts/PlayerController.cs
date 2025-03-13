using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
// using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    private bool isMoving;
    public bool isSpeedBoosted = false;
    public float baseSpeed;
    public Sprite normalSprite;
    public Sprite boostedSprite;
    private SpriteRenderer spriteRenderer;

    private Vector2 input;

    private Animator animator;

    public LayerMask invisibleWallsLayer;

    public LayerMask interactableLayer;

    AudioManager audioManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseSpeed = moveSpeed;
    }

    public void HandleUpdate()
    {
        float speedMultiplier = 1f;

        // Ускорение при удерживании Shift
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speedMultiplier = 1.5f;
        }

        if (isSpeedBoosted)
        {
            speedMultiplier *= 1.75f;
        }
        float currentSpeed = baseSpeed * speedMultiplier;

        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;
            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos, currentSpeed));
                    audioManager.PlaySFX(audioManager.steps);
                }
            }
        }
        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.E))
            Interact();
    }
    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos + new Vector3(0, -0.5f), 0.2f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos, float speed)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    public void ToggleSpeedBoost(bool enabled)
    {
        isSpeedBoosted = enabled;
        spriteRenderer.sprite = enabled ? boostedSprite : normalSprite;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos + new Vector3(0, -0.5f), 0.2f, invisibleWallsLayer | interactableLayer) != null)
        {
            return false;
        }
        return true;
    }
}

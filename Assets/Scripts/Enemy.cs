
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float tick = 0.2f;
    [SerializeField] protected float enterDamage = 10f;
    [SerializeField] protected float stayDamage = 5f;
    [SerializeField] protected float maxHp = 50f;
    [SerializeField] protected float enemyMoveSpeed = 3f;
    [SerializeField] private Image hpBar;

    [SerializeField] private float dieAnimationTime = 1f;
    protected Animator animator;
    protected bool isDead = false;

    protected Transform targetPlayer;
    protected float currentHp;
    protected PlayerController player;
    protected bool playerInside;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();

        player = FindAnyObjectByType<PlayerController>();

        if (player != null)
        {
            Transform targetChild = player.transform.Find("targetPlayer");
            if (targetChild != null)
            {
                targetPlayer = targetChild;
            }
            else
            {
                Debug.LogWarning("Không tìm thấy 'targetPlayer' trên Player. Sẽ ngắm vào gốc (pivot).");
                targetPlayer = player.transform;
            }
        }
        else
        {
            Debug.LogError(gameObject.name + ": Không tìm thấy Player trong Scene!");
        }

        currentHp = maxHp;
        UpdateHpBar();
    }

    protected virtual void Update()
    {
        MoveToPlayer();
    }

    protected void MoveToPlayer()
    {
        if (targetPlayer != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, enemyMoveSpeed * Time.deltaTime);
        }
        FlipEnemy();
    }

    protected void FlipEnemy()
    {
        if (player != null)
        {
            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();

        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("Hurt");
            }
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        this.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        Destroy(gameObject, dieAnimationTime);
    }

    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}
using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class EnemyEntity : MonoBehaviour
{
    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;

    [SerializeField] private int maxHealth;
    [SerializeField] private int enemyDamageAmount;

    private PolygonCollider2D _polygonCollider2D;
    private CapsuleCollider2D _capsuleCollider2D;
    private EnemyAI _enemyAI;

    private int _currentHealth;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform, enemyDamageAmount);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    public void PolygonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    public void PolygonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _polygonCollider2D.enabled = false;
            _capsuleCollider2D.enabled = false;
            _enemyAI.SetDeathState();

            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

}

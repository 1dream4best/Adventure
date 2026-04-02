using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class EnemyVisual : MonoBehaviour
{
    //private static readonly int Die = Animator.StringToHash(IS_DIE);
    //private static readonly int TakeHit = Animator.StringToHash(TAKE_HIT);
    //private static readonly int Running = Animator.StringToHash(IS_RUNNING);
    //private static readonly int SpeedMultiplier = Animator.StringToHash(CHASING_SPEED_MULTIPLIER);
    //private static readonly int Attack = Animator.StringToHash(ATTACK);

    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private GameObject enemyShadow;
    [SerializeField] private EnemyEntity enemyEntity;
    private Animator _animator;

    private const string IS_RUNNING = "IsRunning";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string TAKE_HIT = "TakeHit";
    private const string IS_DIE = "IsDie";


    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit += _enemyEntity__OnTakeHit;
        enemyEntity.OnDeath += _enemyEntity__OnDeath;
    }

    private void _enemyEntity__OnDeath(object sender, EventArgs e)
    {
        //_animator.SetBool(Die, true);
        _animator.SetBool(IS_DIE, true);
        _spriteRenderer.sortingOrder = -1;
        enemyShadow.SetActive(false);
    }

    private void _enemyEntity__OnTakeHit(object sender, EventArgs e)
    {
        //_animator.SetTrigger(TakeHit);
        _animator.SetTrigger(TAKE_HIT);
    }

    private void Update()
    {
        //_animator.SetBool(Running, enemyAI.IsRunning);
        //_animator.SetFloat(SpeedMultiplier, enemyAI.GetRoamingAnimationSpeed());
        _animator.SetBool(IS_RUNNING, enemyAI.IsRunning);
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, enemyAI.GetRoamingAnimationSpeed());
    }

    public void TriggerAttackAnimationTurnOff()
    {
        enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        enemyEntity.PolygonColliderTurnOn();
    }
    private void _enemyAI_OnEnemyAttack(object sender, EventArgs e)
    {
        //_animator.SetTrigger(Attack);
        _animator.SetTrigger(ATTACK);
    }

    private void OnDestroy()
    {
        enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit -= _enemyEntity__OnTakeHit;
        enemyEntity.OnDeath -= _enemyEntity__OnDeath;
    }
}

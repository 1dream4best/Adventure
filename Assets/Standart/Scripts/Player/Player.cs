using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float damageRecoveryTime = 0.5f;
    [SerializeField] private int maxHealth = 10;
    [Header ("Dash Settings")]
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private int dashSpeed = 4;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float dashCoolDownTime = 0.3f;

    private Vector2 _inputVector;

    private Rigidbody2D _rigidBody;
    private KnockBack _knockBack;
    private Camera _mainCamera;

    private readonly float _minMovingSpeed = 0.1f;
    private float _initialMovingSpeed;

    private int _currentHealth;

    private bool _isRunning = false;
    private bool _CanTakeDamage;
    private bool _isAlive;
    private bool _isDashing;

    private void Awake()
    {
        Instance = this;
        _rigidBody = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();

        _mainCamera = Camera.main;

        _initialMovingSpeed = moveSpeed;
    }

    private void Start()
    {
        _currentHealth = maxHealth;
        _CanTakeDamage = true;
        _isAlive = true;
        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;
        GameInput.Instance.OnPlayerDash += Player_OnPlayerDash;

    }

    private void Update()
    {
        _inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (_knockBack.IsGettingKnockedBack)
            return;

        HandleMovement();
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (_CanTakeDamage && _isAlive)
        {
            _CanTakeDamage = false;
            _knockBack.GetKnockBack(damageSource);
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);

            OnFlashBlink?.Invoke(this, EventArgs.Empty);

            StartCoroutine(DamageRecovryRoutine());
        }

        DetectDeath();
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = _mainCamera.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public bool IsAlive() => _isAlive;

    private IEnumerator DamageRecovryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _CanTakeDamage = true;
    }

    private void DetectDeath()
    {
        if (_currentHealth == 0 && _isAlive)
        {
            _isAlive = false;
            _knockBack.StopKnockBackMovement();
            GameInput.Instance.DisableMovement();

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }          
    }

    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void Player_OnPlayerDash(object sender, EventArgs e)
    {
        Dash();
    }

    private void Dash()
    {
        if(!_isDashing)
            StartCoroutine(DashRoutine());
    }    

    private IEnumerator DashRoutine()
    {
        _isDashing = true;
        moveSpeed *= dashSpeed;
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);

        trailRenderer.emitting = false;
        moveSpeed = _initialMovingSpeed;

        yield return new WaitForSeconds(dashCoolDownTime);
        _isDashing = false;
    }

    private void HandleMovement()
    {
        _rigidBody.MovePosition(_rigidBody.position + _inputVector * (moveSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(_inputVector.x) > _minMovingSpeed || Mathf.Abs(_inputVector.y) > _minMovingSpeed)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttack -= Player_OnPlayerAttack;
    }
}

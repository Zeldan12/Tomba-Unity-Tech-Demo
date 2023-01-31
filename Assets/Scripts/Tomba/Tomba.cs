using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tomba : MonoBehaviour {
    public enum JumpInputType {
        JustPressed,
        Held,
        NotPressed
    }

    private TombaState _currentState;

    [SerializeField]
    private int _maxHealth = 4;
    [SerializeField]
    private int _health = 4;
    private int _lifes = 3;
    private int _limitMaxHealth = 16;

    private int _score = 0;
    private bool _hit = false;
    private int _hitDirection;
    [SerializeField]
    private float _invunerabilityDuration = 5f;
    private float _invunerabilityTimer = 0f;
    [SerializeField]
    private Vector2 _hitKnockback;
    [SerializeField]
    private float _hitGroundMove = 2f;

    #region Horizontal Movement
    [Header("Horizontal Movement")]
    [SerializeField]
    private float _horizontalSpeed = 0;
    [SerializeField]
    private float _maxRunSpeed = 12, _acceleration = 20, _decceleration = 20, _turnDecceleration = 30;
    [SerializeField]
    private LayerMask _pushMask;
    [SerializeField]
    private float _pushForce = 5;
    private Coroutine _walkSoundCoroutine;
    #endregion

    #region Jump
    [Header("Vertical Movement")]
    [SerializeField]
    private float _jumpSpeed = 10;
    [SerializeField]
    private float _maxJTimer = 10, _groundedRadios = 2;
    [SerializeField]
    private Transform _groundedPosition;
    [SerializeField]
    LayerMask _groundMask;
    #endregion

    #region Dash Movement
    [Header("Dash")]
    [SerializeField]
    private float _maxDashSpeed = 12;
    [SerializeField]
    private float _dashAcceleration = 20, _dashDecceleration = 20, _dashTurnDecceleration = 30, _dashJumpSpeed = 10, _dashMaxJTimer = 10;
    private bool _isDashing = false;
    #endregion

    #region WallClimb
    [Header("WallClimb")]
    [SerializeField]
    LayerMask _wallMask;
    [SerializeField]
    private Transform _wallSensorPosition, _ledgeSensorPosition;
    [SerializeField]
    private float _wallSensorRadios = 0.5f, _climbUpMoveSpeed = 10, _climbDownMoveSpeed = 10, _wallJumpOffset = 2;
    [SerializeField]
    private float _ledgeSensorRadious = 0.1f;
    [SerializeField]
    private float _movementDegree = 2f, _maxRotationTimer = 2f, _rotationFrequency;
    [SerializeField]
    private Transform _body;
    [SerializeField]
    private Collider2D _ledgeAdjuster;
    #endregion

    #region Properties

    #region Horizontal Movement
    public bool Turn { get; set; }
    public float HorizontalSpeed { get => _horizontalSpeed; set => _horizontalSpeed = value; }
    public float MaxRunSpeed { get => _maxRunSpeed; }
    public float Acceleration { get => _acceleration; }
    public float Decceleration { get => _decceleration; }
    public float TurnDecceleration { get => _turnDecceleration; }
    public float PushForce { get => _pushForce; }
    #endregion

        #region Jump
        public float JumpSpeed {get => _jumpSpeed; }
        public float MaxJTimer { get => _maxJTimer; }
        #endregion


        #region Dash Movement
        public float MaxDashSpeed { get => _maxDashSpeed; }
        public float DashAcceleration { get => _dashAcceleration; }
        public float DashDecceleration { get => _dashDecceleration; }
        public float DashTurnDecceleration { get => _dashTurnDecceleration; }
        public float DashJumpSpeed { get => _dashJumpSpeed; }
        public float DashMaxJTimer { get => _dashMaxJTimer; }
        public bool IsDashing { get => _isDashing; set => _isDashing = value; }
        public ParticleSystem DashParticleSystem { get => _dashParticleSystem; }
        #endregion

        #region WallClimb
        public float ClimbUpMoveSpeed { get => _climbUpMoveSpeed; }
        public float ClimbDownMoveSpeed { get => _climbDownMoveSpeed; }
        public float WallJumpOffset { get => _wallJumpOffset; }
        public float MovementDegree { get => _movementDegree; }
        public float MaxRotationTimer { get => _maxRotationTimer; }
        public float RotationFrequency { get => _rotationFrequency; }
        public Collider2D LedgeAdjuster { get => _ledgeAdjuster; }
        #endregion

    public Vector2 HitKnockback { get => _hitKnockback; }
        public float HitDirection { get => _hitDirection; }
        public float HitGroundMove { get => _hitGroundMove; }
        public int Health { get => _health; }
        public int MaxHealth { get => _maxHealth; }
    #region Inputs
    public float HorizontalInput { get; private set; }
        public float VerticalInput { get; private set; }
        public bool DashInput { get; private set; }
        public JumpInputType JumpInput { get; private set; }

                #endregion

        public bool Grounded { get; private set; }
        /*public bool OnWall { get; private set; }
        public bool OnLedge { get; private set; }
        public GameObject PushObject { get; private set; }*/
        public bool AnimationEvent { get; set; }
        public TombaState CurrentState { get => _currentState; }
    #endregion

    #region Object Components
    private Animator _animatorController;
    private Rigidbody2D _rigidBody;
    private PlayerInput _playerInput;
    private ParticleSystem _dashParticleSystem;
    [SerializeField]
    private SpriteRenderer _sprite;
    TombaStateFactory _stateFactory;

    public Animator AnimatorController { get => _animatorController; }
    public Rigidbody2D RigidBody { get => _rigidBody; }
    public PlayerInput PlayerInput { get => _playerInput; }
    public Transform Body { get => _body; }
    #endregion

    void Start()
    {
            _stateFactory = new TombaStateFactory(this);
            UIManager.Instance.SetUp(this);
            UIManager.Instance.UpdateScore(_score);

            _animatorController = GetComponent<Animator>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();

            _dashParticleSystem = GetComponent<ParticleSystem>();
            _dashParticleSystem.Stop();

            _currentState = _stateFactory.GetState(TombaStateType.Fall);
            _currentState.OnEnter(null);
    }

    void Update()
    {
        HorizontalInput = _playerInput.actions["HorizontalMove"].ReadValue<float>();
        VerticalInput = _playerInput.actions["VerticalMove"].ReadValue<float>();
        DashInput = _playerInput.actions["Dash"].ReadValue<float>() == 1;
        

        if (_playerInput.actions["Jump"].WasPressedThisFrame()) {
            JumpInput = JumpInputType.JustPressed;
        }else if (_playerInput.actions["Jump"].IsPressed()) {
            JumpInput = JumpInputType.Held;
        } else {
            JumpInput = JumpInputType.NotPressed;
        }

        Grounded = Physics2D.OverlapCircle(_groundedPosition.position, _groundedRadios, _groundMask) != null;

        TombaStateType newState = _currentState.Update();
        if (_hit) {
            _hit = false;
            SoundManager.Instance.PlaySound(SoundType.TombaHit, 1f);
            _body.gameObject.layer = LayerMask.NameToLayer("InvunerablePlayer");

            if (_health != 0) {
                StartCoroutine(Invunerability());
            }
            transform.rotation = new Quaternion(transform.rotation.x, _hitDirection == 1 ? 180 : 0, transform.rotation.z, transform.rotation.w);
            _horizontalSpeed = 0;
            _rigidBody.velocity = Vector2.zero;
            newState = TombaStateType.HitAir;
        }

        if (newState != TombaStateType.None) {
            TombaState previousState = _currentState;
            _currentState.OnExit();
            _currentState = _stateFactory.GetState(newState);
            _currentState.OnEnter(previousState);
        }

        _animatorController.SetFloat("HorizontalSpeed", Mathf.Abs(_horizontalSpeed));
    }

    #region Health
    public void TakeDamage(int ammount, int direction) {
        _health -= ammount;
        if (_health < 0) {
            _health = 0;
        }
        UIManager.Instance.UpdateHealth(_health);
        _hit = true;
        _hitDirection = direction;
    }
    public void Heal(int ammount) {
        if (_health == _maxHealth) {
            return;
        }
        _health += ammount;
        if (_health > _maxHealth) {
            _health = _maxHealth;
        }
        UIManager.Instance.UpdateHealth(_health);
    }
    public void IncreaseMaxHealth() {
        _maxHealth++;
        if (_maxHealth > _limitMaxHealth) {
            _maxHealth = _limitMaxHealth;
        }
        UIManager.Instance.IncreaseMaxHealth();
        _health = _maxHealth;
        UIManager.Instance.UpdateHealth(_health);
    }

    private IEnumerator Invunerability() {
        while (_invunerabilityTimer < _invunerabilityDuration) {
            _invunerabilityTimer += Time.deltaTime;
            _sprite.enabled = !_sprite.enabled;
            yield return null;
        }

        _body.gameObject.layer = LayerMask.NameToLayer("Player");
        _sprite.enabled = true;
        _invunerabilityTimer = 0;
    }
    #endregion

    public void TombaAnimationEvent() {
        AnimationEvent = true;
    }

    public void AddScore(int score) {
        _score += score;
        if (_score > 999999) {
            _score = 999999;
        }else if (_score < 0) {
            _score = 0;
        }
        UIManager.Instance.UpdateScore(_score);
    }

    #region WalkSound
    public void StartWalkSound() {
        _walkSoundCoroutine = StartCoroutine(WalkSound());
    }

    public void StopWalkSound() {
        if (_walkSoundCoroutine != null) {
            StopCoroutine(_walkSoundCoroutine);
        }
    }

    private IEnumerator WalkSound() {
        yield return new WaitForSeconds(0.25f);
        while (true) {
            SoundManager.Instance.PlaySound(SoundType.Land, 0.5f);
            yield return new WaitForSeconds(0.25f);
        }
    }
    #endregion

    #region Sensors

    public bool CheckWall() {
        return Physics2D.OverlapCircle(_wallSensorPosition.position, _wallSensorRadios, _wallMask) != null;
    }

    public bool CheckLedge() {
        return Physics2D.OverlapCircle(_ledgeSensorPosition.position, _ledgeSensorRadious, _wallMask) == null;
    }

    public bool CheckPush() {
        return Physics2D.OverlapCircle(_wallSensorPosition.position, _wallSensorRadios, _pushMask) != null;
    }

    #endregion
}

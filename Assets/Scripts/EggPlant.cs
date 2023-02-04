using System.Collections;
using UnityEngine;

public class EggPlant : MonoBehaviour {
    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private float _sensorDistance, _spriteChangeLimit, _rotationSpeed;
    [SerializeField]
    private float _groundedRadios = 2;
    [SerializeField]
    private Transform _groundedPosition;

    [SerializeField]
    private LayerMask _playerMask, _groundMask;
    [SerializeField]
    private Sprite _idleSprite, _biteSprite, _fallSprite;

    private float rotation = 0;
    private Coroutine _coroutine;
    private bool _jumping = false, _grounded = true, _falling = false;
    private Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;

    private void Start() {
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _coroutine = StartCoroutine(Rotate());
    }

    void Update() {
        if (_jumping) {
            Jumping();
        }
        if (_falling) {
            Fall();
        }
        if (_grounded) {
            Grounded();
        }
    }

    private void Jumping() {
        if (_rigidBody.velocity.y <= _spriteChangeLimit && _rigidBody.velocity.y > 0) {
            _spriteRenderer.sprite = _biteSprite;
        }
        if (_rigidBody.velocity.y <= 0) {
            _jumping = false;
            _falling = true;
        }
    }

    private void Fall() {
        if (_rigidBody.velocity.y <= -_spriteChangeLimit) {
            _spriteRenderer.sprite = _fallSprite;
        }
        bool grounded = Physics2D.OverlapCircle(_groundedPosition.position, _groundedRadios, _groundMask) != null;
        if (grounded) {
            _falling = false;
            _grounded = true;
            _spriteRenderer.sprite = _idleSprite;
            _coroutine = StartCoroutine(Rotate());
            _rigidBody.velocity = Vector3.zero;
        }
    }

    private void Grounded() {
        Collider2D collider = Physics2D.Linecast(transform.position, transform.position + (Vector3.up * _sensorDistance), _playerMask).collider;
        if (collider != null) {
            if (collider.GetComponentInParent<Tomba>() != null) {
                StopCoroutine(_coroutine);
                transform.rotation = Quaternion.identity;
                _rigidBody.velocity = new Vector2(0, _jumpForce);
                _grounded = false;
                _jumping = true;
            }
        }
    }

    private IEnumerator Rotate() {
        rotation = 0;
        while (true) {
            rotation += 0.1f;
            float degree = 25 * Mathf.Sin(rotation);
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, degree);
            yield return new WaitForSeconds(_rotationSpeed);
        }

    }
}

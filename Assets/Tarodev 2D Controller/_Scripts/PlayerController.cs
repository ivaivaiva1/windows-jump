using System;
using UnityEngine;

namespace TarodevController
{
    /// <summary>
    /// Hey!
    /// Tarodev here. I built this controller as there was a severe lack of quality & free 2D controllers out there.
    /// I have a premium version on Patreon, which has every feature you'd expect from a polished controller. Link: https://www.patreon.com/tarodev
    /// You can play and compete for best times here: https://tarodev.itch.io/extended-ultimate-2d-controller
    /// If you hve any questions or would like to brag about your score, come to discord: https://discord.gg/tarodev
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;
        private float _fallTimer = 0f;
        private const float MaxFallTime = 10f;


        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
            SpawnPlayer();
        }

        public void SpawnPlayer()
        {
            _rb.velocity = Vector2.zero;
            _frameVelocity = Vector2.zero;
            _frameInput = new FrameInput();

            _jumpToConsume = false;
            _bufferedJumpUsable = false;
            _endedJumpEarly = false;
            _coyoteUsable = false;
            _timeJumpWasPressed = 0;
            _time = 0;

            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            CheckFallDeath();
            GatherInput();
        }

        private void CheckFallDeath()
        {
            if (!_grounded && _rb.velocity.y < 0)
            {
                _fallTimer += Time.fixedDeltaTime;

                if (_fallTimer >= MaxFallTime)
                {
                    _fallTimer = 0;
                    Player.Instance.Die();
                }
            }
            else
            {
                _fallTimer = 0f; // Reset se encostar no ch�o ou parar de cair
            }
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();
            
            ApplyMovement();
        }

        #region Collisions
        
        private float _frameLeftGrounded = float.MinValue;
        public bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Faz os casts para detectar ch�o e teto
            RaycastHit2D groundRay = Physics2D.CapsuleCast(
                _col.bounds.center, _col.size, _col.direction, 0,
                Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);

            RaycastHit2D ceilingRay = Physics2D.CapsuleCast(
                _col.bounds.center, _col.size, _col.direction, 0,
                Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

            // Verifica se bateu em algo que N�O � trigger
            bool groundHit = groundRay && !groundRay.collider.isTrigger;
            bool ceilingHit = ceilingRay && !ceilingRay.collider.isTrigger;

            // Se bateu no teto, zera o impulso para cima
            if (ceilingHit)
                _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            // Caiu no ch�o
            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Saiu do ch�o
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _preventJumpHold;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0 && !_preventJumpHold) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if (_grounded || CanUseCoyote) ExecuteJump();
              
            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            SoundController.Instance.PlaySfxOneShot(SoundController.SfxType.Pulo);
            _fallTimer = 0f;
            _endedJumpEarly = false;
            _preventJumpHold = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            Jumped?.Invoke();
        }

        public void Bounce(float force, float holdMultiplier = 1.3f)
        {
            _fallTimer = 0f;
            _endedJumpEarly = false;
            _preventJumpHold = true;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = 0;

            if (_frameInput.JumpHeld)
            {
                force *= holdMultiplier;
            }

            _frameVelocity.y += force;
        }


        public void DodoSpit(float verticalForce, float horizontalForce)
        {
            _fallTimer = 0f;
            _endedJumpEarly = false;
            _preventJumpHold = true;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity = Vector2.zero;

            _frameVelocity.y += verticalForce;
            _frameVelocity.x += horizontalForce;
        }

        public void HorizontalBounce(float force)
        {
            _frameVelocity.x = force;
        }



        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        private void ApplyMovement() => _rb.velocity = _frameVelocity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }

}
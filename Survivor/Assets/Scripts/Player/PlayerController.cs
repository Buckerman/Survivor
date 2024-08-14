using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Entities.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float rotationSpeed = 20f;
        [SerializeField] private float climbSpeed = 3f;
        [SerializeField] private float moveTowardsDistance = 1.5f;
        [SerializeField] private float edgeDetectionDistance = 1f;
        [SerializeField] public VariableJoystick joystick;

        public float magnitude;
        private CharacterController _controller;
        private Animator _animator;

        private IPlayerState _currentState;

        private bool isClimbing = false;
        private bool isJumping = false;
        public bool IsClimbing { get => isClimbing; set => isClimbing = value; }
        public bool IsJumping { get => isJumping; set => isJumping = value; }

        public float jumpHeight = 2f;
        public float gravity = -9.81f;

        private Vector3 velocity;
        private Vector3 jumpDirection;

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SetState(new IdleState());
        }

        void Update()
        {
            _currentState.Update();

            if (IsJumping)
            {
                HandleAutoJump();
            }
            else if (IsClimbing)
            {
                HandleClimbing();
            }
            else
            {
                HandleMoveInput();
                CheckEdgeAndJump();
            }
        }

        public void SetState(IPlayerState newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }
            _currentState = newState;
            _currentState.Enter(this);
        }

        public void SetAnimation(string parameter, bool state)
        {
            _animator.SetBool(parameter, state);
        }

        private void HandleClimbing()
        {
            Vector3 climbDirection = Vector3.up;
            _controller.Move(climbDirection * climbSpeed * Time.deltaTime);
        }

        private void HandleMoveInput()
        {
            ApplyGravity();

            Vector3 moveDirection = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y).normalized;
            magnitude = moveDirection.sqrMagnitude;

            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotationSpeed);
            }

            _controller.Move(moveDirection * speed * Time.deltaTime);
            _controller.Move(velocity * Time.deltaTime);
        }

        private void ApplyGravity()
        {
            if (!_controller.isGrounded)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            else
            {
                if (velocity.y < 0)
                {
                    velocity.y = -2f;
                }
            }
        }

        private void CheckEdgeAndJump()
        {
            if (_controller.isGrounded)
            {
                isJumping = false;
                Vector3 origin = transform.position;
                origin.y += 1.1f;
                Ray ray = new Ray(origin, Vector3.down);
                RaycastHit hit;

                if (!Physics.Raycast(ray, out hit, edgeDetectionDistance))
                {
                    CheckForPlatform();
                }
            }
        }

        private void PrepareAutoJump()
        {
            IsJumping = true;
            Vector3 forwardDirection = transform.forward;
            jumpDirection = forwardDirection.normalized * 0.5f;
            jumpDirection.y = 0;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        private void HandleAutoJump()
        {
            ApplyGravity();

            _controller.Move(jumpDirection * speed * Time.deltaTime);
            _controller.Move(velocity * Time.deltaTime);

            if (_controller.isGrounded && velocity.y < 0)
            {
                IsJumping = false;
            }
        }

        private void CheckForPlatform()
        {
            if (currentPlatform == null || !currentPlatform.gameObject.activeInHierarchy)
            {
                isJumping = false;
            }
        }

        private Transform currentClimbable;
        private Transform currentPlatform;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Building") || other.CompareTag("Platform"))
            {
                IsClimbing = true;
                currentClimbable = other.transform;

                Vector3 directionToLook = other.transform.position - transform.position;
                directionToLook.y = 0;
                if (directionToLook != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(directionToLook);
                    transform.rotation = lookRotation;
                }
            }
        }

        private IEnumerator PlatformRespawnTimer(Collider other)
        {
            yield return new WaitForSeconds(3.5f);
            other.transform.parent.gameObject.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if ((other.CompareTag("Building") || other.CompareTag("Platform")) && other.transform == currentClimbable)
            {
                IsClimbing = false;

                Vector3 moveTowardsDirection = (currentClimbable.position - transform.position).normalized;
                moveTowardsDirection.y = 0;

                Vector3 moveAmount = moveTowardsDirection * moveTowardsDistance;

                Vector3 targetPosition = transform.position + moveAmount;
                Ray ray = new Ray(targetPosition + Vector3.up * 0.5f, Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1f))
                {
                    _controller.Move(moveAmount);
                }

                currentClimbable = null;
            }

            if (other.CompareTag("canJump"))
            {
                currentPlatform = other.transform;
                isJumping = true;
                PrepareAutoJump();
                StartCoroutine(PlatformRespawnTimer(other));
            }
        }
    }
}

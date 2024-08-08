using System.Collections;
using UnityEngine;

namespace Entities.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float climbSpeed = 3f;
        [SerializeField] private float moveTowardsDistance = 1.25f;
        [SerializeField] private float edgeDetectionDistance = 0.5f;

        private CharacterController _controller;
        private Animator _animator;

        private IPlayerState _currentState;

        private bool isClimbing = false;
        public bool IsClimbing { get => isClimbing; set => isClimbing = value; }

        private bool canJump = false;
        public float jumpHeight = 3f;
        public float gravity = -9.81f;
        private Vector3 velocity;

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

            if (IsClimbing)
            {
                HandleClimbing();
            }
            else
            {
                HandleKeyboardInput();
                CheckEdgeAndJump();
            }
        }

        public void SetState(IPlayerState newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }
            Debug.Log("Entering State: " + newState.GetType().Name);
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

        private void HandleKeyboardInput()
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

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(moveX, 0, moveZ);
            moveDirection.Normalize();

            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotationSpeed);
            }

            _controller.Move(moveDirection * speed * Time.deltaTime);
            _controller.Move(velocity * Time.deltaTime);
        }

        private void CheckEdgeAndJump()
        {
            if (_controller.isGrounded)
            {
                Vector3 origin = transform.position;
                origin.y += 1.1f;
                Ray ray = new Ray(origin, Vector3.down);
                RaycastHit hit;

                if (!Physics.Raycast(ray, out hit, edgeDetectionDistance))
                {
                    CheckForPlatform();
                    if (canJump)
                    {
                        PerformJump();
                    }
                }
            }
        }

        public void PerformJump()
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            canJump = false;
        }

        private void CheckForPlatform()
        {
            if (currentPlatform == null || !currentPlatform.gameObject.activeInHierarchy)
            {
                canJump = false;
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
                canJump = false;
            }

            if (other.CompareTag("canJump"))
            {
                currentPlatform = other.transform;
                canJump = true;
                StartCoroutine(PlatformRespawnTimer(other));
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
                _controller.Move(moveAmount);

                currentClimbable = null;
            }
        }
    }
}

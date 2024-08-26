using QuangDM.Common;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Entities.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [SerializeField] private float speed = 5f;
        [SerializeField] private float rotationSpeed = 20f;
        [SerializeField] private float climbSpeed = 3f;
        [SerializeField] private float moveTowardsDistance = 2f;
        [SerializeField] private float edgeDetectionDistance = 1f;

        public float magnitude;
        private CharacterController _controller;
        private Animator _animator;
        private PlayerShooting _playerShooting;
        private VariableJoystick joystick;


        private IPlayerState _currentState;

        private bool isClimbing = false;
        public bool IsClimbing { get => isClimbing; set => isClimbing = value; }
        private bool jumpAnimationPlaying = false;
        public VariableJoystick Joystick { get => joystick; set => joystick = value; }

        public float jumpHeight = 2f;
        public float gravity = -9.81f;

        private Vector3 velocity;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            _controller = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _playerShooting = GetComponent<PlayerShooting>();
        }

        private void Start()
        {
            SetState(new IdleState());

            Observer.Instance.AddObserver("Joy", Joy);
        }

        private void Joy(object data)
        {
            joystick.OnPointerUp2(); //custom made
        }

        void FixedUpdate()
        {
            _currentState.Update();

            if (IsClimbing)
            {
                HandleClimbing();
            }
            else
            {
                HandleMoveInput();
                CheckEdgeAndJump();
                _playerShooting.enabled = true;
            }
        }

        public void SetState(IPlayerState newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }
            _currentState = newState;
            _currentState.Enter();
        }

        public void SetAnimation(string parameter, bool state)
        {
            _animator.SetBool(parameter, state);
        }

        private void HandleClimbing()
        {
            _playerShooting.enabled = false;
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

        float offsetDistance = 0.5f;
        private void CheckEdgeAndJump()
        {
            if (jumpAnimationPlaying) return;

            if (_controller.isGrounded)
            {
                Vector3 origin = transform.position + transform.forward * offsetDistance;

                Vector3 forwardDirection = transform.forward;
                Ray forwardRay = new Ray(origin, forwardDirection);
                RaycastHit forwardHit;

                Ray downwardRay = new Ray(origin, Vector3.down);
                RaycastHit downwardHit;

                if (Physics.Raycast(forwardRay, out forwardHit, edgeDetectionDistance))
                {
                    return;
                }

                if (!Physics.Raycast(downwardRay, out downwardHit, edgeDetectionDistance))
                {
                    HandleJump();
                }
            }
        }

        private void HandleJump()
        {
            ApplyGravity();

            _animator.Play("PlayerJump");
            SetAnimation("isJumping", true);
            jumpAnimationPlaying = true;

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            _controller.Move(velocity * Time.deltaTime);
        }
        public void OnJumpAnimationEnd()
        {
            jumpAnimationPlaying = false;
            SetAnimation("isJumping", false);
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

        private Transform currentClimbable;

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
        }
    }
}

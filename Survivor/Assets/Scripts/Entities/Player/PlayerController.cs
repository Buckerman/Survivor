using QuangDM.Common;
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    private PlayerShooting _playerShooting;
    private VariableJoystick _joystick;

    private IPlayerState _currentState;

    private bool isClimbing = false;
    public bool IsClimbing { get => isClimbing; set => isClimbing = value; }
    public VariableJoystick Joystick { get => _joystick; set => _joystick = value; }
    private bool jumpAnimationPlaying = false;

    public float magnitude;
    public float edgeDetectionDistance = 2f;
    public float moveTowardsDistance = 2f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private Vector3 velocity;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _playerShooting = GetComponent<PlayerShooting>();
    }

    private void Start()
    {
        SetState(new IdleState());

        Observer.Instance.AddObserver(EventName.Joy, Joy);
    }

    private void Joy(object data)
    {
        _joystick.OnPointerUp2(); //custom made
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
        _controller.Move(climbDirection * Player.Instance.climbSpeed * Time.deltaTime);
    }

    private void HandleMoveInput()
    {
        ApplyGravity();

        Vector3 moveDirection = new Vector3(_joystick.Direction.x, 0f, _joystick.Direction.y).normalized;
        magnitude = moveDirection.sqrMagnitude;

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * Player.Instance.rotationSpeed);
        }

        _controller.Move(Player.Instance.movementSpeed * Time.deltaTime * moveDirection);
        _controller.Move(velocity * Time.deltaTime);
    }

    float offsetDistance = 0.5f;
    private void CheckEdgeAndJump()
    {
        if (jumpAnimationPlaying) return;

        if (_controller.isGrounded)
        {
            Vector3 origin = transform.position;

            Vector3 forwardDirection = transform.forward;
            Ray forwardRay = new Ray(origin, forwardDirection);
            RaycastHit forwardHit;

            Ray downwardRay = new Ray(origin + transform.forward * offsetDistance, Vector3.down);
            RaycastHit downwardHit;

            bool isWallInFront = Physics.Raycast(forwardRay, out forwardHit, edgeDetectionDistance);
            bool isEdge = !Physics.Raycast(downwardRay, out downwardHit, edgeDetectionDistance);

            if (!isWallInFront && isEdge)
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
